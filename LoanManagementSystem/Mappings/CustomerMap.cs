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
            Map(c => c.AadharNumber);
            Map(c => c.PANNumber);
            Map(c => c.PaymentsMissed);
            References(c => c.User).Column("UserId").Cascade.All();
            HasMany(c=>c.LoanApplications).Inverse().Cascade.All();
            HasMany(c=>c.RegistrationDocuments).Inverse().Cascade.All();//check cascade all.. if loan officer deleted, distribute his work among other officers
            HasMany(c=>c.CollateralDocuments).Inverse().Cascade.All(); //check cascade all.. if loan officer deleted, distribute his work among other officers
            HasMany(c=>c.Repayments).Inverse().Cascade.All(); //check cascade all.. if loan officer deleted, distribute his work among other officers
            HasOne(c=>c.BankAccountDetails).PropertyRef(b=>b.Customer).Cascade.All(); //check cascade all.. if loan officer deleted, distribute his work among other officers
        }
    }
}