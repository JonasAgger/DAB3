using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DABHandin3._3.Models
{
    public enum PhoneType
    {
        Work,
        Home,
        Mobile
    }
    public class PhoneNumber
    {
        //public int Id { get; set; }
        public string Number { get; set; }
        public PhoneType Type { get; set; } = PhoneType.Mobile;
        public string Company { get; set; }
        //public virtual Person[] Persons { get; set; } = new Person[] { };
    }
}