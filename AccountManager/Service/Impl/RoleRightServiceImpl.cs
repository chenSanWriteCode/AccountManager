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
    public class RoleRightServiceImpl : BaseServiceImpl<RoleRight>, IRoleRightService
    {
        public Result<int> DeleteByRightId(string id)
        {
            DBContext context = new DBContext();
            var result = context.DB.UseTran(() =>
            {
                return context.RoleRightDB.AsDeleteable().Where(x => x.RightData == id).ExecuteCommand();
            });
            return ResultUtil.CreateResult(result.Data, result.ErrorMessage);
        }

        public Result<int> DeleteByRoleId(string id)
        {
            DBContext context = new DBContext();
            var result = context.DB.UseTran(() =>
            {
                return context.RoleRightDB.AsDeleteable().Where(x => x.RoleId == id).ExecuteCommand();
            });
            return ResultUtil.CreateResult(result.Data, result.ErrorMessage);
        }

        public List<Right> GetByRoleId(string roleId, int pageIndex, int pageSize)
        {
            DBContext context = new DBContext();
            return context.DB.Queryable<RoleRight, Role, Right>((rr, r, rt) => rr.RoleId == r.Id && rr.RightData == rt.Id && r.Id == roleId).Select((rr, r, rt) => rt).ToPageList(pageIndex, pageSize);

        }

        public List<Right> GetByUserId(string userId)
        {
            DBContext context = new DBContext();
            var roleId = context.DB.Queryable<UserRole, Role>((ur, r) => ur.RoleId == r.Id).OrderBy((ur, r) => r.RoleLevel).Select((ur, r) => ur).First(ur => ur.UserId == userId)?.RoleId;
            return context.DB.Queryable<RoleRight, Right>((rr, rt) => rr.RightData == rt.Id && rr.RoleId == roleId).Select((rr, rt) => rt).ToList();
        }
    }
}