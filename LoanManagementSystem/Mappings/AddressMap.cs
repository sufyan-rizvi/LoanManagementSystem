using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Mappings
{
    public class AddressMap:ClassMap<Address>
    {
        public AddressMap()
        {
            Table("Address");
            Id(a => a.AddressId).GeneratedBy.GuidComb();
            Map(a => a.AddressType);
            Map(a => a.FlatNo);
            Map(a => a.BuildingName);
            Map(a => a.StreetName);
            Map(a => a.City);
            Map(a => a.PinCode);
            Map(a => a.State);
            Map(a => a.Country);
            References(a=>a.User).Column("UserId").Cascade.None();

        }
    }
}