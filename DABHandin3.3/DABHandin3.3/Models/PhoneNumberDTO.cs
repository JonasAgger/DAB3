using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DABHandin3._3.Models
{
    public class PhoneNumberDTO
    {
        public PhoneNumberDTO()
        {

        }

        public PhoneNumberDTO(PhoneNumber phoneNumber)
        {
            Number = phoneNumber.Number;
            Type = phoneNumber.Type;
            Company = phoneNumber.Company;
        }

        public PhoneNumber ToPhoneNumber()
        {
            return new PhoneNumber() {Number = Number, Company = Company, Type = Type};
        }

        public string Number { get; set; }
        public PhoneType Type { get; set; }
        public string Company { get; set; }
    }
}