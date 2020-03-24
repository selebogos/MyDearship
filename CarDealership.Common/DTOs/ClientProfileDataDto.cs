using System;
using System.Collections.Generic;
using System.Text;

namespace CarDealershipApp.Common.DTOs
{
    public class ClientProfileDataDto
    {
        public Guid profileId { get; set; }
        public string FullName { get; set; }
        public Guid ClientId { get; set; }
    }
}
