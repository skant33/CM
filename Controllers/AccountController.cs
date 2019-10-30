using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CM.Context.Interfaces;
using CM.Context.SQL;
using CM.Converters;
using CM.Models;
using CM.Repositories;
using CM.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace CM.Controllers
{
    public class AccountController : Controller
    {
        //account
        AccountViewModelConverter accountViewModelConverter = new AccountViewModelConverter(); 
        IAccountContext iaccountcontext;
        AccountRepo accountrepo;

        //appointment
        AppointmentViewModel appointmentViewModel = new AppointmentViewModel();
        AppointmentRepo appointmentrepo;
        IAppointmentContext iappointmentContext;

        public AccountController(IConfiguration iconfiguration)
        {
            string con = iconfiguration.GetSection("ConnectionStrings").GetSection("connectionstring").Value;
            iaccountcontext = new AccountMsSqlContext(con);
            accountrepo = new AccountRepo(iaccountcontext);

            iappointmentContext = new AppointmentMsSqlContext(con);
            appointmentrepo = new AppointmentRepo(iappointmentContext);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AccountDetailViewModel viewmodel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;            
            Account inkomend = accountViewModelConverter.ViewModelToAccount(viewmodel);
            Account opgehaald = accountrepo.Login(inkomend);
            appointmentViewModel.appointments = new List<Appointment>();
            if (opgehaald.Email == inkomend.Email)
            {
                foreach (Appointment appointment in appointmentrepo.GetAppointmentsByUserID(opgehaald))
                {                    
                    appointmentViewModel.appointments.Add(appointment);
                }
                return View("~/Views/Home/Dashboard.cshtml", appointmentViewModel);
            }
            else
            {
                return View("~/Views/Home/Login.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(AccountDetailViewModel viewmodel, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            Account inkomend = accountViewModelConverter.ViewModelToAccount(viewmodel);
            if (accountrepo.Register(inkomend) == true)
            {
                //geregistreerd
                return View("~/Views/Home/Login.cshtml");
            }
            else
            {
                //mislukt
                return View("~/Views/Home/Login.cshtml");
            }
        }
    }
}