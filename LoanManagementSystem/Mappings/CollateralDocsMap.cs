using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Mappings
{
    public class CollateralDocsMap:ClassMap<CollateralDocuments>
    {
        public CollateralDocsMap()
        {
            Table("CollateralDocs");
            Id(c => c.CollateralId).GeneratedBy.GuidComb();
            //Map(c => c.DocumentType);
            Map(c => c.DocLink);
            References(r => r.Customer).Column("CustomerId").Cascade.None();
        }
    }
}