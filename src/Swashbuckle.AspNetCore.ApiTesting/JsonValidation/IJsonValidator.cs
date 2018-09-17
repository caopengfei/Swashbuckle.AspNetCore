using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;

namespace Swashbuckle.AspNetCore.ApiTesting
{
    public interface IJsonValidator
    {
        bool CanValidate(OpenApiSchema schema);

        bool Validate(OpenApiSchema schema, JToken instance, out IEnumerable<string> errorsMessages);
    }
}