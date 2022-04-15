using SelfHostWeb.Controller.Controller.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace SelfHostWeb.Server
{
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //1.异常日志记录（正式项目里面一般是用log4net记录异常日志）
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "——" +
                actionExecutedContext.Exception.GetType().ToString() + "：" + actionExecutedContext.Exception.Message + "——堆栈信息：" +
                actionExecutedContext.Exception.StackTrace);

            //2.返回调用方具体的异常信息
            if (actionExecutedContext.Exception is NotImplementedException)
            {
                var message = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                var result = new ResponseModel<bool>
                {
                    ExMessage = actionExecutedContext.Exception.Message,
                    Data = false,
                    StatusCode = HttpStatusCode.BadRequest
                };
                message.Content = new ObjectContent<ResponseModel<bool>>(result, new JsonMediaTypeFormatter(), "application/json");
                message.ReasonPhrase = "This Func is Not Supported";
                actionExecutedContext.Response = message;
            }
            else if (actionExecutedContext.Exception is UnauthorizedAccessException)
            {
                var message = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                var result = new ResponseModel<bool>
                {
                    ExMessage = actionExecutedContext.Exception.Message,
                    Data = false,
                    StatusCode = HttpStatusCode.BadRequest
                };
                message.Content = new ObjectContent<ResponseModel<bool>>(result, new JsonMediaTypeFormatter(), "application/json");
                message.ReasonPhrase = "Unauthorized";
                actionExecutedContext.Response = message;
            }
            else if (actionExecutedContext.Exception is TimeoutException)
            {
                var message = new HttpResponseMessage(HttpStatusCode.RequestTimeout);
                var result = new ResponseModel<bool>
                {
                    ExMessage = actionExecutedContext.Exception.Message,
                    Data = false,
                    StatusCode = HttpStatusCode.BadRequest
                };
                message.Content = new ObjectContent<ResponseModel<bool>>(result, new JsonMediaTypeFormatter(), "application/json");
                actionExecutedContext.Response = message;
            }
            else
            {
                // 异常统一返回标准返回值
                var message = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                var responseResult = new ResponseModel<bool>()
                {
                    ExMessage = actionExecutedContext.Exception.Message,
                    Data = false,
                    StatusCode = HttpStatusCode.BadRequest
                };
                message.Content = new ObjectContent<ResponseModel<bool>>(responseResult, new JsonMediaTypeFormatter(), "application/json");
                message.ReasonPhrase = "WebApiException";
                actionExecutedContext.Response = message;
            }

            base.OnException(actionExecutedContext);
        }
    }
}
