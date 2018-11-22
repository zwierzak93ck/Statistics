using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticsWebAPI.Helpers
{
    public class EmailSettings
    {
        public string SmtpClientHost { get; set; }
        public int SmtpClientPort { get; set; }
        public string MailAddressEmail { get; set; }
        public string MailAddressPassword { get; set; }
    }
}
