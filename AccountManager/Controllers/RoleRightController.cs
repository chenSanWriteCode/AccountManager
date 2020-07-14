using System.Linq;
using System.Web.Http;
using AccountManager.DB.Entity;
using AccountManager.Service;
using Infrastructure;
using Unity;
using static Infrastructure.ConstInfo;

namespace AccountManager.Controllers
{
    public class RoleRightController : ApiController
    {
        [Dependency]
        public IRoleRightService Service { get; set; }
        // GET api/<controller>
        public IHttpActionResult GetByRoleId(string roleId, int pageIndex = 1, int pageSize = 10)
        {
            return Ok(Service.GetByRoleId(roleId, pageIndex, pageSize));
        }

        public IHttpActionResult GetByUserId(string userId)
        {
            return Ok(Service.GetByUserId(userId));
        }

        public IHttpActionResult Post([FromBody]RoleRight value)
        {
            if (value == null)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_Null));
            }
            var count = Service.GetByRoleId(value.RoleId, 1, 1000).Count(x => x.Id == value.RightData);
            if (count >= 1)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_AlreadyHave));
            }
            return Ok(Service.Add(value));
        }



        public IHttpActionResult Delete(string id)
        {
            return Ok(Service.DeleteById(id));
        }
    }
}