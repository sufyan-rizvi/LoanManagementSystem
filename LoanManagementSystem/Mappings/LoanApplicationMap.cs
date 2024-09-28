using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Mappings
{
    public class LoanApplicationMap:ClassMap<LoanApplication>
    {
        public LoanApplicationMap()
        {
            Table("Applications");
            Id(a => a.ApplicationId).GeneratedBy.GuidComb();
            Map(a => a.ApplicationDate);
            Map(a => a.PaymentStartDate);
            Map(a => a.LoanAmount);
            Map(a => a.Tenure);
            Map(a => a.Status);
            References(a => a.Applicant).Column("CustomerId").Cascade.All();
            References(a => a.AssignedOfficer).Column("LoanOfficerId").Cascade.All();
            References(a => a.Scheme).Column("LoanSchemeId").Cascade.All();
            HasMany(l=>l.Repayments).Inverse().Cascade.All();
            

        }
    }
}