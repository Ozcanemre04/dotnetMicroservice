using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.models
{
    public class Order
    {
        [Key]
        public required Guid Id { get; set; }
        public required Guid ProductId { get; set; }
        public required int Quantity { get; set; }
       
        public required decimal TotalPrice { get; set; }
    }
}