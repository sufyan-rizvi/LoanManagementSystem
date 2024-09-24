﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoanManagementSystem.Models
{
    public class User
    {
        public virtual int UserId { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual Address Address { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Role Role { get; set; }



    }
}