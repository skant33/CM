﻿using CM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Context.Interfaces
{
    public interface IAccountContext
    {
        Account Login(Account account);

        bool Register(Account account);

        List<Account> GetAllDoctors();

        List<Account> GetAllPatients();

        bool CheckIfAdmin(int? accountid);

        Account GetAccountByID(int AccountID);
    }
}
