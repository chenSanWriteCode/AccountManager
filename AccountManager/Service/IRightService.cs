using System.Collections.Generic;
using AccountManager.DB.Entity;
using Infrastructure;

namespace AccountManager.Service
{
    public interface IRightService : IBaseService<Right>
    {
        Result<int> UpdateById(string id, Right value);
        List<Right> GetByTitle(string title, int pageIndex, int pageSize);
    }
}
