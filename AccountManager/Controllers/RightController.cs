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
    public class RightController : ApiController
    {
        [Dependency]
        public IRightService Service { get; set; }
        [Dependency]
        public IRoleRightService RRService { get; set; }

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
            return Ok(ResultUtil.AddNotNullData(data, ERR_NotFound));
        }
        public IHttpActionResult GetByTitle(string title, int pageIndex = 1, int pageSize = 10)
        {
            var data = Service.GetByTitle(title, pageIndex, pageSize);
            return Ok(data);
        }
        // POST api/<controller>
        public IHttpActionResult Post([FromBody]Right value)
        {
            if (value == null)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_Null));
            }
            var data = Service.GetByTitle(value.Title, 1, 1000);
            if (data.Count > 0)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_AlreadyHave_Right));
            }
            return Ok(Service.Add(value));
        }

        // PUT api/<controller>/5
        public async Task<IHttpActionResult> PutAsync(string id, [FromBody]Right value)
        {
            //根据id查出来的model ！=null
            var model = await Service.GetByIdAsync(id);
            if (model == null)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_NotFound));
            }

            var data = Service.GetByTitle(value?.Title, 1, 1000);
            if (data.Count > 0)
            {
                return Ok(ResultUtil.CreateResult<int>(ERR_AlreadyHave_Right));
            }

            //传入修改的value！=null
            if (value == null)
            {
                return Content(HttpStatusCode.BadRequest, ERR_Null);
            }
            return Ok(Service.UpdateById(id, value));
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(string id)
        {
            var result = RRService.DeleteByRightId(id);
            if (result.Success)
            {
                return Ok(Service.DeleteById(id));
            }
            else
            {
                return Ok(ResultUtil.CreateResult<int>(result.Message));
            }

        }
    }
}