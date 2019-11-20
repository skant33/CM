using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CM.Text;
using CM.Voice.VoiceApi.Sdk;
using CM.Voice.VoiceApi.Sdk.Models.Instructions.Apps;
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

        public async Task<IActionResult> SendSMS()
        {
            List<string> receivers = new List<string>();
            receivers.Add("0031627404177");

            var client = new TextClient(new Guid(config.GetSection("ApiKey").Value));
            var result = await client.SendMessageAsync("Hoi", "CMProftaak", receivers, null).ConfigureAwait(false);
           
            return View("~/Views/Home/Login.cshtml");
        }

        public async Task<IActionResult> SendPhoneConversation()
        {
            var myApiKey = Guid.Parse(config.GetSection("X-CM-PRODUCTTOKEN").Value);
            var httpClient = new HttpClient();
            var client = new VoiceApiClient(httpClient, myApiKey);            
            var instruction = new NotificationInstruction
            {
                Caller = "0031627404177",
                Callee = "0031634698094",
                Prompt = "hoi max je bent echt lelijk, ga maar goed werken kehba",
            };
            var result = await client.SendAsync(instruction).ConfigureAwait(false);
            return View("~/Views/Home/Login.cshtml");

        }
    }
}