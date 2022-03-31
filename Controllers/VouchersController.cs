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
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherRepository repository;
        private readonly IMapper mapper;

        public VouchersController(IVoucherRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        // GET: api/<VouchersController>
        [HttpGet]
        public IActionResult Get(QueryParameter parameter)
        {
            var pageInfo = repository.GetVoucherPageInfo(parameter);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pageInfo));
            return Ok(mapper.Map<ICollection<VoucherRead>>(repository.GetVouchers(parameter)));
        }

        // GET api/<VouchersController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var voucher = repository.GetVoucher(id);
            if (voucher == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<VoucherRead>(voucher));
        }

        // POST api/<VouchersController>
        [HttpPost]
        public IActionResult Post(VoucherWrite voucherWrite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string? userId = HttpContext.User.Identity?.Name;
            var voucher = mapper.Map<Voucher>(voucherWrite);
            repository.CreateVoucher(voucher, string.IsNullOrEmpty(userId) ? "System" : userId);
            return CreatedAtRoute(new { id = voucher.Id }, mapper.Map<VoucherRead>(voucher));
        }

        // PUT api/<VouchersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, VoucherWrite voucherWrite)
        {
            var p = repository.GetVoucher(id);
            if (p == null)
            {
                return NotFound();
            } else if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string? userId = HttpContext.User.Identity?.Name;
            var voucher = mapper.Map<Voucher>(voucherWrite);
            voucher.Id = id;
            repository.UpdateVoucher(voucher, string.IsNullOrEmpty(userId) ? "System" : userId);
            return NoContent();
        }

        // DELETE api/<VouchersController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var voucher = repository.GetVoucher(id);
            if (voucher == null)
            {
                return NotFound();
            }
            string? userId = HttpContext.User.Identity?.Name;
            repository.DeleteVoucher(voucher, string.IsNullOrEmpty(userId) ? "System" : userId);
            return NoContent();
        }
    }
}