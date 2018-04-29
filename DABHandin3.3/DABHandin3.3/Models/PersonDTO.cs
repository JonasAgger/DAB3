using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DABHandin3._3.Models
{
    public class PersonDTO
    {
        public PersonDTO()
        {

        }

        public PersonDTO(Person person)
        {
            Id = person.Id;
            FirstName = person.FirstName;
            MiddleName = person.MiddleName;
            LastName = person.LastName;
            Type = person.Type;
            Addresses = new List<AddressDTO>();

            foreach (var personAddress in person.Addresses)
            {
                Addresses.Add(new AddressDTO(personAddress));
            }

            Emails = new List<EmailDTO>();

            foreach (var personEmail in person.Emails)
            {
                Emails.Add(new EmailDTO(personEmail));
            }

            PhoneNumbers = new List<PhoneNumberDTO>();

            foreach (var personPhoneNumber in person.PhoneNumbers)
            {
                PhoneNumbers.Add(new PhoneNumberDTO(personPhoneNumber));
            }
        }

        public Person ToPerson()
        {
            return new Person() {Id = Id,FirstName = FirstName,MiddleName = MiddleName,LastName = LastName, Addresses = Addresses.Select(pa => pa.ToAddress()).ToList(), Emails = Emails.Select(em => em.ToEmail()).ToList(), PhoneNumbers = PhoneNumbers.Select(pn => pn.ToPhoneNumber()).ToList(), Type = Type};
        }


        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Type Type { get; set; } = Type.Worker;
        public List<EmailDTO> Emails { get; set; }
        public List<PhoneNumberDTO> PhoneNumbers { get; set; }
        public List<AddressDTO> Addresses { get; set; }

    }
}