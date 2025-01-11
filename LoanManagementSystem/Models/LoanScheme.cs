using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Unity.Resolution;

namespace LoanManagementSystem.Models
{
    public class LoanScheme
    {
        public virtual Guid LoanSchemeId { get; set; }

        [Required(ErrorMessage = "Scheme Name is required.")]
        [StringLength(100, ErrorMessage = "Scheme Name cannot exceed 100 characters.")]
        [Display(Name = "Scheme Name")]
        public virtual string SchemeName { get; set; }

        [Required(ErrorMessage = "Scheme Type is required.")]
        [Display(Name = "Scheme Type")]
        public virtual SchemeType SchemeType { get; set; }

        public virtual string SchemeTypeString
        {
            get
            {
                return Enum.GetName(typeof(SchemeType), (int)SchemeType);
            }
        }

        [Required(ErrorMessage = "Interest Rate is required.")]
        [Range(0, 100, ErrorMessage = "Interest Rate must be between 0 and 100.")]
        [Display(Name = "Interest %")]
        public virtual double InterestRate { get; set; }

        [Display(Name = "Long Description")]
        [Required(ErrorMessage = "A Long description is required.")]
        [Column(TypeName = "text")]
        public virtual string LongDescription { get; set; }

        [Display(Name = "Short Description")]
        [Required(ErrorMessage = "A Short description is required.")]
        [Column(TypeName = "text")]
        public virtual string ShortDescription { get; set; }

        public virtual bool IsActive { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual IList<LoanApplication> LoanApplications { get; set; } = new List<LoanApplication>();
    }
}