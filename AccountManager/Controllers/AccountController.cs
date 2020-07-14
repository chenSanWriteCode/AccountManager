using System.Web.Http;
using AccountManager.Models;
using AccountManager.Service;
using Unity;

namespace AccountManager.Controllers
{
    [AllowAnonymous]
    public class AccountController : ApiController
    {
        [Dependency]
        public IAccountService Service { get; set; }
        [HttpPost]
        public IHttpActionResult Login([FromBody]LoginInfoMode info)
        {
            if (long.TryParse(info.Name, out long _))
            {
                return Ok(Service.LoginByPhoneNum(info.Name, info.Password));
            }
            return Ok(Service.LoginByNickName(info.Name, info.Password));
        }
    }
}