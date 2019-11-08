using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CM.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CM.Controllers
{
    public class NotificationController : Controller
    {
        private IConfiguration config;
        public NotificationController(IConfiguration config)
        {
            this.config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Verstuur()
        {
            List<string> receivers = new List<string>();
            receivers.Add("0031627404177");

            var client = new TextClient(new Guid(config.GetSection("ApiKey").Value));
            var result = await client.SendMessageAsync("Hoi", "CMProftaak", receivers, null).ConfigureAwait(false);

            return View();
        }
    }
}