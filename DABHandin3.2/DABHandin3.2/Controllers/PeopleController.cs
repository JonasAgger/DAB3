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
                    LastName = p.LastName,
                    PhoneDtos = from n in p.PhoneNumbers
                        select new PhoneDTO
                        {
                            Id = n.Id,
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
            Person person = await db.People.FindAsync(id);
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
                LastName = person.LastName,
                PhoneDtos = from n in person.PhoneNumbers
                    select new PhoneDTO
                    {
                        Id = n.Id,
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

            pers.FirstName = person.FirstName;
            pers.MiddleName = person.MiddleName;
            pers.LastName = person.LastName;
            pers.Type = person.Type;

            for (int i = 0; i < person.Addresses.Count; i++)
            {
                pers.Addresses.//person.Addresses.ToArray()[i].Id
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

            return CreatedAtRoute("DefaultApi", new { id = person.Id }, person);
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