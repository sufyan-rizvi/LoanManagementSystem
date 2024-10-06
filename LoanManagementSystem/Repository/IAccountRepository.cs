using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface IAccountRepository
    {
        User GetByUsername(string username);
        void AddAdmin(Admin admin);
        Admin GetAdminByUserId(Guid id);
        LoanOfficer GetLoanofficerByUserId(Guid id);
        Customer GetCustomerByUserId(Guid id);
        void AddCustomer(Customer customer);
        Customer GetCustomerByUsername(string username);
        Customer GetCustomerByEmail(string email);

    }
}
