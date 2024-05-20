using AutoMapper;
using Events;
using MassTransit;
using MassTransit.RabbitMqTransport.Integration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.data;
using ProductService.dtos;
using ProductService.models;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _DbContext;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndPoint;
        public ProductController(ApplicationDbContext dbContext, IMapper mapper, IPublishEndpoint publishEndPoint)
        {
            _DbContext = dbContext;
            _mapper = mapper;
            _publishEndPoint = publishEndPoint;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct(){
            var products = await _DbContext.Products.ToListAsync();
            var productdto = products.Select(product => _mapper.Map<ProductDtos>(product));
            return Ok(productdto);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id){
            var product = await _DbContext.Products.FindAsync(id);
            if(product==null) return NotFound("Product is not found");
            _DbContext.Remove(product);
            await _DbContext.SaveChangesAsync();
            await _publishEndPoint.Publish(_mapper.Map<ProductDeletedEventDto>(product));
            return Ok("product is deleted");
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDtos addProductDtos){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var product = _mapper.Map<Product>(addProductDtos);
            await _DbContext.Products.AddAsync(product);
            var productDto = _mapper.Map<ProductDtos>(product);
            await _DbContext.SaveChangesAsync();
            await _publishEndPoint.Publish(_mapper.Map<ProductCreatedEventDto>(productDto));
            return Ok(productDto);
        }

    }
}