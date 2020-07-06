using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AccountManager.DB;
using Infrastructure;
using log4net;

namespace AccountManager.Service.Impl
{
    public class BaseServiceImpl<T> : IBaseService<T> where T : BaseEntity, new()
    {
        public virtual Result<int> Add(T model)
        {
            DBContext context = new DBContext();
            return context.Add(model);
        }

        public virtual Result<int> DeleteById(string id)
        {
            DBContext context = new DBContext();
            return context.DeleteById<T>(id);
        }

        public virtual async Task<List<T>> GetAsync(int pageIndex, int pageSize)
        {
            DBContext context = new DBContext();
            return await context.GetAsync<T>(pageIndex, pageSize);
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            DBContext context = new DBContext();
            return await context.GetByIdAsync<T>(id);
        }

        protected virtual Result<int> Update(string sql)
        {
            DBContext context = new DBContext();
            return context.Update(sql);
        }
    }


}