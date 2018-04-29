using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using DABHandin3._2.Data;
using DABHandin3._2.Models;


namespace DABHandin3._2.Controllers
{
    public class PeopleController : ApiController
    {
        private Context db = new Context();

        // GET: api/People
        public IQueryable<PersonDTO> GetPeople()
        {
            var uow = new UnitOfWork<Person>(db);

            var people = from p in uow.Repo.ReadAll()
                select new PersonDTO
                {
                    Id = p.Id,
                    EmailDtos = from e in p.Emails
                        select new EmailDTO
                        {
                            ID = e.Id,
                            MailAddress = e.MailAddress
                        },
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName,
                    PhoneDtos = from n in p.PhoneNumbers
                        select new PhoneDTO
                        {
                            Id = n.Id,
                            Number = n.Number,
                            Company = n.Company,
                            Type = n.Type
                        },
                    AddressDtos = from a in p.Addresses
                        select new AddressDTO
                        {
                            Id = a.Id,
                            HouseNumber = a.HouseNumber,
                            StreetName = a.StreetName,
                            City = new CityDTO
                            {
                                CityName = a.City.CityName,
                                Id = a.City.Id,
                                ZipCode = a.City.ZipCode
                            }

                        }
                };

            return people;
        }

        // GET: api/People/5
        [ResponseType(typeof(PersonDTO))]
        public async Task<IHttpActionResult> GetPerson(int id)
        {
            var uow = new UnitOfWork<Person>(db);

            Person person = uow.Repo.Read(id);
            if (person == null)
            {
                return NotFound();
            }

            var persondto = new PersonDTO
            {
                Id = person.Id,
                EmailDtos = from e in person.Emails
                    select new EmailDTO
                    {
                        ID = e.Id,
                        MailAddress = e.MailAddress
                    },
                FirstName = person.FirstName,
                MiddleName = person.MiddleName,
                LastName = person.LastName,
                PhoneDtos = from n in person.PhoneNumbers
                    select new PhoneDTO
                    {
                        Id = n.Id,
                        Number = n.Number,
                        Company = n.Company,
                        Type = n.Type
                    },
                AddressDtos = from a in person.Addresses
                    select new AddressDTO
                    {
                        Id = a.Id,
                        HouseNumber = a.HouseNumber,
                        StreetName = a.StreetName,
                        City = new CityDTO
                        {
                            CityName = a.City.CityName,
                            Id = a.City.Id,
                            ZipCode = a.City.ZipCode
                        }

                    }
            };

            return Ok(persondto);
        }

        // PUT: api/People/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPerson(int id, Person person)
        {
            var uow = new UnitOfWork<Person>(db);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.Id)
            {
                return BadRequest();
            }

            var pers = uow.Repo.Read(id);

            if (pers == null) return NotFound(); // Not found

            pers.FirstName = person.FirstName;
            pers.MiddleName = person.MiddleName;
            pers.LastName = person.LastName;
            pers.Type = person.Type;



            if (person.Emails != null)
            {
                // Update emails - sketchy but works
                var emails = new UnitOfWork<Email>(db);
                pers.Emails.Clear();

                foreach (var email in person.Emails)
                {
                    var mails = emails.Repo.ReadAll().FirstOrDefault(em => email.Id == em.Id);

                    // Checking if mailaddress exists 
                    if (mails != null)
                    {
                        pers.Emails.Add(mails);
                    }
                    else
                    {
                        emails.Repo.Create(email);
                        pers.Emails.Add(email);
                    }
                }

                emails.Commit();
            }


            // Same as above, just for addresses
            if (person.Addresses != null)
            {
                var addr = new UnitOfWork<Address>(db);
                pers.Addresses.Clear();
                foreach (var address in person.Addresses)
                {
                    var ad = addr.Repo.ReadAll().Include(e => e.City).FirstOrDefault(e => address.Id == e.Id);
                    if (ad != null)
                    {
                        pers.Addresses.Add(ad);
                    }
                    else
                    {
                        // Creating City if not found
                        var city = new UnitOfWork<City>(db);
                        if (city.Repo.ReadAll().FirstOrDefault(c => c.CityName == address.City.CityName) == null)
                        {
                            city.Repo.Create(address.City);
                            city.Commit();
                        }

                        addr.Repo.Create(address);
                        pers.Addresses.Add(address);

                    }
                }

                addr.Commit();
            }



            if (person.PhoneNumbers != null)
            {
                var numbers = new UnitOfWork<PhoneNumber>(db);
                pers.PhoneNumbers.Clear();

                foreach (var number in person.PhoneNumbers)
                {
                    var phNumber = numbers.Repo.ReadAll().FirstOrDefault(p => p.Id == number.Id);
                    if (phNumber != null)
                    {
                        pers.PhoneNumbers.Add(phNumber);
                    }
                    else
                    {
                        numbers.Repo.Create(number);
                        pers.PhoneNumbers.Add(number);
                    }
                }
            }

            uow.Repo.Update(id, pers);

            try
            {
                uow.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/People
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> PostPerson(Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var uow = new UnitOfWork<Person>(db);

            uow.Repo.Create(person);

            uow.Commit();

            PersonDTO pers = uow.Repo.ReadAll()
                .Include(p => p.Emails).Include(p => p.PhoneNumbers).Include(p => p.Addresses)
                .Select(p => new PersonDTO()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    EmailDtos = from email in p.Emails
                        select new EmailDTO
                        {
                            ID = email.Id,
                            MailAddress = email.MailAddress
                        },
                    PhoneDtos = from number in p.PhoneNumbers
                        select new PhoneDTO
                        {
                            Id = number.Id,
                            Company = number.Company,
                            Type = number.Type
                        },
                    AddressDtos = from adr in p.Addresses
                        select new AddressDTO()
                        {
                            Id = adr.Id,
                            HouseNumber = adr.HouseNumber,
                            StreetName = adr.StreetName,
                            City_Id = adr.City_Id,
                            City = new CityDTO() { CityName = adr.City.CityName, ZipCode = adr.City.ZipCode}
                        }
                }).SingleOrDefault(p => p.Id == person.Id);

            return CreatedAtRoute("DefaultApi", new { id = person.Id }, pers);
        }

        // DELETE: api/People/5
        [ResponseType(typeof(Person))]
        public async Task<IHttpActionResult> DeletePerson(int id)
        {
            var uow = new UnitOfWork<Person>(db);

            Person person = uow.Repo.Read(id);
            if (person == null)
            {
                return NotFound();
            }

            uow.Repo.Delete(person);

            uow.Commit();

            return Ok(person);
        }

        protected override void Dispose(bool disposing)
        {
            var uow = new UnitOfWork<Person>(db);

            if (disposing)
            {
                uow.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonExists(int id)
        {
            var uow = new UnitOfWork<Person>(db);

            return uow.Repo.ReadAll().Count(e => e.Id == id) > 0;
        }
    }
}