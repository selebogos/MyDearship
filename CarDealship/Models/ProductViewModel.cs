using CarDealershipApp.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealershipApp.Web.UI.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "The test name is required")]
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public decimal SellingPrice { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "The product type is required")]
        public int ProductTypeId { get; set; }
        public virtual ProductTypeViewModel ProductType { get; set; }
        [Required(ErrorMessage = "The order is required")]
        public Guid OrderId { get; set; }
        public virtual OrderViewModel Order { get; set; }
    }
}
