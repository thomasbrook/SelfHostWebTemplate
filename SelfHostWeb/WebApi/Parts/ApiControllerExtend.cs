using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SelfHostWeb.SwaggerExtension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SelfHostWeb.WebApi.Parts
{
    public static class ApiControllerExtend
    {
        public static string SerialException(this ApiController controller, SimpleException exp)
        {
            if (exp == null) return string.Empty;

            var message = string.Empty;
            if (exp.Message != null) message = string.Join(",", exp.Message);
            if (exp.StackTrace != null) message += " " + string.Join(" ", exp.StackTrace);

            return message;
        }

        public static HttpResponseMessage SerialResponseMessage<T>(this ApiController controller, ResponseModel<T> respModel, IsoDateTimeConverter timeFormat = null)
        {
            var temp = string.Empty;
            if (timeFormat == null) temp = JsonConvert.SerializeObject(respModel, Formatting.Indented);
            else temp = JsonConvert.SerializeObject(respModel, Formatting.Indented, timeFormat);

            var resp = new HttpResponseMessage
            {
                Content = new StringContent(temp, Encoding.GetEncoding("UTF-8"), "text/plain")
            };
            return resp;
        }
    }
}
