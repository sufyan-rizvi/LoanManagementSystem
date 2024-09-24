using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class CollateralDocuments
    {
        public virtual Guid CollateralId { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public virtual string DocLink { get; set; }
        public virtual Customer Customer { get; set; }
    }
}