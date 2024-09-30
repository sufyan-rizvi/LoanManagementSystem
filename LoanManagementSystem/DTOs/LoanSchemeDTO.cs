using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.DTOs
{
    public class LoanSchemeDTO
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
        public virtual double InterestRate { get; set; }
        public virtual string LongDescription { get; set; }
        public virtual string ShortDescription { get; set; }
        public virtual bool IsActive { get; set; }
    }
}