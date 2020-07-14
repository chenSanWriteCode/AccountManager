using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using Infrastructure;
using log4net;

namespace AccountManager.Filter
{
    public class JWTAttribute : AuthorizeAttribute
    {
        private ILog log = LogManager.GetLogger(nameof(JWTAttribute));
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.TryGetValues("token", out IEnumerable<string> data))
            {
                var token = data.FirstOrDefault();
                if (string.IsNullOrEmpty(token))
                {
                    return false;
                }
                var result = JWTUtil.Validata(token);
                if (result.Success)
                {
                    //dynamic tokenContent = result.Data;
                    //var role = tokenContent.Role;
                    //log.Info($"{role}角色登录了系统");
                    return true;
                }
            }
            return false;
        }
    }
}