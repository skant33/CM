﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Models
{
    public class Account
    {
        public int AccountID { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public int MeldingID { get; set; }

        public AccountRole RoleId = new AccountRole();

        public override string ToString()
        {
            return Name;
        }
    }
}
