using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DatingApp.Filter
{
 
        public class AddRequiredHeaderParameter : IOperationFilter
        {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Security == null)
                operation.Security = new List<OpenApiSecurityRequirement>();

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "SomeheaderKey",
                In = ParameterLocation.Header,
                Required = false
            });

            var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" } };
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>(),

            });



        }

    }
    
}
