using SelfHostWeb.WebApi.Parts;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Description;

namespace SelfHostWeb.SwaggerExtension
{
    public class GlobalHttpHeaderFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="schemaRegistry"></param>
        /// <param name="apiDescription"></param>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            //var filterPipeline = apiDescription.ActionDescriptor.GetFilterPipeline(); //判断是否添加权限过滤器
            //var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Instance).Any(filter => filter is IAuthorizationFilter); //判断是否允许匿名方法 
            //是否有验证用户标记
            var isNeedAuth = apiDescription.ActionDescriptor.GetCustomAttributes<WebApiAuthAttribute>().Any();
            if (isNeedAuth)//如果有验证标记则显示 Authorization 输入框 (swagger form提交时会将这个值放入header里)
            {
                operation.parameters.Add(new Parameter { name = "Authorization", @in = "header", description = "Authorization", required = true, type = "string" });
            }
        }
    }
}
