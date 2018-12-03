using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using StatisticsWebAPI.Helpers;
using StatisticsWebAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace StatisticsWebAPI.Services
{
    public class AuthMessageService: IEmailSender
    {
        public AuthMessageService(){}

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(EmailSettings.MailAddressEmail, "Test")
            };

            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            using (SmtpClient smtp = new SmtpClient(EmailSettings.SmtpClientHost, EmailSettings.SmtpClientPort))
            {
                smtp.Credentials = new NetworkCredential(EmailSettings.MailAddressEmail, EmailSettings.MailAddressPassword);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail);
            }
        }
    }
}
