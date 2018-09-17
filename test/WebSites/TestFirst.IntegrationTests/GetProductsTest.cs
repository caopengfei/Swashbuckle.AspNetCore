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
    public class GetProductsTest : ApiOperationFixture<TestFirst.Startup>
    {
        public GetProductsTest(
            ApiTestsHelper apiTestsHelper,
            WebApplicationFactory<TestFirst.Startup> webAppFactory)
            : base(apiTestsHelper, webAppFactory)
        {
            Describe("v1", "/api/products", OperationType.Get, new OpenApiOperation
            {
                Parameters = new List<OpenApiParameter>
                {
                    new OpenApiParameter { Reference = new OpenApiReference { Type = ReferenceType.Parameter, Id = "pageNo"} },
                    new OpenApiParameter { Reference = new OpenApiReference { Type = ReferenceType.Parameter, Id = "pageSize"} }
                },
                Responses = new OpenApiResponses
                {
                    [ "200" ] = new OpenApiResponse 
                    {
                        Description = "Retrieved products",
                        Content = new Dictionary<string, OpenApiMediaType >
                        {
                            [ "application/json" ] = new OpenApiMediaType
                            {
                                Schema = new OpenApiSchema
                                {
                                    Type = "array",
                                    Items = new OpenApiSchema 
                                    {
                                        Reference = new OpenApiReference { Type = ReferenceType.Schema, Id = "product" }
                                    }
                                }
                            }
                        }
                    },
                    [ "400" ] = new OpenApiResponse 
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.Response, Id = "invalidRequest" }
                    }
                }
            });
        }

        [Fact]
        public async Task Returns200AndArrayOfProducts_GivenValidRequestParameters()
        {
            await TestAsync("200",
                requestParameters: new Dictionary<string, object>
                {
                    [ "pageNo" ] = 1,
                    [ "pageSize" ] = 10
                });
        }

        [Fact]
        public async Task Returns400AndErrorMap_GivenInvalidRequestParameters()
        {
            await TestAsync("400");
        }
    }
}