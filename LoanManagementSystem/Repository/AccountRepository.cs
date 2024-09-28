using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using LoanManagementSystem.Models;
using NHibernate;
using NHibernate.Linq;

namespace LoanManagementSystem.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ISession _session;

        public AccountRepository(ISession session)
        {
            _session = session;
        }

        public void AddCustomer(Customer customer)
        {
            using (var txn = _session.BeginTransaction())
            {
                customer.User.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(customer.User.Password, 6);
                customer.User.IsActive = true;
                customer.User.Address.User = customer.User;
                customer.User.Role = new Role();
                customer.User.Role.RoleName = "Customer";
                customer.User.Role.User = customer.User;
                _session.Save(customer);
                txn.Commit();
            }
        }

        public Admin GetAdminByUserId(Guid id)
        {
            return _session.Query<Admin>().Fetch(a => a.User).ThenFetch(u => u.Address).FirstOrDefault(a => a.User.UserId == id);
        }
        public User GetByUsername(string username)
        {
            return _session.Query<User>().SingleOrDefault(u => u.Username == username);
        }

        public Customer GetCustomerByUserId(Guid id)
        {
            return _session.Query<Customer>().Fetch(a => a.User).ThenFetch(u => u.Address).FirstOrDefault(a => a.User.UserId == id);
        }

        public LoanOfficer GetLoanofficerByUserId(Guid id)
        {
            return _session.Query<LoanOfficer>().Fetch(a => a.User).ThenFetch(u => u.Address).FirstOrDefault(a => a.User.UserId == id);
        }

        public Customer GetCustomerByUsername(string username)
        {
            return _session.Query<Customer>().Fetch(c => c.User).FirstOrDefault(c => c.User.Username == username);
        }

        public Customer GetCustomerByEmail(string email)
        {
            return _session.Query<Customer>().Fetch(c => c.User).FirstOrDefault(c => c.User.Email == email);
        }



    }
}