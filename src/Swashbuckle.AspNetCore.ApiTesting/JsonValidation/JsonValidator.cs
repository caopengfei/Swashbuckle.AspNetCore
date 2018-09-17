using System.Linq;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;

namespace Swashbuckle.AspNetCore.ApiTesting
{
    public class JsonValidator : IJsonValidator
    {
        private readonly OpenApiDocument _openApiDocument;
        private readonly IEnumerable<IJsonValidator> _subValidators;

        public JsonValidator(OpenApiDocument openApiDocument)
        {
            _openApiDocument = openApiDocument;

            _subValidators = new IJsonValidator[]
            {
                new JsonNullValidator(),
                new JsonBooleanValidator(),
                new JsonObjectValidator(this),
                new JsonArrayValidator(this),
                new JsonNumberValidator(),
                new JsonStringValidator(),
                new JsonAllOfValidator(this),
                new JsonAnyOfValidator(this),
                new JsonOneOfValidator(this),
            };
        }

        public bool CanValidate(OpenApiSchema schema) => true;

        public bool Validate(OpenApiSchema schema, JToken jToken, out IEnumerable<string> errorMessages)
        {
            schema = (schema.Reference != null && schema.Reference.Type == ReferenceType.Schema)
                ? (OpenApiSchema)_openApiDocument.ResolveReference(schema.Reference)
                : schema;

            var errorMessagesList = new List<string>();

            foreach (var subValidator in _subValidators)
            {
                if (!subValidator.CanValidate(schema)) continue;

                subValidator.Validate(schema, jToken, out IEnumerable<string> subErrorMessages);
                errorMessagesList.AddRange(subErrorMessages);
            }

            errorMessages = errorMessagesList;
            return !errorMessages.Any();
        }
    }
}