using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountManager.DB.Entity;
using Infrastructure;

namespace AccountManager.Service
{
    public interface IUserRoleService : IBaseService<UserRole>
    {
        List<User> GetByRoleId(string roleId,int pageIndex,int pageSize);
        List<Role> GetByUserId(string userId, int pageIndex, int pageSize);
        Result<int> DeleteByRoleId(string roleId);

        Result<int> DeleteByUserId(string userId);
    }
}
