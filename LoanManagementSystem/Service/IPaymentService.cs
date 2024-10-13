using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Service
{
    public interface IPaymentService
    {
        void SaveRepayment(Repayment repayment);
    }
}
