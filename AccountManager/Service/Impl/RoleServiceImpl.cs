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
    public class RoleServiceImpl : BaseServiceImpl<Role>, IRoleService
    {
        public Role GetByName(string name)
        {
            DBContext context = new DBContext();
            return context.RoleDB.GetSingle(x => x.RoleName == name);
        }

        public Result<int> UpdateRole(string id, Role role)
        {
            StringBuilder sql = new StringBuilder($"update web_role set lastupdateby='{role.LastUpdateBy}' ");
            if (!string.IsNullOrEmpty(role.RoleName))
            {
                sql.Append($" ,roleName='{role.RoleName}'");
            }
            sql.Append($" ,roleLevel={role.RoleLevel} where id='{id}'");
            return base.Update(sql.ToString());
        }
    }
}