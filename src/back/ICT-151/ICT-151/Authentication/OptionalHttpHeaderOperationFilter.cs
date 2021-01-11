using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ICT_151.Authentication
{
    //https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1425#issuecomment-571926368 <- love this guy
    public class OptionalHttpHeaderOperationFilter : IOperationFilter
    {
        private readonly string headerName;
        private readonly string headerDescription;
        private readonly string headerType;

        public OptionalHttpHeaderOperationFilter(string headerName_, string headerDescription_, string headerType_ = "String")
        {
            headerName = headerName_;
            headerDescription = headerDescription_;
            headerType = headerType_;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Security == null)
                operation.Security = new List<OpenApiSecurityRequirement>();


            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = SessionTokenAuthOptions.DefaultSchemeName
                }
            };
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
        }
    }
}
