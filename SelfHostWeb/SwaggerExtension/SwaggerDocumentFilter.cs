using Swashbuckle.Swagger;
using System.Web.Http.Description;

namespace SelfHostWeb.SwaggerExtension
{
    public class SwaggerDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.vendorExtensions.Add("x-document", "foo");
        }
    }
}
