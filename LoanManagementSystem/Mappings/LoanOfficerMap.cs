using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Mappings
{
    public class LoanOfficerMap:ClassMap<LoanOfficer>
    {
        public LoanOfficerMap()
        {
            Table("LoanOfficers");
            Id(l => l.OfficerId).GeneratedBy.GuidComb();
            References(l => l.User).Column("UserId").Cascade.All();
            HasMany(l=>l.LoanApplications).Inverse().Cascade.All();
        }
    }
}