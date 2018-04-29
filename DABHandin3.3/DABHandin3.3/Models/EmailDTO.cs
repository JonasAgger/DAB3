using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DABHandin3._3.Models
{
    public class EmailDTO
    {
        public EmailDTO()
        {

        }

        public EmailDTO(Email email)
        {
            MailAddress = email.MailAddress;
        }

        public Email ToEmail()
        {
            return new Email(){MailAddress = MailAddress};
        }

        public string MailAddress { get; set; }
    }
}