using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccountManager.DB;
using AccountManager.DB.Entity;
using Infrastructure;
using SqlSugar;

namespace AccountManager.Service.Impl
{
    public class UserRoleServiceImpl : BaseServiceImpl<UserRole>, IUserRoleService
    {
        public Result<int> DeleteByRoleId(string roleId)
        {
            DBContext context = new DBContext();
            var result = context.DB.UseTran(() =>
            {
                return context.UserRoleDB.AsDeleteable().Where(x => x.RoleId == roleId).ExecuteCommand();
            });
            return ResultUtil.CreateResult(result.Data, result.ErrorMessage);
        }

        public Result<int> DeleteByUserId(string userId)
        {
            DBContext context = new DBContext();
            var result = context.DB.UseTran(() =>
            {
                return context.UserRoleDB.AsDeleteable().Where(x => x.UserId == userId).ExecuteCommand();
            });
            return ResultUtil.CreateResult(result.Data, result.ErrorMessage);
        }

        public List<User> GetByRoleId(string roleId, int pageIndex, int pageSize)
        {
            DBContext context = new DBContext();
            return context.DB.Queryable<User, UserRole>((u, ur) => u.Id == ur.UserId).Select((u, ur) => u).ToPageList(pageIndex, pageSize);
        }

        public List<Role> GetByUserId(string userId, int pageIndex, int pageSize)
        {
            DBContext context = new DBContext();
            return context.DB.Queryable<Role, UserRole>((r, ur) => r.Id == ur.RoleId && ur.UserId==userId).Select((r, ur) => r).ToPageList(pageIndex, pageSize);
        }
    }
}