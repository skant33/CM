using CM.Models;
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

        Account GetAccountByID(int AccountID);

        bool CheckIfAdmin(int? accountid);
        Account GetAccountByEmail(string email);
    }
}
