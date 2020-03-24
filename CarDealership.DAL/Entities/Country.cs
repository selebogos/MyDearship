using CarDealershipApp.DAL.DBContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarDealershipApp.DAL.Entities
{
    [Table("Countries")]
    public class Country : DomainEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
