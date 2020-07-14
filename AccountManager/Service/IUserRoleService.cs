using System.Collections.Generic;
using AccountManager.DB.Entity;
using Infrastructure;

namespace AccountManager.Service
{
    public interface IUserRoleService : IBaseService<UserRole>
    {
        List<User> GetByRoleId(string roleId, int pageIndex = 1, int pageSize = 10);
        List<Role> GetByUserId(string userId, int pageIndex = 1, int pageSize = 10);
        Result<int> DeleteByRoleId(string roleId);

        Result<int> DeleteByUserId(string userId);
    }
}
