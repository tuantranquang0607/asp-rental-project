using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public class EmailConfiguration
    {
        public string SmtpServer { get; set; } = default!;

        public int Port { get; set; }

        public string UserName { get; set; } = default!;

        public string Password { get; set; } = default!;

        public string From { get; set; } = default!;

        public bool EnableSSL { get; set; }
    }
}
