using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AccountManager.DB.Entity;
using AccountManager.Service;
using Infrastructure;
using Unity;
using static Infrastructure.ConstInfo;

namespace AccountManager.Controllers
{
    public class RoleController : ApiController
    {
        private IRoleService Service;
        [Dependency]
        public IRoleRightService RRService { get; set; }
        [Dependency]
        public IUserRoleService URService { get; set; }

        public RoleController(IRoleService service)
        {
            Service = service;
        }
        // GET api/<controller>
        public async Task<IHttpActionResult> GetAsync(int pageIndex = 1, int pageSize = 10)
        {
            var result = await Service.GetAsync(pageIndex, pageSize);
            return Ok(result);
        }

        // GET api/<controller>/5
        public async Task<IHttpActionResult> GetAsync(string id)
        {
            var data = await Service.GetByIdAsync(id);
            return Ok(ResultUtil.AddNotNullData(data, ERR_NotFound_Role));
        }
        public IHttpActionResult GetByRoleName(string roleName)
        {
            var data = Service.GetByName(roleName);
            return Ok(data);
        }
        // POST api/<controller>
        public IHttpActionResult Post([FromBody]Role value)
        {
            if (value == null)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_Null));
            }
            var data = Service.GetByName(value.RoleName);
            if (data != null)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_AlreadyHave_Role));
            }
            return Ok(Service.Add(value));
        }

        // PUT api/<controller>/5
        public async Task<IHttpActionResult> PutAsync(string id, [FromBody]Role value)
        {
            //根据id查出来的model ！=null
            var data = await Service.GetByIdAsync(id);
            if (data == null)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_NotFound_Role));
            }
            //传入修改的value！=null
            if (value == null)
            {
                return Content(HttpStatusCode.BadRequest, ERR_Null);
            }
            return Ok(Service.UpdateRole(id, value));
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(string id)
        {
            string msg;
            var result = RRService.DeleteByRoleId(id);
            if (result.Success)
            {
                if (URService.DeleteByRoleId(id).Success)
                {
                    return Ok(Service.DeleteById(id));
                }
                else
                {
                    msg = result.Message;
                    
                }
            }
            else
            {
                msg = result.Message;
            }
            return Ok(ResultUtil.CreateResult<int>(msg));

        }

    }
}