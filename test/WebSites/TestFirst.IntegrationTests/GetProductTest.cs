using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.OpenApi.Models;
using Xunit;
using Xunit.Abstractions;
using Swashbuckle.AspNetCore.ApiTesting.Xunit;

namespace TestFirst.IntegrationTests
{
    public class GetProductTest : ApiOperationFixture<TestFirst.Startup>
    {
        public GetProductTest(
            ApiTestsHelper apiTestsHelper,
            WebApplicationFactory<TestFirst.Startup> webAppFactory)
            : base(apiTestsHelper, webAppFactory)
        {
            Describe("v1", "/api/products/{id}", OperationType.Get, new OpenApiOperation
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
                Responses = new OpenApiResponses
                {
                    [ "200" ] = new OpenApiResponse 
                    {
                        Description = "Retrieved product",
                        Content = new Dictionary<string, OpenApiMediaType >
                        {
                            [ "application/json" ] = new OpenApiMediaType
                            {
                                Schema = new OpenApiSchema
                                {
                                    Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "product" }
                                }
                            }
                        }
                    },
                    [ "404" ] = new OpenApiResponse 
                    {
                        Description = "Product not found"
                    }
                }
            });
        }

        [Fact]
        public async Task Returns200AndProduct()
        {
            await TestAsync("200",
                requestParameters: new Dictionary<string, object>
                {
                    [ "id" ] = 1
                });
        }

        [Fact]
        public async Task Returns404_GivenUnknownId()
        {
            await TestAsync("404",
                requestParameters: new Dictionary<string, object>
                {
                    [ "id" ] = 2
                });
        }
    }
}