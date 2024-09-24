using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class Customer
    {
        public virtual Guid CustId { get; set; }
        public virtual string AadharNumber { get; set; }
        public virtual string PANNumber { get; set; }
        public virtual User User { get; set; }
        public virtual IList<RegistrationDocuments> RegistrationDocuments { get; set; }
        public virtual IList<CollateralDocuments> CollateralDocuments { get; set; }

    }
}