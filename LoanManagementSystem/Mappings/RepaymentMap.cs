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
            Map(r => r.PaymentDate);
            Map(r => r.Amount);
            Map(r => r.TransactionId);
            Map(r => r.IsApproved);
            References(r => r.Application).Column("LoanApplicationId").Cascade.All();
            
        }
    }
}