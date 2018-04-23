using DABHandin3._2.Data;

namespace DABHandin3._2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Address : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Address()
        {
            People = new HashSet<Person>();
        }

        public int Id { get; set; }

        [Required]
        public string AddressType { get; set; }

        [Required]
        public string StreetName { get; set; }

        [Required]
        public string HouseNumber { get; set; }

        public int? City_Id { get; set; }

        public virtual City City { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person> People { get; set; }
    }

    public class AddressDTO : Entity
    {
        public int Id { get; set; }
        public string AddressType { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public int City_Id { get; set; }
        public CityDTO City { get; set; }
    }
}
