using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Mappings
{
    public class LoanSchemeMap : ClassMap<LoanScheme>
    {
        public LoanSchemeMap()
        {
            Table("LoanSchemes");
            Id(s => s.LoanSchemeId).GeneratedBy.GuidComb();
            Map(s => s.SchemeType);
            Map(s => s.InterestRate);
            References(s=>s.Admin).Column("AdminId").Cascade.None();
        }
    }
}