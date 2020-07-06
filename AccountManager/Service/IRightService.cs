using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountManager.DB.Entity;
using Infrastructure;

namespace AccountManager.Service
{
    public interface IRightService:IBaseService<Right>
    {
        Result<int> UpdateById(string id, Right value);
        List<Right> GetByTitle(string title,int pageIndex,int pageSize);
    }
}
