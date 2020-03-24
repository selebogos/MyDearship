using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealershipApp.Common.DTOs
{
    public class OrderDataDto
    {
        public Guid OrderId { get; set; }
        public int OrderNumber { get; set; }
        public Guid ClientId { get; set; }
    }
}
