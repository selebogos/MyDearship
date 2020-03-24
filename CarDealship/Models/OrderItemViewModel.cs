using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealershipApp.Web.UI.Models
{
    public class OrderItemViewModel
    {
        public Guid OrderId { get; set; }
        public virtual OrderViewModel Order { get; set; }
        public Guid ProductId { get; set; }
        public virtual ProductViewModel Product { get; set; }
    }
}
