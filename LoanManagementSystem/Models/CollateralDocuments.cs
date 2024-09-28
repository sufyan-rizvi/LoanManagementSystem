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
        public virtual string ImageUrl { get; set; }  //Used to show uploaded image
        public virtual string PublicId { get; set; }   //used to show full image on click on link from cloudinary
        public virtual Customer Customer { get; set; }
    }
}