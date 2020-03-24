using CarDealershipApp.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealershipApp.Web.UI.Models
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {
            OrderItems = new List<OrderItemViewModel>();
        }
        public Guid Id { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalSellingPrice { get; set; }
        public int OrderNumber { get; set; }
        [Required(ErrorMessage = "The date created is required")]
        public DateTime DateCreated { get; set; }
        public virtual ICollection<OrderItemViewModel> OrderItems { get; set; }
        [Required(ErrorMessage = "The client is required")]
        public Guid ProfileId { get; set; }
        public virtual ClientProfileViewModel Profile { get; set; }
        public string AddedById { get; set; }
        public string CreatedBy { get; set; }
    }
}
