using DABHandin3._2.Data;

namespace DABHandin3._2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class PhoneNumber : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhoneNumber()
        {
            People = new HashSet<Person>();
        }

        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        public int Type { get; set; }

        public string Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person> People { get; set; }
    }

    public class PhoneDTO : Entity
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Company { get; set; }
        public int Type { get; set; }
    }
}
