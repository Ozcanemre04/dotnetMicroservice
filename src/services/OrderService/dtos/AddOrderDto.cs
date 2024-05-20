using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.dtos
{
    public class AddOrderDto
    {
        
        public required Guid ProductId { get; set; }
        public required int Quantity { get; set; }
       
    }
}