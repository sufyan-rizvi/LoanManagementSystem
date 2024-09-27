﻿using System;
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

        public void AddOfficer(LoanOfficer officer)
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

        public void AddScheme(LoanScheme scheme)
        {
            using (var txn = _session.BeginTransaction())
            {
                scheme.IsActive = true;
                _session.Save(scheme);
            }
        }

        public void DeleteOfficer(LoanOfficer officer)
        {
            using (var txn = _session.BeginTransaction())
            {
                var existingOfficer = _session.Query<LoanOfficer>().FirstOrDefault(l => l.OfficerId == officer.OfficerId);
                existingOfficer.User.IsActive = !existingOfficer.User.IsActive;
                _session.Update(existingOfficer);
                txn.Commit();                
            }
        }
        public IList<LoanOfficer> GetAllOfficers()
        {
            return _session.Query<LoanOfficer>().Fetch(l => l.User).ThenFetch(u => u.Address).ToList();
        }

        public void DeleteScheme(LoanScheme scheme)
        {
            using(var txn = _session.BeginTransaction())
            {
                var existingScheme = _session.Query<LoanScheme>().FirstOrDefault(s=>s.LoanSchemeId == scheme.LoanSchemeId);
                existingScheme.IsActive = !existingScheme.IsActive;
                _session.Update(existingScheme);
                txn.Commit();
            }
        }

        

        public IList<LoanScheme> GetAllSchemes()
        {
            return _session.Query<LoanScheme>().ToList();
        }

        public LoanOfficer GetByOfficerEmail(string email)
        {
            return _session.Query<LoanOfficer>().FirstOrDefault(l=>l.User.Email == email);  
        }

        public LoanOfficer GetByOfficerId(Guid id)
        {
            return _session.Query<LoanOfficer>().FirstOrDefault(l => l.OfficerId == id);
        }

        public LoanOfficer GetByOfficerUsername(string username)
        {
            return _session.Query<LoanOfficer>().FirstOrDefault(l => l.User.Username == username);
        }

        public void UpdateOfficer(LoanOfficer officer)
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

        public void UpdateScheme(LoanScheme scheme)
        {
            using(var txn = _session.BeginTransaction())
            {
                _session.Merge(scheme);
                txn.Commit();
            }
        }

        public LoanScheme GetSchemeById(Guid id)
        {
            return _session.Query<LoanScheme>().FirstOrDefault(s=>s.LoanSchemeId == id);
        }
    }
}