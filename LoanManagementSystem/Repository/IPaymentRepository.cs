using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public interface IPaymentRepository
    {
        void SaveRepayment(Repayment repayment);
    }
}
