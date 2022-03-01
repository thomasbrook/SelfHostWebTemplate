using Swashbuckle.Swagger;
using System;

namespace SelfHostWeb.SwaggerExtension
{
    public class SchemaFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            schema.vendorExtensions.Add("x-schema", "bar");
        }
    }
}
