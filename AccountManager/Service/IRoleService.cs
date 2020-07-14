using AccountManager.DB.Entity;
using Infrastructure;

namespace AccountManager.Service
{
    public interface IRoleService : IBaseService<Role>
    {
        Role GetByName(string name);

        Result<int> UpdateRole(string id, Role role);
    }
}
