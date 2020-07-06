using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountManager.DB.Entity;
using Infrastructure;

namespace AccountManager.Service
{
    public interface IRoleRightService : IBaseService<RoleRight>
    {
        List<Right> GetByRoleId(string roleId,int pageIndex,int pageSize);

        List<Right> GetByUserId(string userId);
        Result<int> DeleteByRightId(string id);
        Result<int> DeleteByRoleId(string id);
    }
}
