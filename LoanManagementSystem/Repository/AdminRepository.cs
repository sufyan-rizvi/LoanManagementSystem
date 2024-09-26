using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LoanManagementSystem.Models;
using NHibernate;
using NHibernate.Linq;
using BC = BCrypt.Net.BCrypt;

namespace LoanManagementSystem.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ISession _session;

        public AdminRepository(ISession session)
        {
            _session = session;
        }

        public void Add(LoanOfficer officer)
        {
            using (var txn = _session.BeginTransaction())
            {
                officer.User.Password = BC.EnhancedHashPassword(officer.User.Password, 6);
                officer.User.Address.User = officer.User;
                officer.User.IsActive = true;
                _session.Save(officer);
                txn.Commit();
            }
        }

        public void Delete(LoanOfficer officer)
        {
            using (var txn = _session.BeginTransaction())
            {
                var Officer = _session.Query<LoanOfficer>().FirstOrDefault(l => l.OfficerId == officer.OfficerId);
                Officer.User.IsActive = !Officer.User.IsActive;
                _session.Update(Officer);
                txn.Commit();                
            }
        }

        public IList<LoanOfficer> GetAll()
        {
            return _session.Query<LoanOfficer>().Fetch(l=>l.User).ThenFetch(u=>u.Address).ToList();            
        }

        public LoanOfficer GetByEmail(string email)
        {
            return _session.Query<LoanOfficer>().FirstOrDefault(l=>l.User.Email == email);  
        }

        public LoanOfficer GetByOfficerId(Guid id)
        {
            return _session.Query<LoanOfficer>().FirstOrDefault(l => l.OfficerId == id);
        }

        public LoanOfficer GetByUsername(string username)
        {
            return _session.Query<LoanOfficer>().FirstOrDefault(l => l.User.Username == username);
        }

        public void Update(LoanOfficer officer)
        {
            using (var txn = _session.BeginTransaction())
            {
                var existingOfficer = _session.Query<LoanOfficer>().FirstOrDefault(l => l.OfficerId == officer.OfficerId);
                if (existingOfficer != null)
                {
                    //existingOfficer.User.FirstName = officer.User.FirstName;
                    //existingOfficer.User.LastName = officer.User.LastName;
                    //existingOfficer.User.PhoneNumber = officer.User.PhoneNumber;
                    //existingOfficer.User.Email = officer.User.PhoneNumber;
                    //existingOfficer.User.Address.FlatNo = officer.User.Address.FlatNo;
                    //existingOfficer.User.Address.BuildingName = officer.User.Address.BuildingName;
                    //existingOfficer.User.Address.StreetName = officer.User.Address.StreetName;
                    //existingOfficer.User.Address.City = officer.User.Address.City;
                    //existingOfficer.User.Address.PinCode = officer.User.Address.PinCode;
                    //existingOfficer.User.Address.State = officer.User.Address.State;
                    //existingOfficer.User.Address.Country = officer.User.Address.Country;
                    existingOfficer.User = officer.User;

                    _session.Merge(existingOfficer);
                    txn.Commit();
                }
            }
        }
    }
}