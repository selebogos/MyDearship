using CarDealershipApp.DAL.DBContext;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealershipApp.DAL.Entities
{
    [Table("Orders")]
    public class Order : DomainEntity
    {
        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
        public int OrderNumber { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalCost { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalSellingPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public Guid ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public virtual ClientProfile Profile { get; set; }
        [Required]
        public string AddedById { get; set; }
        [ForeignKey("AddedById")]
        public virtual IdentityUser User { get; set; }
    }
}
