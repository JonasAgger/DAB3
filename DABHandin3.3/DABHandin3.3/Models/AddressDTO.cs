using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DABHandin3._3.Models
{
    public class AddressDTO
    {
        public AddressDTO()
        {

        }

        public AddressDTO(Address address)
        {
            AddressType = address.AddressType;
            HouseNumber = address.HouseNumber;
            StreetName = address.StreetName;
            CityDTO = new CityDTO();
        }

        public Address ToAddress()
        {
            return new Address() {AddressType = AddressType, City = CityDTO.ToCity()};
        }

        public string AddressType { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public virtual CityDTO CityDTO { get; set; }
    }

    public class CityDTO
    {
        public CityDTO()
        {

        }

        public CityDTO(City city)
        {
            CityName = city.CityName;
            ZipCode = city.ZipCode;
        }

        public City ToCity()
        {
            return new City() {CityName = CityName, ZipCode = ZipCode};
        }


        public string CityName { get; set; }
        public string ZipCode { get; set; }
    }
}