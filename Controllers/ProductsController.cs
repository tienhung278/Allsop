using AutoMapper;
using Allsop.Contracts;
using Allsop.Models.DTOs;
using Allsop.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Allsop.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Allsop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository repository;
        private readonly IMapper mapper;

        public ProductsController(IProductRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public IActionResult Get(QueryParameter parameter)
        {
            var pageInfo = repository.GetProductPageInfo(parameter);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pageInfo));            
            return Ok(mapper.Map<ICollection<ProductRead>>(repository.GetProducts(parameter)));
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = repository.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<ProductRead>(product));
        }

        // POST api/<ProductsController>
        [HttpPost]
        public IActionResult Post(ProductWrite productWrite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string? userId = HttpContext.User.Identity?.Name;
            var product = mapper.Map<Product>(productWrite);
            repository.CreateProduct(product, string.IsNullOrEmpty(userId) ? "System" : userId);
            product = repository.GetProduct(product.Id);
            return CreatedAtRoute(new { id = product?.Id }, mapper.Map<ProductRead>(product));
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, ProductWrite productWrite)
        {
            var p = repository.GetProduct(id);
            if (p == null)
            {
                return NotFound();
            } else if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string? userId = HttpContext.User.Identity?.Name;
            var product = mapper.Map<Product>(productWrite);
            product.Id = id;
            repository.UpdateProduct(product, string.IsNullOrEmpty(userId) ? "System" : userId);
            return NoContent();
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var productRead = repository.GetProduct(id);
            if (productRead == null)
            {
                return NotFound();
            }
            string? userId = HttpContext.User.Identity?.Name;
            repository.DeleteProduct(id, string.IsNullOrEmpty(userId) ? "System" : userId);
            return NoContent();
        }
    }
}