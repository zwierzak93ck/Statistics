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

        public Task SendEmailAsync(string email, string subject, string message, string smtpClientHost, int smtpClientPort, string mailAddressEmail, string mailAddressPassword)
        {
            return Execute(subject, message, email, smtpClientHost, smtpClientPort, mailAddressEmail, mailAddressPassword);
        }

        public async Task Execute(string subject, string message, string email, string smtpClientHost, int smtpClientPort, string mailAddressEmail, string mailAddressPassword)
        {
           
            MailMessage mail = new MailMessage()
            {
                From = new MailAddress(mailAddressEmail, "Test")
            };

            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;

            using (SmtpClient smtp = new SmtpClient(smtpClientHost, smtpClientPort))
            {
                smtp.Credentials = new NetworkCredential(mailAddressEmail, mailAddressPassword);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail);
            }

        }
    }
}
