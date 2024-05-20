using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.dtos
{
    public class AddProductDtos
    {
        public required string Name { get; set; }
        public required string Category { get; set; }
        public required decimal Price { get; set; }
    }
}