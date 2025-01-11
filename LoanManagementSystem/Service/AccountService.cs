using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;

namespace LoanManagementSystem.Service
{
    public class AccountService:IAccountService
    {
        private readonly IAccountRepository _accRepo;

        public AccountService(IAccountRepository accRepo)
        {
            _accRepo = accRepo;
        }

        public void AddAdmin(Admin admin)
        {
            _accRepo.AddAdmin(admin);
        }

        public User GetUserByUsername(string username)
        {
            return _accRepo.GetByUsername(username);
        }

        public Admin GetAdminByUserId(Guid id)
        {
            return _accRepo.GetAdminByUserId(id);
        }

        public LoanOfficer GetOfficerByUserId(Guid id)
        {
            return _accRepo.GetLoanofficerByUserId(id);
        }

        public Customer GetCustomerByUserId(Guid id)
        {
            return _accRepo.GetCustomerByUserId(id);
        }

        public void AddCustomer(Customer customer)
        {
            var usernameCheck = _accRepo.GetCustomerByUsername(customer.User.Username);
            if (usernameCheck != null)                
                throw new InvalidOperationException("A customer with the same username exists!");
            var emailCheck = _accRepo.GetCustomerByEmail(customer.User.Email);
            if (usernameCheck != null)
                throw new InvalidOperationException("A customer with the same email exists!");
            _accRepo.AddCustomer(customer);
        }

        public bool CheckUserNameFound(string username)
        {
            return _accRepo.GetCustomerByUsername(username) != null;
        }

        public bool CheckEmailFound(string email)
        {
            return _accRepo.GetCustomerByEmail(email) != null;
        }


    }
}