using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Events;
using ProductService.dtos;
using ProductService.models;

namespace ProductService
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile(){
            CreateMap<Product,ProductDtos>();
            CreateMap<ProductDtos,ProductCreatedEventDto>();
            CreateMap<Product,ProductDeletedEventDto>();
            CreateMap<AddProductDtos,Product>();
        }
        
    }
}