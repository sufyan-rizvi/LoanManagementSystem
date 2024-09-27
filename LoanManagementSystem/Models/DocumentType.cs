using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    //public enum DocumentType
    //{
    //    Aadhar,PAN, Photo, VehicleDoc, HouseDoc, SharesDoc
    //}
    public class DocumentType
    {
        public virtual int DocumentTypeId { get; set; }
        public virtual string Name { get; set; }
    }
}