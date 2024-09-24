using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Mappings
{
    public class CustomerMap:ClassMap<Customer>
    {
        public CustomerMap()
        {
            Table("Customers");
            Id(c=>c.CustId).GeneratedBy.GuidComb();
            Map(c => c.AadharNumber).Unique();
            Map(c => c.PANNumber).Unique();
            References(c => c.User).Column("UserId").Cascade.None();
            HasMany(c=>c.RegistrationDocuments).Inverse().Cascade.All();//check cascade all.. if loan officer deleted, distribute his work among other officers
            HasMany(c=>c.CollateralDocuments).Inverse().Cascade.All(); //check cascade all.. if loan officer deleted, distribute his work among other officers
        }
    }
}