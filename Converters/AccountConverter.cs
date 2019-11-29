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
                Password = ADVM.Password,
                //AccountRole =ADVM.AccountRole,
                MeldingID = ADVM.MeldingID,
                DateOfBirth = ADVM.DateOfBirth,
                PhoneNumber = ADVM.PhoneNumber
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
                Password = Account.Password,
                //AccountRole = Account.AccountRole,
                MeldingID = Account.MeldingID,
                DateOfBirth = Account.DateOfBirth,
                PhoneNumber = Account.PhoneNumber
            };
            return ADVM;
        }
    }
}
