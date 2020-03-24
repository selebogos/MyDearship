﻿using System.ComponentModel.DataAnnotations;
namespace CarDealershipApp.Web.Models
{
    public class NormalRangeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The normal range description is required")]
        public string Description { get; set; }
    }
}
