using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace LoanManagementSystem.Data
{
    public class NhibernateHelper
    {
        private static ISessionFactory _sessionFactory = null;

        //public static ISession CreateSession()
        //{
        //    if (_sessionFactory == null)
        //    {
        //        _sessionFactory = Fluently.Configure()
        //            .Database(MsSqlConfiguration.MsSql2012.ConnectionString(""))
        //            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<>())
        //            .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(true, true))
        //            .BuildSessionFactory();
        //    }
        //    return _sessionFactory.OpenSession();
        //}
    }
}