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
            Map(r => r.DocumentType);
            Map(r => r.ImageUrl);
            Map(r => r.PublicId);
            References(r => r.Application).Column("ApplicationId").Cascade.None();
        }
    }
}