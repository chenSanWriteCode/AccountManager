using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountManager.DB;
using Infrastructure;

namespace AccountManager.Service
{
    public interface IBaseService<T> where T : BaseEntity, new()
    {
        Result<int> Add(T model);
        Result<int> DeleteById(string id);
        // bool Update(string sql);
        Task<List<T>> GetAsync(int pageIndex, int pageSize);
        Task<T> GetByIdAsync(string id);
    }
}
