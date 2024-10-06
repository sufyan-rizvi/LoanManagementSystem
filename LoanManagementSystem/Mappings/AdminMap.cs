using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Mappings
{
    public class AdminMap:ClassMap<Admin>
    {
        public AdminMap()
        {
            Table("Admins");
            Id(a => a.AdminId).GeneratedBy.GuidComb();
            References(a => a.User).Column("UserId").Cascade.All();
            HasMany(a => a.LoanOfficer).Inverse().Cascade.All();
            HasMany(a => a.Schemes).Inverse().Cascade.All();
            
        }
    }
}