using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Mappings
{
    public class BankAccountDetailMap:ClassMap<BankAccountDetails>
    {
        public BankAccountDetailMap()
        {
            Table("BankDetails");
            Id(b=>b.AccountDetailId).GeneratedBy.GuidComb();
            Map(b => b.NameOnCard);
            Map(b => b.CardNumber);
            Map(b => b.CVV);
            Map(b => b.ExpiryMonth);
            Map(b => b.ExpiryYear);
            References(b=>b.Customer).Column("CustomerId").Cascade.All();
        }
    }
}