using System;
using System.Collections.Generic;
namespace CarDealershipApp.Common.DTOs
{
    public class OrderDto
    {
        public OrderDto()
        {
            OrderItems = new List<OrderItemDto>();
        }
        public Guid Id { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalSellingPrice { get; set; }
        public int OrderNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public string AddedById { get; set; }
        public string CreatedBy { get; set; }
        public virtual ICollection<OrderItemDto> OrderItems { get; set; }
        public Guid ProfileId { get; set; }
        public virtual ClientProfileDto Profile { get; set; }
    }
}
