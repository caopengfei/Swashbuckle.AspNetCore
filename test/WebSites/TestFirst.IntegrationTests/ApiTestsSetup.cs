using System.Collections.Generic;
using System.IO;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.ApiTesting;
using Xunit;

namespace TestFirst.IntegrationTests
{
    [CollectionDefinition("ApiTests")]
    public class ApiTestsCollection : ICollectionFixture<ApiTestsHelper>
    {}

    public class ApiTestsHelper : ApiTestsHelperBase
    {
        public ApiTestsHelper()
        {
            AddDocument("v1", new OpenApiDocument
            {
                Info = new OpenApiInfo
                {
                    Version = "V1",
                    Title = "API V1"
                },
                Paths = new OpenApiPaths(),
                Components = new OpenApiComponents
                {
                    Schemas = new Dictionary<string, OpenApiSchema>
                    {
                        [ "product" ] = new OpenApiSchema 
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                [ "id" ] = new OpenApiSchema { Type = "number", ReadOnly = true },
                                [ "name" ] = new OpenApiSchema { Type = "string" },
                            },
                            Required = new SortedSet<string> { "id", "name" }
                        },
                        [ "errorMap" ] = new OpenApiSchema 
                        {
                            Type = "object",
                            AdditionalProperties = new OpenApiSchema
                            {
                                Type = "array",
                                Items = new OpenApiSchema { Type = "string" }
                            }
                        },
                        [ "patchDocument" ] = new OpenApiSchema
                        {
                            Type = "array",
                            Items = new OpenApiSchema
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "patchOperation" }
                            }
                        },
                        [ "patchOperation" ] = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                [ "op" ] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Enum = new List<IOpenApiAny>
                                    {
                                        new OpenApiString("add"),
                                        new OpenApiString("remove"),
                                        new OpenApiString("replace"),
                                        new OpenApiString("move"),
                                        new OpenApiString("copy"),
                                        new OpenApiString("test")
                                    }
                                },
                                [ "path" ] = new OpenApiSchema
                                {
                                    Type = "string"
                                },
                                [ "value" ] = new OpenApiSchema()
                            }
                        }

                    },
                    Parameters = new Dictionary<string, OpenApiParameter>
                    {
                        [ "pageNo" ] = new OpenApiParameter
                        {
                            Name = "pageNo",
                            In = ParameterLocation.Query,
                            Schema = new OpenApiSchema { Type = "number" },
                            Required = true
                        },
                        [ "pageSize" ] = new OpenApiParameter
                        {
                            Name = "pageSize",
                            In = ParameterLocation.Query,
                            Schema = new OpenApiSchema { Type = "number" },
                            Required = true
                        }
                    },
                    Responses = new Dictionary<string, OpenApiResponse>
                    {
                        [ "invalidRequest" ] = new OpenApiResponse
                        {
                            Description = "Invalid request",
                            Content = new Dictionary<string, OpenApiMediaType>
                            {
                                [ "application/json" ] = new OpenApiMediaType
                                {
                                    Schema = new OpenApiSchema
                                    {
                                        Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "errorMap" }
                                    }
                                }
                            }
                        }
                    }
                } 
            });

            OutputRoot(Path.Combine(System.AppContext.BaseDirectory, "..", "..", "..", "..", "TestFirst", "wwwroot", "api-docs"));
        }
    }
}