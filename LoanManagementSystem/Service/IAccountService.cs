using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface IAccountService
    {
        User GetUserByUsername(string username);
        Admin GetAdminByUserId(Guid id);
        LoanOfficer GetOfficerByUserId(Guid id);
        Customer GetCustomerByUserId(Guid id);
        void AddCustomer(Customer customer);

    }
}
