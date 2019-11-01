using CM.Context.Interfaces;
using CM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Repositories
{
    public class AccountRepo
    {
        private IAccountContext context;

        public AccountRepo(IAccountContext context)
        {
            this.context = context;
        }
        
        public Account Login(Account account)
        {
            return context.Login(account);
        }

        public bool Register(Account account)
        {
            return context.Register(account);
        }

        public Account GetAccountByID(int AccountID)
        {
            return context.GetAccountByID(AccountID);
        }
    }
}
