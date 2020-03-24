﻿
using System;
namespace CarDealershipApp.Web.UI.Models
{
    public class AddressViewModel
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string Province { get; set; }
        public int Code { get; set; }
        public Guid CountryId { get; set; }
        public virtual CountryViewModel Country { get; set; }
    }
}
