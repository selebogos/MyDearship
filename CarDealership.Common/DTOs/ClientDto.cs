using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealershipApp.Common.DTOs
{
    public class ClientDto
    {
        public string FullName { get; set; }
        public string AddedById { get; set; }
        public string Address { get; set; }
        public string CreatedBy { get; set; }
        public Guid Id { get; set; }
    }
}
