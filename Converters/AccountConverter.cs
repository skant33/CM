using CM.Models;
using CM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Converters
{
    public class AccountConverter
    {
        public Account ViewModelToAccount (AccountDetailViewModel ADVM)
        {
            Account Account = new Account()
            {
                AccountID = ADVM.AccountID,
                Name = ADVM.Name,
                Email = ADVM.Email,
                Password = ""
            };
            return Account;
        }

        public AccountDetailViewModel ViewModelFromAccount(Account Account)
        {
            AccountDetailViewModel ADVM = new AccountDetailViewModel()
            {
                AccountID = Account.AccountID,
                Name = Account.Name,
                Email = Account.Email,
                Password = ""
            };
            return ADVM;
        }
    }
}
