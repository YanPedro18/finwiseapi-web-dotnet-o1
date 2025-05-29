using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Swagger.Attributes;

namespace Swagger.Filters
{
    public class SwaggerIgnoreFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context.Type == null)
                return;

            var excludedProperties = context.Type.GetProperties()
                .Where(t => t.GetCustomAttributes(typeof(SwaggerIgnoreAttribute), true).Any());

            foreach (var excludedProperty in excludedProperties)
            {
                var propertyName = excludedProperty.Name;
                // Swagger usa camelCase por padrão, então:
                var camelCaseName = char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
                schema.Properties.Remove(camelCaseName);
            }
        }
    }
}
