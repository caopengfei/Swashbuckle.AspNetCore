using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;

namespace TestFirst.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        [HttpPost]
        public IActionResult CreateProduct([FromBody]Product product)
        {
            if (!this.ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            //return new StatusCodeResult(201);
            return Created("api/products/1", null);
        }

        [HttpGet]
        public IActionResult GetProducts([FromQuery]PagingParameters pagingParameters)
        {
            if (!this.ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            return new ObjectResult(new[] { new Product { Id = 1, Name = "Test product" } });
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            if (id != 1)
                return NotFound();

            return new ObjectResult(new Product { Id = 1, Name = "Test product" });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody]Product product)
        {
            if (id != 1)
                return NotFound();

            if (!this.ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            return Ok();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchProduct(int id, [FromBody]JsonPatchDocument<Product> patchDocument)
        {
            if (id != 1)
                return NotFound();

            if (!this.ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            return Ok();
        }
    }

    public class Product
    {
        public int Id { get; internal set;}

        [Required]
        public string Name { get; set; }
    }

    public class PagingParameters
    {
        [BindRequired]
        public int PageNo { get; set; }

        [BindRequired]
        public int PageSize { get; set; }
    }
}
