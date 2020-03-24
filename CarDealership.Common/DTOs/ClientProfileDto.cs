using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealershipApp.Common.DTOs
{
    public class ClientProfileDto
    {
        public ClientProfileDto()
        {
            Orders = new List<OrderDto>();
        }
        public Guid Id { get; set; }
        public virtual ICollection<OrderDto> Orders { get; set; }
        public Guid ClientId { get; set; }
        public virtual ClientDto Client { get; set; }
    }
}
