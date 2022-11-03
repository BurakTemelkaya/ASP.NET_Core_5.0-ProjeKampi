using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.MailUtilities.Models
{
    public class ChangedUserInformationModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string NameSurname { get; set; }
        public string About { get; set; }
        public string City { get; set; }
    }
}
