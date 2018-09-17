using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.ApiTesting.Xunit;

namespace TestFirst.IntegrationTests
{
    public class PatchProductTest : ApiOperationFixture<TestFirst.Startup>
    {
        public PatchProductTest(
            ApiTestsHelper apiTestsHelper,
            WebApplicationFactory<TestFirst.Startup> webAppFactory)
            : base(apiTestsHelper, webAppFactory)
        {
            Describe("v1", "/api/products/{id}", OperationType.Patch, new OpenApiOperation
            {
                Parameters = new List<OpenApiParameter>
                {
                    new OpenApiParameter
                    {
                        Name = "id",
                        In = ParameterLocation.Path,
                        Schema = new OpenApiSchema { Type = "number" }
                    }
                },
                RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType> 
                    {
                        [ "application/json" ] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "patchDocument" }
                            }
                        }
                    }
                },
                Responses = new OpenApiResponses
                {
                    [ "200" ] = new OpenApiResponse 
                    {
                        Description = "Product patched"
                    },
                    [ "400" ] = new OpenApiResponse 
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.Response, Id = "invalidRequest" }
                    },
                    [ "404" ] = new OpenApiResponse 
                    {
                        Description = "Product not found"
                    }
                }
            });
        }

        [Fact]
        public async Task Returns200_GivenAValidRequestBody()
        {
            await TestAsync("200",
                requestParameters: new Dictionary<string, object>
                {
                    [ "id" ] = 1
                },
                requestBody: new[] {
                    new { op = "replace", path = "/name", value = "foobar" }
                }
            );
        }

        [Fact]
        public async Task Returns400AndErrorMap_GivenAnInvalidRequestBody()
        {
            await TestAsync("400",
                requestParameters: new Dictionary<string, object>
                {
                    [ "id" ] = 1
                },
                requestBody: new
                { }
            );
        }

        [Fact]
        public async Task Returns404_GivenUnknownId()
        {
            await TestAsync("404",
                requestParameters: new Dictionary<string, object>
                {
                    [ "id" ] = 2
                },
                requestBody: new
                { }
            );
        }
    }
}