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

namespace CM.Controllers
{
    public class AppointmentController : Controller
    {
        AppointmentViewModelConverter appointmentViewModelConverter = new AppointmentViewModelConverter();
        AppointmentViewModel appointmentViewModel = new AppointmentViewModel();
        IAppointmentContext iappointmentcontext;
        AppointmentRepo appointmentrepo;

        public AppointmentController(IConfiguration config)
        {
            string con = config.GetSection("ConnectionStrings").GetSection("connectionstring").Value;
            iappointmentcontext = new AppointmentMsSqlContext(con);
            appointmentrepo = new AppointmentRepo(iappointmentcontext);
        }

        public IActionResult Index()
        {
            return View();
        }
       
    }
}