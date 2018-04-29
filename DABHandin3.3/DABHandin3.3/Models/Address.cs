using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DABHandin3._3.Models
{
    public class Address
    {
        //public int Id { get; set; }
        public string AddressType { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public virtual City City { get; set; }
        //public virtual Person[] Persons { get; set; }
    }
    public class City
    {
        //public int Id { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }
    }
}