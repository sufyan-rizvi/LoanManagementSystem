using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class CollateralDocuments
    {
        public virtual Guid CollateralId { get; set; }
        public virtual string VehicleDoc { get; set; }
        public virtual string HouseDoc { get; set; }
        public virtual string SharesDoc { get; set; }
        public virtual Customer Customer { get; set; }
    }
}