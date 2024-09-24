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
            Map(u => u.Username).Not.Nullable().Unique();
            Map(u => u.Password).Not.Nullable();
            Map(u => u.Email).Unique().Not.Nullable();
            Map(u => u.FirstName).Not.Nullable();
            Map(u => u.LastName).Not.Nullable();
            Map(u => u.IsActive);
            HasOne(u => u.Role).PropertyRef(u=>u.User).Cascade.All();
            HasMany(u=>u.Address).Inverse().Cascade.All();
            HasMany(u => u.Admins).Inverse().Cascade.All();
            HasMany(u => u.LoanOfficers).Inverse().Cascade.All();
            HasMany(u => u.Customers).Inverse().Cascade.All();
        }
    }
}