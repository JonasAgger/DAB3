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
    public class PhoneNumbersController : ApiController
    {
        private Context db = new Context();

        // GET: api/PhoneNumbers
        public IQueryable<PhoneDTO> GetPhoneNumbers()
        {
            var uow = new UnitOfWork<PhoneNumber>(db);

            var numbers = from p in uow.Repo.ReadAll()
                select new PhoneDTO
                {
                    Id = p.Id,
                    Number = p.Number,
                    Company = p.Company,
                    Type = p.Type
                };
            return numbers;
        }

        // GET: api/PhoneNumbers/5
        [ResponseType(typeof(PhoneDTO))]
        public async Task<IHttpActionResult> GetPhoneNumber(int id)
        {
            var uow = new UnitOfWork<PhoneNumber>(db);

            var phoneNumber = uow.Repo.Read(id);

            if (phoneNumber == null)
            {
                return NotFound();
            }


            var number = new PhoneDTO
                {
                    Id = phoneNumber.Id,
                    Number = phoneNumber.Number,
                    Company = phoneNumber.Company,
                    Type = phoneNumber.Type
                };

            return Ok(number);
        }

        // PUT: api/PhoneNumbers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPhoneNumber(int id, PhoneNumber phoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != phoneNumber.Id)
            {
                return BadRequest();
            }

            db.Entry(phoneNumber).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneNumberExists(id))
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

        // POST: api/PhoneNumbers
        [ResponseType(typeof(PhoneNumber))]
        public async Task<IHttpActionResult> PostPhoneNumber(PhoneNumber phoneNumber)
        {
            var uow = new UnitOfWork<PhoneNumber>(db);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            uow.Repo.Create(phoneNumber);

            uow.Commit();

            return CreatedAtRoute("DefaultApi", new { id = phoneNumber.Id }, phoneNumber);
        }

        // DELETE: api/PhoneNumbers/5
        [ResponseType(typeof(PhoneNumber))]
        public async Task<IHttpActionResult> DeletePhoneNumber(int id)
        {
            var uow = new UnitOfWork<PhoneNumber>(db);

            PhoneNumber phoneNumber = uow.Repo.Read(id);

            if (phoneNumber == null)
            {
                return NotFound();
            }

            uow.Repo.Delete(phoneNumber);

            uow.Commit();

            return Ok(phoneNumber);
        }

        protected override void Dispose(bool disposing)
        {
            var uow = new UnitOfWork<PhoneNumber>(db);

            if (disposing)
            {
                uow.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhoneNumberExists(int id)
        {
            var uow = new UnitOfWork<PhoneNumber>(db);

            return uow.Repo.ReadAll().Count(e => e.Id == id) > 0;
        }
    }
}