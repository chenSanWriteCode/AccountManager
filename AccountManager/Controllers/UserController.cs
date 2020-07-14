using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AccountManager.DB.Entity;
using AccountManager.Service;
using Infrastructure;
using Unity;
using static Infrastructure.ConstInfo;

namespace AccountManager.Controllers
{
    public class UserController : ApiController
    {
        [Dependency]
        public IUserService Service { get; set; }
        [Dependency]
        public IUserRoleService URService { get; set; }
        public async Task<IHttpActionResult> Get(int pageIndex = 1, int pageSize = 10)
        {
            var result = await Service.GetAsync(pageIndex, pageSize);
            return Ok(result);
        }
        public async Task<IHttpActionResult> Get(string id)
        {
            var data = await Service.GetByIdAsync(id);
            var result = ResultUtil.AddNotNullData(data);
            return Ok(result);
        }

        public IHttpActionResult GetByPhoneNum(string phoneNum)
        {
            var data = Service.GetByPhoneNum(phoneNum);
            var result = ResultUtil.AddNotNullData(data, ConstInfo.ERR_NotFound_User);
            return Ok(result);
        }

        public IHttpActionResult Post([FromBody]User user)
        {
            if (user == null)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_Null));
            }
            var data = Service.GetByPhoneNum(user.PhoneNum);
            if (data != null)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_AlreadyHave_Phone));
            }
            return Ok(Service.Add(user));
        }

        public async Task<IHttpActionResult> PutAsync(string id, [FromBody]User value)
        {
            User model;
            if (long.TryParse(id, out _))
            {
                model = Service.GetByPhoneNum(id);
            }
            else
            {
                model = await Service.GetByIdAsync(id);
            }
            if (model == null)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_NotFound_User));
            }
            if (value == null)
            {
                return Content(HttpStatusCode.BadRequest, ConstInfo.ERR_Null);
            }
            return Ok(Service.Update(id, value));

        }

        public async Task<IHttpActionResult> DeleteAsync(string id)
        {
            User model;
            if (long.TryParse(id, out _))
            {
                model = Service.GetByPhoneNum(id);
            }
            else
            {
                model = await Service.GetByIdAsync(id);
            }
            if (model == null)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_NotFound_User));
            }
            var result = URService.DeleteByUserId(model.Id);
            if (result.Success)
            {
                return Ok(Service.DeleteById(model.Id));
            }
            return Ok(ResultUtil.CreateResult<int>(result.Message));
        }
    }
}