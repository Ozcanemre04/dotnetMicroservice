using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Events
{
    public class ProductDeletedEventDto
    {
        public required Guid Id { get; set; }
        
    }
}