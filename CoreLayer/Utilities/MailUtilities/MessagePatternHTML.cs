using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.MailUtilities
{
    public class MessagePatternHTML
    {
        public static string ContactMessageHTML(string name, string subject, string message)
        {
            return "<h1>You have received a new message</h1>" +
                "<h2> Name: " + name + "</h2>" +
                "<h2>Subject : " + subject + "</h2>" +
                "<h2>Message : " + message + "</h2>";
        }
        public static string PasswordMessageHTML(string link)
        {
            return "<h1>You can change the password by clicking the link below. If you haven't done this, ignore it.</h1>" +
                "<h2>Link : " + link + "</h2>";
        }
    }
}
