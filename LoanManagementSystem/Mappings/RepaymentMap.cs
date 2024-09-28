using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Mappings
{
    public class RepaymentMap:ClassMap<Repayment>
    {
        public RepaymentMap()
        {
            Table("Repayments");
            Id(r => r.RepaymentId).GeneratedBy.GuidComb();
            Map(r => r.StartDate);
            Map(r => r.EndDate);
            Map(r => r.NextPaymentDate);
            Map(r => r.Amount);
            //References(r => r.Customer).Column("CustomerId").Cascade.All();
            References(r => r.Application).Column("LoanApplicationId").Cascade.All();
            
        }
    }
}