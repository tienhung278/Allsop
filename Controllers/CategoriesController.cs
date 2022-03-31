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
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository repository;
        private readonly IMapper mapper;

        public CategoriesController(ICategoryRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // GET: api/<CategorysController>
        [HttpGet]
        public IActionResult Get(QueryParameter parameter)
        {
            var pageInfo = repository.GetCategoryPageInfo(parameter);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pageInfo));
            return Ok(mapper.Map<ICollection<CategoryRead>>(repository.GetCategories(parameter)));
        }

        // GET api/<CategorysController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var Category = repository.GetCategory(id);
            if (Category == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CategoryRead>(Category));
        }

        // POST api/<CategorysController>
        [HttpPost]
        public IActionResult Post(CategoryWrite categoryWrite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string? userId = HttpContext.User.Identity?.Name;
            var category = mapper.Map<Category>(categoryWrite);
            repository.CreateCategory(category, string.IsNullOrEmpty(userId) ? "System" : userId);
            return CreatedAtRoute(new { id = category.Id }, mapper.Map<CategoryRead>(category));
        }

        // PUT api/<CategorysController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, CategoryWrite categoryWrite)
        {
            var p = repository.GetCategory(id);
            if (p == null)
            {
                return NotFound();
            } else if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string? userId = HttpContext.User.Identity?.Name;
            var category = mapper.Map<Category>(categoryWrite);
            category.Id = id;
            repository.UpdateCategory(category, string.IsNullOrEmpty(userId) ? "System" : userId);
            return NoContent();
        }

        // DELETE api/<CategorysController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = repository.GetCategory(id);
            if (category == null)
            {
                return NotFound();
            }
            string? userId = HttpContext.User.Identity?.Name;
            repository.DeleteCategory(category, string.IsNullOrEmpty(userId) ? "System" : userId);
            return NoContent();
        }
    }
}