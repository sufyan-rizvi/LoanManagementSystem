﻿using System;
using System.Collections.Generic;
using LoanManagementSystem.Models;

namespace LoanManagementSystem.Repository
{
    public  interface ILoanOfficerRepository
    {
        List<LoanApplication> GetAllDocuments();
        List<RegistrationDocuments> GetAppDocuments(Guid id);
        void RegApproveLoan(Guid id);
        void RejectApproveLoan(Guid id, string comments);
        List<LoanApplication> GetCollateralDocuments();
        List<CollateralDocuments> GetToShowCollateralDocuments(Guid id);
        void ApproveCollateralDocuments(Guid id, string comments);
        void RejectCollateralDocuments(Guid id, string comments);
    }
}
