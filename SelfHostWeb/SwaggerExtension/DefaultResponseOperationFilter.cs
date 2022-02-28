using Swashbuckle.Swagger;
using System.Web.Http.Description;

namespace SelfHostWeb.SwaggerExtension
{
    public class DefaultResponseOperationFilter : IOperationFilter
    {
        public class ErrorMessage
        {
            /// <summary>
            /// 堆栈信息
            /// </summary>
            public string Message { get; set; }
        }

        public class AuthMessage
        {
            /// <summary>
            /// 
            /// </summary>
            public string Data { get; set; }
            /// <summary>
            /// 410、权限校验失败；412、未登陆（该定义编码与HttpStatus无关，乱用）
            /// </summary>
            public int StatusCode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ExMessage { get; set; }
        }

        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            var errorSchema = schemaRegistry.GetOrRegister(typeof(ErrorMessage));
            operation.responses.Add("503", new Response
            {
                description = "ServiceUnavailable",
                schema = errorSchema
            });

            var authSchema = schemaRegistry.GetOrRegister(typeof(AuthMessage));
            operation.responses.Add("401", new Response
            {
                description = "Unauthorized",
                schema = authSchema
            });
        }
    }
}
