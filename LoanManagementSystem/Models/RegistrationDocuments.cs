using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class RegistrationDocuments
    {
        public virtual Guid RegId { get; set; }
        public string AadharLink { get; set; }
        public string PANLink { get; set; }
        public string PhotoLink { get; set; }
        public virtual Customer Customer { get; set; }


    }
}