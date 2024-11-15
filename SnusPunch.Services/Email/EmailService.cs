using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnusPunch.Shared.Models.Auth;
using SnusPunch.Shared.Models.Identity;
using SnusPunch.Shared.Models.ResultModel;
using System.Net;
using System.Net.Mail;

namespace SnusPunch.Services.Email
{
    public class EmailService
    {
        private readonly ILogger<EmailService> mLogger;
        private readonly IConfiguration mConfiguration;

        public EmailService(ILogger<EmailService> aLogger, IConfiguration aConfiguration)
        {
            mLogger = aLogger;
            mConfiguration = aConfiguration;
        }

        public async Task<ResultModel> SendEmail(string aRecipient, string aHeader, string aBody)
        {
            ResultModel sResultModel = new ResultModel();

            try
            {
                SmtpClient sSmtpClient = new SmtpClient
                {
                    Port = Convert.ToInt32(mConfiguration["SmtpSettings:Port"]),
                    Host = mConfiguration["SmtpSettings:Host"],
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(mConfiguration["SmtpSettings:Username"], mConfiguration["SmtpSettings:Password"])
                };

                MailMessage sMailMessage = new MailMessage
                {
                    From = new MailAddress(mConfiguration["SmtpSettings:Username"], "SnusPunch"),
                    Subject = aHeader,
                    Body = aBody,
                };

                sMailMessage.To.Add(aRecipient);

                await sSmtpClient.SendMailAsync(sMailMessage);
            }
            catch(Exception ex)
            {
                sResultModel.Success = false;
                sResultModel.AddExceptionError(ex);
            }

            return sResultModel;
        }
    }
}
