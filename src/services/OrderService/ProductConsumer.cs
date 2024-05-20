
using AutoMapper;
using Events;
using MassTransit;
using OrderService.data;
using OrderService.models;


namespace OrderService
{
    public class ProductConsumer : IConsumer<ProductCreatedEventDto>
    {
        private readonly ApplicationDbContext _DbContext;
        private readonly IMapper _mapper;
       
        public ProductConsumer(ApplicationDbContext dbContext, IMapper mapper)
        {
            _DbContext = dbContext;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<ProductCreatedEventDto> context)
        {
            var msg = context.Message;
            await Console.Out.WriteLineAsync(msg.Name);
            var product = _mapper.Map<ProductCreatedEvent>(msg);
            await _DbContext.ProductCreatedEvents.AddAsync(product);
            await _DbContext.SaveChangesAsync();
           
        }
    }
}