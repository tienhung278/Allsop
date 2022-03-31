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
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository repository;
        private readonly IMapper mapper;

        public BrandsController(IBrandRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // GET: api/<BrandsController>
        [HttpGet]
        public IActionResult Get(QueryParameter parameter)
        {
            var pageInfo = repository.GetBrandPageInfo(parameter);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pageInfo));
            return Ok(mapper.Map<ICollection<BrandRead>>(repository.GetBrands(parameter)));
        }

        // GET api/<BrandsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var brand = repository.GetBrand(id);
            if (brand == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<BrandRead>(brand));
        }

        // POST api/<BrandsController>
        [HttpPost]
        public IActionResult Post(BrandWrite brandWrite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string? userId = HttpContext.User.Identity?.Name;
            var brand = mapper.Map<Brand>(brandWrite);
            repository.CreateBrand(brand, string.IsNullOrEmpty(userId) ? "System" : userId);
            return CreatedAtRoute(new { id = brand.Id }, mapper.Map<BrandRead>(brand));
        }

        // PUT api/<BrandsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, BrandWrite brandWrite)
        {
            var p = repository.GetBrand(id);
            if (p == null)
            {
                return NotFound();
            } else if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string? userId = HttpContext.User.Identity?.Name;
            var brand = mapper.Map<Brand>(brandWrite);
            brand.Id = id;
            repository.UpdateBrand(brand, string.IsNullOrEmpty(userId) ? "System" : userId);
            return NoContent();
        }

        // DELETE api/<BrandsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var brand = repository.GetBrand(id);
            if (brand == null)
            {
                return NotFound();
            }
            string? userId = HttpContext.User.Identity?.Name;
            repository.DeleteBrand(brand, string.IsNullOrEmpty(userId) ? "System" : userId);
            return NoContent();
        }
    }
}