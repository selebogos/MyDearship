using CarDealershipApp.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarDealershipApp.DAL.Entities
{
    [Table("Products")]
    public class Product : DomainEntity
    {
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Cost { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal SellingPrice { get; set; }
        public string ImagePath { get; set; }
        public int ProductTypeId { get; set; }
        [ForeignKey("ProductTypeId")]
        public virtual ProductType ProductType { get; set; }
    }
}
