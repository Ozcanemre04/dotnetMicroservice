using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.models
{
    public class ProductCreatedEvent
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public required decimal Price { get; set; }
    }
}