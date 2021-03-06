﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CM.ViewModels
{
    public class AccountDetailViewModel
    {
        public int AccountID { get; set; }
        public string Password { get; set; }
        public int AccountRole { get; set; }
        public int MeldingID { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

        public AccountDetailViewModel()
        {

        }
    }
}
