﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;
using static System.Net.Mime.MediaTypeNames;

namespace LoanManagementSystem.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepo;
        private readonly ICustomerRepository _customerRepo;

        public AdminService(IAdminRepository adminRepo, ICustomerRepository customerRepository)
        {
            _adminRepo = adminRepo;
            _customerRepo = customerRepository;
        }

        public IList<LoanApplicationMonthWise> AmountResult()
        {
            return _adminRepo.AmountResult();
        }
        public IList<LoanApplicationMonthWise> NPAResult()
        {
            return _adminRepo.NPAResult();
        }

        public IList<LoanApplicationMonthWise> LoanApplicationMonthWise()
        {
            return _adminRepo.LoanApplicationMonthWise();
        }
        public void AddOfficer(LoanOfficer officer)
        {
            //check same username
            var usernameCheck = _adminRepo.GetByOfficerUsername(officer.User.Username);
            if (usernameCheck != null && usernameCheck.OfficerId != officer.OfficerId)
            {
                throw new InvalidOperationException("A customer with the same username already exists");
            }
            //check same email
            var emailCheck = _adminRepo.GetByOfficerEmail(officer.User.Email);
            if (emailCheck != null && emailCheck.OfficerId != officer.OfficerId)
            {
                throw new InvalidOperationException("A customer with the same email already exists");
            }
            _adminRepo.AddOfficer(officer);

        }
        public List<LoanApplication> LoanReport()
        {
            return (_adminRepo.LoanReport());
        }
        public List<LoanApplication> NPAReport()
        {
            return (_adminRepo.NPAReport());
        }

        public List<LoanScheme> SchemesReportk()
        {
            return (_adminRepo.SchemesReportk());
        }
        public List<LoanOfficer> LoanOfficerReport()
        {
            return (_adminRepo.LoanOfficerReport());
        }

        public void AddScheme(LoanScheme scheme)
        {
            _adminRepo.AddScheme(scheme);
        }

        public LoanScheme DeleteScheme(Guid id)
        {
            var scheme = _adminRepo.GetSchemeById(id);
            if (scheme == null)
                throw new InvalidOperationException("No such scheme found!");
            _adminRepo.DeleteScheme(scheme);
            return scheme;
        }

        public void EditOfficer(LoanOfficer officer)
        {

            var usernameCheck = _adminRepo.GetByOfficerUsername(officer.User.Username);
            if (usernameCheck != null && officer.OfficerId != usernameCheck.OfficerId)
            {
                throw new InvalidOperationException("An officer with the same username already exists");
            }
            //check same email
            var emailCheck = _adminRepo.GetByOfficerEmail(officer.User.Email);
            if (emailCheck != null && emailCheck.OfficerId != officer.OfficerId)
            {
                throw new InvalidOperationException("An officer with the same email already exists");
            }

            _adminRepo.UpdateOfficer(officer);
        }

        public void EditScheme(LoanScheme scheme)
        {
            _adminRepo.UpdateScheme(scheme);
        }

        public IList<LoanOfficer> GetAllOfficers()
        {
            return _adminRepo.GetAllOfficers();
        }

        public IList<LoanOfficer> GetAllActiveOfficers()
        {
            return _adminRepo.GetAllActiveOfficers();
        }

        public IList<LoanScheme> GetAllSchemes()
        {
            return _adminRepo.GetAllSchemes();
        }

        public LoanOfficer GetOfficerById(Guid id)
        {
            return _adminRepo.GetByOfficerId(id);
        }

        public LoanScheme GetSchemeById(Guid id)
        {
            return _adminRepo.GetSchemeById(id);
        }

        public LoanOfficer ToggleOfficerDelete(Guid id)
        {
            var userExists = _adminRepo.GetByOfficerId(id);
            var officerPendingLoanApplications = userExists.LoanApplications;
            Random number = new Random();


            if (userExists != null)
            {
                if (userExists.User.IsActive)
                {
                    _adminRepo.DeleteOfficer(userExists);
                    var currentOfficerWorkforce = _adminRepo.GetAllActiveOfficers();
                    for (var i = 0; i < officerPendingLoanApplications.Count; i++)
                    {
                        officerPendingLoanApplications[i].AssignedOfficer = currentOfficerWorkforce[number.Next(0, currentOfficerWorkforce.Count)];
                        _customerRepo.UpdateApplication(officerPendingLoanApplications[i]);
                    }

                }
                else
                {
                    _adminRepo.DeleteOfficer(userExists);
                }
                return userExists;

            }
            throw new InvalidOperationException("No such user Exists!");
        }
    }
}