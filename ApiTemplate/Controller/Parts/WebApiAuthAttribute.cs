using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace ApiTemplate.Controller.Parts
{
    public class WebApiAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization != null)
                {
                    var encryptedKey = HttpUtility.UrlDecode(actionContext.Request.Headers.Authorization.ToString());

                    if (!ValidateToken(encryptedKey))
                    {
                        var rm = new ResponseModel<string>();
                        rm.StatusCode = HttpStatusCode.Gone;

                        var countresult = JsonConvert.SerializeObject(rm, Newtonsoft.Json.Formatting.Indented);
                        actionContext.Response = new HttpResponseMessage { Content = new StringContent(countresult, Encoding.GetEncoding("UTF-8"), "text/plain") };
                    }
                }
                else
                {
                    var rm = new ResponseModel<string>();
                    rm.StatusCode = HttpStatusCode.PreconditionFailed;

                    var countresult = JsonConvert.SerializeObject(rm, Newtonsoft.Json.Formatting.Indented);
                    actionContext.Response = new HttpResponseMessage { Content = new StringContent(countresult, Encoding.GetEncoding("UTF-8"), "text/plain") };
                }
            }
            catch (Exception)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            base.OnActionExecuting(actionContext);
        }

        public bool ValidateToken(string token)
        {
            if (ConfigurationManager.AppSettings["swaggerAuth"].Equals(token))
            {
                return true;
            }
            return false;
        }
    }
}
