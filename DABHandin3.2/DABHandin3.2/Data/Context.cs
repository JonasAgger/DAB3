namespace DABHandin3._2.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Context : DbContext
    {
        public Context()
            : base("name=AdressContext")
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Email> Emails { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasMany(e => e.People)
                .WithMany(e => e.Addresses)
                .Map(m => m.ToTable("AddressPersons"));

            modelBuilder.Entity<City>()
                .HasMany(e => e.Addresses)
                .WithOptional(e => e.City)
                .HasForeignKey(e => e.City_Id);

            modelBuilder.Entity<Email>()
                .HasMany(e => e.People)
                .WithMany(e => e.Emails)
                .Map(m => m.ToTable("PersonEmails"));

            modelBuilder.Entity<Person>()
                .HasMany(e => e.PhoneNumbers)
                .WithMany(e => e.People)
                .Map(m => m.ToTable("PhoneNumberPersons"));
        }
    }
}
