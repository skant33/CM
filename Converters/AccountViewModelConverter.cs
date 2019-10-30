using CM.Models;
using CM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CM.Converters
{
    public class AccountViewModelConverter
    {
        public Account ViewModelToAccount(AccountDetailViewModel viewmodel)
        {
            Account account = new Account()
            {
                AccountID = viewmodel.AccountID,
                Password = viewmodel.Password,
                AccountRole = viewmodel.AccountRole,
                MeldingID = viewmodel.MeldingID,
                Name = viewmodel.Name,
                DateOfBirth = viewmodel.DateOfBirth,
                Email = viewmodel.Email,
                PhoneNumber = viewmodel.PhoneNumber
            };
            return account;
        }
        public AccountDetailViewModel AccountToViewModel(Account account)
        {
            AccountDetailViewModel viewmodel = new AccountDetailViewModel()
            {
                AccountID = account.AccountID,
                Password = account.Password,
                AccountRole = account.AccountRole,
                MeldingID = account.MeldingID,
                Name = account.Name,
                DateOfBirth = account.DateOfBirth,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber
            };
            return viewmodel;
        }
    }
}
