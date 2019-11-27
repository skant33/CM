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

        int CheckRoleID(int? accountid);

        Account GetAccountByEmail(string email);

        List<Account> GetAllPatients();

        List<Account> GetAllDoctors();

        bool LinkAccounts(int patientid, int doctorid);

        List<Account> GetLinkedPatientsByDoctorID(int doctorid);
    }
}
