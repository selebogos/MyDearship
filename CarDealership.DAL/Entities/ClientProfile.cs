using CarDealershipApp.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarDealershipApp.DAL.Entities
{
    [Table("ClientProfiles")]
    public class ClientProfile : DomainEntity
    {
        public ClientProfile()
        {
            Orders = new List<Order>();
        }
        public virtual ICollection<Order> Orders { get; set; }
        public Guid ClientId { get; set; }
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
