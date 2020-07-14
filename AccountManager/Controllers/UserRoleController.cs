using System.Linq;
using System.Web.Http;
using AccountManager.DB.Entity;
using AccountManager.Service;
using Infrastructure;
using Unity;
using static Infrastructure.ConstInfo;

namespace AccountManager.Controllers
{
    public class UserRoleController : ApiController
    {
        [Dependency]
        public IUserRoleService Service { get; set; }
        // GET api/<controller>
        public IHttpActionResult GetByRoleId(string roleId, int pageIndex = 1, int pageSize = 10)
        {
            return Ok(Service.GetByRoleId(roleId, pageIndex, pageSize));
        }
        public IHttpActionResult GetByuserId(string userId, int pageIndex = 1, int pageSize = 10)
        {
            return Ok(Service.GetByUserId(userId, pageIndex, pageSize));
        }

        public IHttpActionResult Post([FromBody]UserRole value)
        {
            if (value == null)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_Null));
            }
            var count = Service.GetByUserId(value.UserId, 1, 10).Count(x => x.Id == value.RoleId);
            if (count > 0)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_AlreadyHave));
            }
            return Ok(Service.Add(value));
        }

        public IHttpActionResult Delete(string id)
        {
            var data = Service.GetByIdAsync(id);
            if (data == null)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_NotFound));
            }
            return Ok(Service.DeleteById(id));
        }
    }
}