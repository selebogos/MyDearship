using CarDealershipApp.DAL.DBContext;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CarDealershipApp.DAL.Entities
{
    [Table("Clients")]
    public class Client : DomainEntity
    {
        public string FullName { get; set; }
        [Required]
        public string AddedById { get; set; }
        [ForeignKey("AddedById")]
        public virtual IdentityUser User { get; set; }
        public string Address { get; set; }
    }
}
