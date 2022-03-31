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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository repository;
        private readonly IMapper mapper;

        public OrdersController(IOrderRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public IActionResult Get(QueryParameter parameter)
        {
            var pageInfo = repository.GetOrderPageInfo(parameter);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pageInfo));
            return Ok(mapper.Map<ICollection<OrderRead>>(repository.GetOrders(parameter)));
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var order = repository.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<OrderRead>(order));
        }

        // POST api/<OrdersController>
        [HttpPost]
        public IActionResult Post(OrderWrite orderWrite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string? userId = HttpContext.User.Identity?.Name;
            var order = mapper.Map<Order>(orderWrite);
            var result = repository.CreateOrder(order, string.IsNullOrEmpty(userId) ? "System" : userId);
            if (result)
                return CreatedAtRoute(new { id = order.Id }, mapper.Map<OrderRead>(order));
            else
                return BadRequest(new { Message = "The voucher is invalid" });
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, OrderWrite orderWrite)
        {
            var p = repository.GetOrder(id);
            if (p == null)
            {
                return NotFound();
            } else if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string? userId = HttpContext.User.Identity?.Name;
            var order = mapper.Map<Order>(orderWrite);
            order.Id = id;
            var result = repository.UpdateOrder(order, string.IsNullOrEmpty(userId) ? "System" : userId);
            if (result)
                return NoContent();
            else
                return BadRequest(new { Message = "The voucher is invalid" });            
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = repository.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }
            string? userId = HttpContext.User.Identity?.Name;
            repository.DeleteOrder(order, string.IsNullOrEmpty(userId) ? "System" : userId);
            return NoContent();
        }
    }
}