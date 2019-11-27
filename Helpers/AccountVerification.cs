using CM.Context.Interfaces;
using CM.Context.SQL;
using CM.Models;
using CM.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Helpers
{
    public class AccountVerification
    {
        Account account = new Account();
        IAccountContext iaccountcontext;
        AccountRepo accountrepo;

        private string con;

        public AccountVerification(string con)
        {
            this.con = con;
            iaccountcontext = new AccountMsSqlContext(con);
            accountrepo = new AccountRepo(iaccountcontext);
        }

        public bool CheckIfLoggedIn(int? accountid)
        {
            if(accountid == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
