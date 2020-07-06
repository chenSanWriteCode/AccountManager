using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AccountManager.Service;
using Unity;

namespace AccountManager.Controllers
{
    public class AccountController : ApiController
    {
        [Dependency]
        public IUserService Service { get; set; }
        [HttpGet]
        public IHttpActionResult Login(string nickName, string password)
        {
            return Ok(Service.GetByNamePassw(nickName, password));
        }
    }
}