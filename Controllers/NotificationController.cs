using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CM.Helpers.MailClasses;
using CM.Models;
using CM.Text;
using CM.Voice.VoiceApi.Sdk;
using CM.Voice.VoiceApi.Sdk.Models;
using CM.Voice.VoiceApi.Sdk.Models.Instructions.Apps;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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

        public async Task SendSMS(Appointment appointment)
        {
            List<string> receivers = new List<string>();
            receivers.Add(appointment.patient.PhoneNumber);

            var client = new TextClient(new Guid(config.GetSection("ApiKey").Value));
            var result = await client.SendMessageAsync(String.Format("You have an appointment at {0} with doctor {1}. Description: {2}", appointment.DateTime, appointment.doctor.Name, appointment.Description), "CMProftaak", receivers, null).ConfigureAwait(false);
        }

        public async Task SendPhoneConversation(Appointment appointment)
        {
            try
            {
                var myApiKey = Guid.Parse("E4802F51-F6A2-474A-8883-3CDB2EAACDB3");
                var httpClient = new HttpClient();
                var client = new VoiceApiClient(httpClient, myApiKey);
                httpClient.DefaultRequestHeaders.Add("X-CM-PRODUCTTOKEN", "E4802F51-F6A2-474A-8883-3CDB2EAACDB3");
                var instruction = new NotificationInstruction
                {
                    Caller = "0031637328840", // stevensnummer
                    Callee = appointment.patient.PhoneNumber ,
                    Prompt = "You have an appointment at " + appointment.DateTime + " with doctor " + appointment.doctor.Name + ". Description: " + appointment.Description      ,
                    MaxReplays = 2,
                    ReplayPrompt = "Press 1 to repeat this message."
                };           
                var result = await client.SendAsync(instruction).ConfigureAwait(false);
            }
            catch
            {

            }
        }

        public async Task SendEmail(Appointment appointment)
        {
            Mail mail = new Mail()
            {
                FromAddressID = new Guid("c10d75e0-bb6b-4bad-8050-a1d0f794acda"),
                ToAddress = appointment.patient.Email,
                TextBody = String.Format("You have an appointment at {0} with doctor {1}. Description: {2}", appointment.DateTime, appointment.doctor.Name, appointment.Description),
                Subject = String.Format("Appointment at {0}", appointment.DateTime)
            };
            StringContent content = new StringContent(JsonConvert.SerializeObject(mail), Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("X-CM-PRODUCTTOKEN", "E4802F51-F6A2-474A-8883-3CDB2EAACDB3");
            HttpResponseMessage message = await client.PostAsync("https://api.cmtelecom.com/bulkemail/v1.0/accounts/44D51DE6-3DF0-46A0-BA49-B7D26E3B30B6/mails", content);
            string response = await message.Content.ReadAsStringAsync();
        }

    }   

}