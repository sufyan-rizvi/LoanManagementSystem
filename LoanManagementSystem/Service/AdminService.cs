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
            var usernameCheck = _adminRepo.GetByUsername(officer.User.Username);
            if (usernameCheck != null && usernameCheck.OfficerId != officer.OfficerId)
            {
                throw new InvalidOperationException("A customer with the same username already exists");
            }
            //check same email
            var emailCheck = _adminRepo.GetByEmail(officer.User.Email);
            if (emailCheck != null && emailCheck.OfficerId != officer.OfficerId)
            {
                throw new InvalidOperationException("A customer with the same email already exists");
            }
            _adminRepo.Add(officer);

        }

        public void EditOfficer(LoanOfficer officer)
        {
            if (!officer.User.IsActive)
            {
                throw new InvalidOperationException("Inactive Loanofficer!");
            }
            var usernameCheck = _adminRepo.GetByUsername(officer.User.Username);
            if (usernameCheck != null && officer.OfficerId != usernameCheck.OfficerId)
            {
                throw new InvalidOperationException("An officer with the same username already exists");
            }
            //check same email
            var emailCheck = _adminRepo.GetByEmail(officer.User.Email);
            if (emailCheck != null && emailCheck.OfficerId != officer.OfficerId)
            {
                throw new InvalidOperationException("An officer with the same email already exists");
            }
            
            _adminRepo.Update(officer);
        }

        public IList<LoanOfficer> GetAllOfficers()
        {
            return _adminRepo.GetAll();
        }

        public LoanOfficer ToggleDelete(Guid id)
        {
            var userExists = _adminRepo.GetByOfficerId(id);
            if (userExists != null)
            {

                _adminRepo.Delete(userExists);
                return userExists;

            }
            throw new InvalidOperationException("No such user Exists!");
        }
    }
}