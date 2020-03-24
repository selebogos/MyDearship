using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarDealershipApp.Web.UI.Models
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "The client's name is required")]
        public string FullName { get; set; }
        public string AddedById { get; set; }
        public string CreatedBy { get; set; }
        public string Address { get; set; }
    }
}
