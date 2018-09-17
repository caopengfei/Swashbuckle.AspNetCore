using System.Linq;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;

namespace Swashbuckle.AspNetCore.ApiTesting
{
    public class JsonAnyOfValidator : IJsonValidator
    {
        private JsonValidator _jsonValidator;

        public JsonAnyOfValidator(JsonValidator jsonValidator)
        {
            _jsonValidator = jsonValidator;
        }

        public bool CanValidate(OpenApiSchema schema) => schema.AnyOf != null;

        public bool Validate(OpenApiSchema schema, JToken instance, out IEnumerable<string> errorMessages)
        {
            var errorMessagesList = new List<string>();

            var allOfArray = schema.AnyOf.ToArray();

            for (int i=0;i<allOfArray.Length;i++)
            {
                if (_jsonValidator.Validate(allOfArray[i], instance, out IEnumerable<string> subErrorMessages))
                {
                    errorMessages = Enumerable.Empty<string>();
                    return true;
                }

                errorMessagesList.AddRange(subErrorMessages.Select(msg => $"{msg} (anyOf[{i}])"));
            }

            errorMessages = errorMessagesList;
            return !errorMessages.Any();
        }
    }
}