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

        [Required(ErrorMessage = "Flat Number is required.")]
        [StringLength(10, ErrorMessage = "Flat Number cannot exceed 10 characters.")]
        [Display(Name = "Flat Number")]
        public virtual string FlatNo { get; set; }

        [Required(ErrorMessage = "Building Name is required.")]
        [StringLength(100, ErrorMessage = "Building Name cannot exceed 100 characters.")]
        [Display(Name = "Building Name")]
        public virtual string BuildingName { get; set; }

        [Required(ErrorMessage = "Street Name is required.")]
        [StringLength(100, ErrorMessage = "Street Name cannot exceed 100 characters.")]
        [Display(Name = "Street Name")]
        public virtual string StreetName { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public virtual string City { get; set; }

        [Required(ErrorMessage = "Pin code is required.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Pin Code must be exactly 6 digits.")]
        [Display(Name = "Pin code")]
        public virtual string PinCode { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters.")]
        public virtual string State { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        public virtual string Country { get; set; }

        public virtual User User { get; set; }

    }
}