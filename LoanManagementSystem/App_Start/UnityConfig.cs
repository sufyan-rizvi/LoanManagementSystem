using System.Web.Mvc;
using LoanManagementSystem.Data;
using LoanManagementSystem.Repository;
using LoanManagementSystem.Service;
using NHibernate;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace LoanManagementSystem
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<ISession>(new InjectionFactory(c => NhibernateHelper.CreateSession()));
            container.RegisterType<IAdminService, AdminService>();
            container.RegisterType<IAdminRepository, AdminRepository>();
            container.RegisterType<IAccountRepository, AccountRepository>();
            container.RegisterType<IAccountService, AccountService>();
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}