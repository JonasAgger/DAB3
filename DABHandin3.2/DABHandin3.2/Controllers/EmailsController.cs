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
    public class EmailsController : ApiController
    {
        private Context db = new Context();

        // GET: api/Emails
        public IQueryable<EmailDTO> GetEmails()
        {
            var ouw = new UnitOfWork<Email>(db);

            var email = from e in ouw.Repo.ReadAll()
                select new EmailDTO
                {
                    ID = e.Id,
                    MailAddress = e.MailAddress
                };

            return email;
        }

        // GET: api/Emails/5
        [ResponseType(typeof(Email))]
        public async Task<IHttpActionResult> GetEmail(int id)
        {
            var ouw = new UnitOfWork<Email>(db);

            Email email = await db.Emails.FindAsync(id);
            if (email == null)
            {
                return NotFound();
            }

            

            return Ok(email);
        }

        // PUT: api/Emails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmail(int id, Email email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != email.Id)
            {
                return BadRequest();
            }

            db.Entry(email).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailExists(id))
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

        // POST: api/Emails
        [ResponseType(typeof(Email))]
        public async Task<IHttpActionResult> PostEmail(Email email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Emails.Add(email);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = email.Id }, email);
        }

        // DELETE: api/Emails/5
        [ResponseType(typeof(Email))]
        public async Task<IHttpActionResult> DeleteEmail(int id)
        {
            Email email = await db.Emails.FindAsync(id);
            if (email == null)
            {
                return NotFound();
            }

            db.Emails.Remove(email);
            await db.SaveChangesAsync();

            return Ok(email);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmailExists(int id)
        {
            return db.Emails.Count(e => e.Id == id) > 0;
        }
    }
}