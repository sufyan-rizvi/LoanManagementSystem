using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Mappings
{
    public class RegistrationDocumentsMap:ClassMap<RegistrationDocuments>
    {
        public RegistrationDocumentsMap()
        {
            Table("RegistrationDocs");
            Id(r=>r.RegId).GeneratedBy.GuidComb();
            Map(r => r.DocumentType);
            Map(r=>r.ImageUrl);
            Map(r=>r.PublicId);
            References(r=>r.Customer).Column("CustomerId").Cascade.None();
        }
    }
}