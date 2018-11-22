using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticsWebAPI.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, string smtpClientHost, int smtpClientPort, string mailAddressEmail, string mailAddressPassword);
    }
}
