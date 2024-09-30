using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Unity.Resolution;

namespace LoanManagementSystem.Models
{
    public class LoanScheme
    {
        public virtual Guid LoanSchemeId { get; set; }
        public virtual string SchemeName { get; set; }
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
        public virtual string LongDescription { get; set; }
        public virtual string ShortDescription { get; set; }
        public virtual bool IsActive { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public virtual IList<LoanApplication> LoanApplications { get; set; }


    }
}