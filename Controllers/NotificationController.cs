using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CM.Models;
using CM.Text;
using CM.Voice.VoiceApi.Sdk;
using CM.Voice.VoiceApi.Sdk.Models;
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

        public async Task<IActionResult> StartHangfire()
        {
            APIInteraction api = new APIInteraction(config);
            await api.StartHangfire();
            return RedirectToAction("Beheerder", "Account");
        }

        public async Task<IActionResult> StopHangfire()
        {
            APIInteraction api = new APIInteraction(config);
            await api.StopHangfire();
            return RedirectToAction("Beheerder", "Account");
        }

        public async Task SendSMS()
        {
            List<string> receivers = new List<string>();
            receivers.Add("0031634698094");

            var client = new TextClient(new Guid(config.GetSection("ApiKey").Value));
            var result = await client.SendMessageAsync("Hoi", "CMProftaak", receivers, null).ConfigureAwait(false);
        }

        public async Task SendPhoneConversation()
        {
            var myApiKey = Guid.Parse("E4802F51-F6A2-474A-8883-3CDB2EAACDB3");
            var httpClient = new HttpClient();
            var client = new VoiceApiClient(httpClient, myApiKey);
            var instruction = new NotificationInstruction
            {
                Caller = "0031627404177",
                Callee = "0031634698094",
                Prompt = "This is a test notification call using the CM voice A P I.",
                MaxReplays = 2,
                ReplayPrompt = "Press 1 to repeat this message."
            };
            var result = await client.SendAsync(instruction).ConfigureAwait(false);
        }

        public async Task SendEmail()
        {

        }
    }
}