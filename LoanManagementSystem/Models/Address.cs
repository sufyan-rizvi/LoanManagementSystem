using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class Address
    {
        public virtual Guid AddressId { get; set; }
        [Required]
        [Display(Name = "Flat Number")]
        public virtual string FlatNo { get; set; }
        [Required]
        [Display(Name = "Building Name")]
        public virtual string BuildingName { get; set; }
        [Required]
        [Display(Name = "Street Name")]
        public virtual string StreetName { get; set; }
        [Required]
        public virtual string City { get; set; }
        [Required]
        [Display(Name = "Pin code")]
        public virtual string PinCode { get; set; }
        [Required]
        public virtual string State { get; set; }
        [Required]
        public virtual string Country { get; set; }
        public virtual User User { get; set; }

    }
}