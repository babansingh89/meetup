using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Security.Principal;
using System.Net.Http.Headers;

namespace SchoolERP_System.Models
{
    public class IdentityBasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
                else
                {
                    string authenticationtocken = actionContext.Request.Headers.Authorization.Parameter;
                    string decodedauthenticationtocken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationtocken));
                    string[] usernamepasswordArray = decodedauthenticationtocken.Split(':');
                    string username = usernamepasswordArray[0];
                    string password = usernamepasswordArray[1];

                    // DirectoryFunctions objDirFun = new DirectoryFunctions();
                    //AuthenticateUser
                    //string strAuthenticate = objDirFun.authenticateUser(username, password);

                    if (username == "admin" && password == "123")
                    //if (strAuthenticate == "Success")
                    {
                        IPrincipal principal = new GenericPrincipal(new GenericIdentity(username), new string[] { "Admin" });
                        Thread.CurrentPrincipal = principal;
                        if (HttpContext.Current != null)
                        {
                            HttpContext.Current.User = principal;
                        }
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "{\"Status\":401}", "application/json");
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}