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
    public class OrderDetailsController : ControllerBase
    {
        private readonly IOrderDetailRepository repository;
        private readonly IMapper mapper;

        public OrderDetailsController(IOrderDetailRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // GET: api/<OrderDetailsController>
        [HttpGet]
        public IActionResult Get(QueryParameter parameter)
        {
            var pageInfo = repository.GetOrderDetailPageInfo(parameter);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pageInfo));
            return Ok(mapper.Map<ICollection<OrderDetailRead>>(repository.GetOrderDetails(parameter)));
        }

        // GET api/<OrderDetailsController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var orderDetail = repository.GetOrderDetail(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<OrderDetailRead>(orderDetail));
        }

        // POST api/<OrderDetailsController>
        [HttpPost]
        public IActionResult Post(OrderDetailWrite orderDetailWrite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string? userId = HttpContext.User.Identity?.Name;
            var orderDetail = mapper.Map<OrderDetail>(orderDetailWrite);
            repository.CreateOrderDetail(orderDetail, string.IsNullOrEmpty(userId) ? "System" : userId);
            return CreatedAtRoute(new { id = orderDetail.Id }, mapper.Map<OrderDetailRead>(orderDetail));
        }

        // PUT api/<OrderDetailsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, OrderDetailWrite orderDetailWrite)
        {
            var p = repository.GetOrderDetail(id);
            if (p == null)
            {
                return NotFound();
            } else if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string? userId = HttpContext.User.Identity?.Name;
            var orderDetail = mapper.Map<OrderDetail>(orderDetailWrite);
            orderDetail.Id = id;
            repository.UpdateOrderDetail(orderDetail, string.IsNullOrEmpty(userId) ? "System" : userId);
            return NoContent();
        }

        // DELETE api/<OrderDetailsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var orderDetail = repository.GetOrderDetail(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            string? userId = HttpContext.User.Identity?.Name;
            repository.DeleteOrderDetail(orderDetail, string.IsNullOrEmpty(userId) ? "System" : userId);
            return NoContent();
        }
    }
}