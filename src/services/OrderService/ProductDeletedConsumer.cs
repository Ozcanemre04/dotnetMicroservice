using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Events;
using MassTransit;
using OrderService.data;

namespace OrderService
{
    public class ProductDeletedConsumer : IConsumer<ProductDeletedEventDto>
    {
        private readonly ApplicationDbContext _DbContext;
       
        public ProductDeletedConsumer(ApplicationDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ProductDeletedEventDto> context)
        {
            var msg = context.Message;
            await Console.Out.WriteLineAsync("id with "+ msg +" is deleted");
            var product = await _DbContext.ProductCreatedEvents.FindAsync(msg.Id) ?? throw new Exception("not found");
            _DbContext.Remove(product);
            await _DbContext.SaveChangesAsync();
        }
    }
}