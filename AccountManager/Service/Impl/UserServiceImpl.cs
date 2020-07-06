using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using AccountManager.DB;
using AccountManager.DB.Entity;
using Infrastructure;

namespace AccountManager.Service.Impl
{
    public class UserServiceImpl : BaseServiceImpl<User>, IUserService
    {
        public Result<int> DeleteByPhoneNum(string phoneNum)
        {
            DBContext context = new DBContext();
            var result = context.DB.UseTran(() =>
            {
                return context.UserDB.AsDeleteable().Where(x => x.PhoneNum == phoneNum).ExecuteCommand();
            });
            return ResultUtil.CreateResult(result.Data, result.ErrorMessage);
        }

        public User GetByNamePassw(string name, string password)
        {
            DBContext context = new DBContext();
            return context.UserDB.GetSingle(x => x.NickName == name && x.Password == password);
        }

        public User GetByPhoneNum(string phoneNum)
        {
            DBContext context = new DBContext();
            var length = phoneNum.Trim().Length;
            return context.UserDB.GetSingle(x => x.PhoneNum.Substring(x.PhoneNum.Length - length, length) == phoneNum);
        }

        public  Result<int> Update(string phoneNumOrId, User model)
        {
            StringBuilder sql = new StringBuilder($"update web_user set lastupdateby = '{model.LastUpdateBy}',");
            if (!string.IsNullOrEmpty(model.NickName))
            {
                sql.Append($" NickName='{model.NickName}',");
            }
            if (!string.IsNullOrEmpty(model.Password))
            {
                sql.Append($" password='{model.Password}',");
            }
            if (!string.IsNullOrEmpty(model.PhoneNum))
            {
                sql.Append($" PhoneNum='{model.PhoneNum}',");
            }
            if (!string.IsNullOrEmpty(model.UserName))
            {
                sql.Append($" UserName= '{model.UserName}',");
            }
            if (model.VIPLevel.HasValue)
            {
                sql.Append($" VIPLevel={model.VIPLevel.Value},");
            }
            sql.Remove(sql.Length-1, 1);
            if (long.TryParse(phoneNumOrId, out _))
            {
                sql.Append($" where phonenum='{phoneNumOrId}'");
            }
            else
            {
                sql.Append($" where Id='{phoneNumOrId}'");
            }
            return base.Update(sql.ToString());
        }
    }
}