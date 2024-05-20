using AutoMapper;
using Events;
using OrderService.dtos;
using OrderService.models;


namespace OrderService
{
    public class AutoMapperProfiles : Profile
    {
         public AutoMapperProfiles(){
            CreateMap<Order,OrderDtos>();
            CreateMap<AddOrderDto,Order>();
            CreateMap<ProductCreatedEventDto,ProductCreatedEvent>();
            
        }
    }
}