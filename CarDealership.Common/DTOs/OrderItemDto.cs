using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealershipApp.Common.DTOs
{
    public class OrderItemDto
    {
        public Guid OrderId { get; set; }
        public virtual OrderDto Order { get; set; }
        public Guid ProductId { get; set; }
        public virtual ProductDto Product { get; set; }
    }
}
