using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarDealershipApp.Web.UI.Models
{
    public class ClientProfileViewModel
    {
        public ClientProfileViewModel()
        {
            Orders = new List<OrderViewModel>();
        }
        public Guid Id { get; set; }
        public virtual ICollection<OrderViewModel> Orders { get; set; }
        public Guid ClientId { get; set; }
        public virtual ClientViewModel Client { get; set; }
    }
}
