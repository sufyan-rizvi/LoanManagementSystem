using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class Address
    {
        public virtual Guid AddressId { get; set; }
        public virtual AddressType AddressType { get; set; }
        public virtual string FlatNo { get; set; }
        public virtual string BuildingName { get; set; }
        public virtual string StreetName { get; set; }
        public virtual string City { get; set; }
        public virtual string PinCode { get; set; }
        public virtual string State { get; set; }
        public virtual string Country { get; set; }
        public virtual User User { get; set; }
    }
}