using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using Infrastructure;

namespace AccountManager.Filter
{
    public class JWTAttribute : AuthorizeAttribute
    {
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
                    dynamic tokenContent = result.Data;
                    var roleId = tokenContent.RoleId;
                    return true;
                }
            }
            return false;
        }
    }
}