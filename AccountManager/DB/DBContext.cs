using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using AccountManager.DB.Entity;
using Infrastructure;
using log4net;
using SqlSugar;

namespace AccountManager.DB
{
    public class DBContext
    {
        private ILog log = LogManager.GetLogger(nameof(DBContext));
        private static string connectionStr = ConfigurationManager.ConnectionStrings["AccountDataBase"].ConnectionString;
        public SqlSugarClient DB { get; }
        public SimpleClient<User> UserDB => new SimpleClient<User>(DB);
        public SimpleClient<Role> RoleDB => new SimpleClient<Role>(DB);
        public SimpleClient<UserRole> UserRoleDB => new SimpleClient<UserRole>(DB);
        public SimpleClient<RoleRight> RoleRightDB => new SimpleClient<RoleRight>(DB);
        public SimpleClient<Right> RightDB => new SimpleClient<Right>(DB);
        public DBContext()
        {
            DB = new SqlSugarClient(new ConnectionConfig()
            {
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
                ConnectionString = connectionStr
            });
            DB.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                DB.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };
        }
        public void InitTables(List<Type> entityTypeList)
        {
            DB.CodeFirst.SetStringDefaultLength(200).InitTables(entityTypeList.ToArray());
        }
        public async Task<List<T>> GetAsync<T>(int pageIndex, int pageSize) where T : BaseEntity
        {
            return await DB.Queryable<T>().ToPageListAsync(pageIndex, pageSize);
        }
        public async Task<T> GetByIdAsync<T>(string id) where T : BaseEntity
        {
            return await DB.Queryable<T>().InSingleAsync(id);
        }
        public Result<int> Add<T>(T model) where T : BaseEntity, new()
        {
            model.Id = Guid.NewGuid().ToString();
            var result = DB.Ado.UseTran(() =>
            {
                return DB.Insertable(model).ExecuteCommand();
            });
            if (!result.IsSuccess)
            {
                log.Error(result.ErrorMessage);
            }
            return ResultUtil.CreateResult<int>(result.Data, result.ErrorMessage);

        }
        public Result<int> DeleteById<T>(string id) where T : BaseEntity, new()
        {
            var result = DB.Ado.UseTran(() =>
            {
                return DB.Deleteable<T>().Where(x => x.Id == id).ExecuteCommand();
            });
            if (!result.IsSuccess)
            {
                log.Error(result.ErrorMessage);
            }
            return ResultUtil.CreateResult<int>(result.Data, result.ErrorMessage);
        }
        public Result<int> Update(string sql)
        {
            var str = sql.ToUpper();
            int index = str.IndexOf("WHERE");
            string sqlParam = $" ,LastUpdateTime='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' ";
            if (index == -1)
            {
                sql += sqlParam;
            }
            else
            {
                sql = sql.Insert(index, sqlParam);
            }

            var result = DB.Ado.UseTran(() =>
            {
                return DB.Ado.ExecuteCommand(sql);
            });
            if (!result.IsSuccess)
            {
                log.Error(result.ErrorMessage);
            }
            return ResultUtil.CreateResult<int>(result.Data, result.ErrorMessage);

        }

    }
}