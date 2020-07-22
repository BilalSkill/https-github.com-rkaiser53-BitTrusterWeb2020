using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BitTrusterWebApi.Helper;
using BitTrusterWebApi.Model;
using BitTrusterWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BitTrusterWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _SettingService;
        private readonly AppSettings _appSettings;
        public SettingsController(
            ISettingService settingService,
            IOptions<AppSettings> appSettings)
        {
            _SettingService = settingService;
            _appSettings = appSettings.Value;
            DbContext.ConnectionString = _appSettings.ConnectionString;
        }

        #region Settings
        [HttpGet]
        public dynamic GetAllSettings()
        {
            
            return _SettingService.GetAllSettings();
        }

        [HttpPost]
        public void SetAllSettings(dynamic param)
        {
            _SettingService.SetAllSettings(param);
        }


        [HttpPost("/sendEmail")]
        public void SendTestMail(dynamic param)
        {
            try
            {
                var Server = (string)param.Server;
                var TLS = (bool)param.TLS;
                var UserName = (string)param.UserName;
                var Password = (string)param.Password;

                if (Server == null || Server == "")
                    throw new Exception("Server name is empty");

                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress((string)param.mailFrom);
                mailMessage.To.Add(new MailAddress((string)param.mailTo));
                mailMessage.Subject = "BitTruster Test Mail";
                mailMessage.Body = "Congratulations, the test mail has arrived.";
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.High;

                SmtpClient client = new SmtpClient(Server);

                var ServerParts = Server.Split(':');
                if (ServerParts.Length > 1)
                {
                    client.Host = ServerParts[0];
                    try
                    {
                        client.Port = Convert.ToInt16(ServerParts[1]);
                    }
                    catch
                    {
                        throw new Exception("Portnumber is not a number");
                    }
                }
                client.EnableSsl = (TLS);
                client.Credentials = new NetworkCredential(UserName, Password);
                client.Send(mailMessage);
            }
            catch (Exception e)
            {
                var message = "<br>";

                var innerExeptions = BtExceptionHelper.GetInnerExceptions(e);
                foreach (var exeption in innerExeptions)
                {
                    message += exeption.Message + "<br>";
                }

                throw new Exception(message);
            }

        }
        #endregion
    }
}
