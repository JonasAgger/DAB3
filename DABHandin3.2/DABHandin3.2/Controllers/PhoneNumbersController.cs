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
using DABHandin3._2.Models;

namespace DABHandin3._2.Controllers
{
    public class PhoneNumbersController : ApiController
    {
        private Context db = new Context();

        // GET: api/PhoneNumbers
        public IQueryable<PhoneNumber> GetPhoneNumbers()
        {
            return db.PhoneNumbers;
        }

        // GET: api/PhoneNumbers/5
        [ResponseType(typeof(PhoneNumber))]
        public async Task<IHttpActionResult> GetPhoneNumber(int id)
        {
            PhoneNumber phoneNumber = await db.PhoneNumbers.FindAsync(id);
            if (phoneNumber == null)
            {
                return NotFound();
            }

            return Ok(phoneNumber);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PhoneNumbers.Add(phoneNumber);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = phoneNumber.Id }, phoneNumber);
        }

        // DELETE: api/PhoneNumbers/5
        [ResponseType(typeof(PhoneNumber))]
        public async Task<IHttpActionResult> DeletePhoneNumber(int id)
        {
            PhoneNumber phoneNumber = await db.PhoneNumbers.FindAsync(id);
            if (phoneNumber == null)
            {
                return NotFound();
            }

            db.PhoneNumbers.Remove(phoneNumber);
            await db.SaveChangesAsync();

            return Ok(phoneNumber);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhoneNumberExists(int id)
        {
            return db.PhoneNumbers.Count(e => e.Id == id) > 0;
        }
    }
}