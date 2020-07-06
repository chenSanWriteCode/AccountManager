using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
