using DABHandin3._2.Data;

namespace DABHandin3._2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Email : Entity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Email()
        {
            People = new HashSet<Person>();
        }

        public int Id { get; set; }

        [Required]
        public string MailAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Person> People { get; set; }
    }

    public class EmailDTO : Entity
    {
        public int ID { get; set; }
        public string MailAddress { get; set; }
        public int Person_Id { get; set; }
    }
}
