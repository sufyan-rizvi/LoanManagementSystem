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
        [Display(Name ="Scheme Name")]
        public virtual string SchemeName { get; set; }
        [Display(Name = "Scheme Type")]
        public virtual SchemeType SchemeType { get; set; }
        public virtual string SchemeTypeString
        {
            get
            {
                return Enum.GetName(typeof(SchemeType), (int)SchemeType);
            }
        }
        [Required]
        [Display(Name ="Interest %")]
        public virtual double InterestRate { get; set; }
        [Display(Name = "Long Description")]
        [Column(TypeName = "text")]
        public virtual string LongDescription { get; set; }
        [Display(Name = "Short Description")]
        [Column(TypeName = "text")]
        public virtual string ShortDescription { get; set; }
        public virtual bool IsActive { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual IList<LoanApplication> LoanApplications { get; set; }


    }
}