using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CM.Models;

namespace CM.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Agenda()
        {
            ViewData["Message"] = "Your agenda";

            return View();
        }

        public IActionResult Appointment()
        {
            ViewData["Message"] = "Your agenda";

            return View();
        }

        public IActionResult Beheerder()
        {
            ViewData["Message"] = "Your agenda";

            return View();
        }

        public IActionResult Dashboard()
        {
            ViewData["Message"] = "Your agenda";

            return View();
        }

        public IActionResult Login()
        {
            ViewData["Message"] = "Your agenda";

            return View();
        }

        public IActionResult MyAccount()
        {
            ViewData["Message"] = "Your agenda";

            return View();
        }

        public IActionResult Register()
        {
            ViewData["Message"] = "Your agenda";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
