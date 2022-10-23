using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.MailUtilities
{
    public interface IMailService
    {
        public void SendMail(string mail,string subject, string message);
    }
}
