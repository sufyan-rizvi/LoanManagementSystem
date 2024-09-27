using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanManagementSystem.Models;
using LoanManagementSystem.Repository;

namespace LoanManagementSystem.Service
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepo;

        public AdminService(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
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
            if (!officer.User.IsActive)
            {
                throw new InvalidOperationException("Inactive Loanofficer!");
            }
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

        public IList<LoanScheme> GetAllSchemes()
        {
            return _adminRepo.GetAllSchemes();
        }

        public LoanOfficer ToggleOfficerDelete(Guid id)
        {
            var userExists = _adminRepo.GetByOfficerId(id);
            if (userExists != null)
            {

                _adminRepo.DeleteOfficer(userExists);
                return userExists;

            }
            throw new InvalidOperationException("No such user Exists!");
        }
    }
}