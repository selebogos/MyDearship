using System;
namespace CarDealershipApp.Common.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public decimal SellingPrice { get; set; }
        public string ImagePath { get; set; }
        public int ProductTypeId { get; set; }
        public virtual ProductTypeDto ProductType { get; set; }
        public Guid OrderId { get; set; }
        public virtual OrderDto Order { get; set; }
    }
}
