using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Mappings
{
    public class UserMap:ClassMap<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(u=>u.UserId).GeneratedBy.GuidComb();
            Map(u => u.Username);
            Map(u => u.Password);
            Map(u => u.Email);
            Map(u => u.FirstName);
            Map(u => u.LastName);
            Map(u => u.PhoneNumber);
            Map(u => u.IsActive);
            Map(u => u.Age);
            HasOne(u => u.Role).PropertyRef(u=>u.User).Cascade.All();
            HasOne(u=>u.Address).PropertyRef(u=>u.User).Cascade.All();
            HasOne(u => u.Admin).PropertyRef(u=>u.User).Cascade.All();
            HasOne(u => u.LoanOfficer).PropertyRef(u => u.User).Cascade.All();
            HasOne(u => u.Customer).PropertyRef(u => u.User).Cascade.All();
        }
    }
}