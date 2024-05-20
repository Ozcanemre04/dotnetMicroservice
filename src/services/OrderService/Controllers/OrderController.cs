using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.data;
using OrderService.dtos;
using OrderService.models;


namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationDbContext _DbContext;
        private readonly IMapper _mapper;
        
        public OrderController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _DbContext = dbContext;
            _mapper = mapper;
                 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrder(){
            var orders = await _DbContext.Orders.ToListAsync();
            var orderdto = orders.Select(order => _mapper.Map<OrderDtos>(order));
            return Ok(orderdto);
        }
        [HttpGet("product")]
        public async Task<IActionResult> GetAllProductsevent(){
            var products = await _DbContext.ProductCreatedEvents.ToListAsync();
            return Ok(products);
        }

           [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteOrder([FromRoute] Guid id){
            var order = await _DbContext.Orders.FindAsync(id);
            if(order==null) return NotFound("order is not found");
            _DbContext.Remove(order);
            await _DbContext.SaveChangesAsync();
            return Ok("order is deleted");
        }
       

          [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderDto addOrderDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var product  = await _DbContext.ProductCreatedEvents.FirstOrDefaultAsync(x=>x.Id == addOrderDto.ProductId);
            if(product == null){
                return NotFound("product isn't found");
            }
            var order = _mapper.Map<Order>(addOrderDto);
            order.TotalPrice = product.Price * addOrderDto.Quantity;
            await _DbContext.Orders.AddAsync(order);
            var orderDto = _mapper.Map<OrderDtos>(order);
            await _DbContext.SaveChangesAsync();
            return Ok(orderDto);
        }
    }
}