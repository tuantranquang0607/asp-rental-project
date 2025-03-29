using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public interface IEmailSender // It comes internal as default, which isnt what we want because it cant be referenced, so make it public.
    {
        void Send(EmailMessage message);
    }
}
