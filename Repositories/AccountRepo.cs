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

        public bool CheckIfAdmin (int? accountid)
        {
            return context.CheckIfAdmin(accountid);
        }

        public Account GetAccountByEmail(string email)
        {
            return context.GetAccountByEmail(email);
        }

        public List<Account> GetAllPatients()
        {
            return context.GetAllPatients();
        }

        public List<Account> GetAllDoctors()
        {
            return context.GetAllDoctors();
        }

        public bool LinkAccounts(int patientid, int doctorid)
        {
            return context.LinkAccounts(patientid, doctorid);
        }
    }
}
