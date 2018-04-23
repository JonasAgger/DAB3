using DABHandin3._2.Data;

namespace DABHandin3._2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class City : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public City()
        {
            Addresses = new HashSet<Address>();
        }

        public int Id { get; set; }

        [Required]
        public string CityName { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Address> Addresses { get; set; }
    }

    public class CityDTO : Entity
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string ZipCode { get; set; }

    }
}
