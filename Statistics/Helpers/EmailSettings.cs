using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticsWebAPI.Helpers
{
    public static class EmailSettings
    {
        public static string SmtpClientHost { get; set; }
        public static int SmtpClientPort { get; set; }
        public static string MailAddressEmail { get; set; }
        public static string MailAddressPassword { get; set; }
    }
}
