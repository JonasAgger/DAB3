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
    public class AddressesController : ApiController
    {
        
        private Context db = new Context();

        // GET: api/Addresses
        public IQueryable<AddressDTO> GetAddresses()
        {
            var ouw = new UnitOfWork<Address>(db);

            var address = from a in ouw.Repo.ReadAll()
                select new AddressDTO
                {
                    Id = a.Id,
                    StreetName = a.StreetName,
                    HouseNumber = a.HouseNumber,
                    AddressType = a.AddressType,
                    City_Id = a.City_Id,
                    City = new CityDTO
                    {
                        CityName = a.City.CityName,
                        Id = a.City.Id,
                        ZipCode = a.City.ZipCode
                    }
                };
            return address;
        }

        // GET: api/Addresses/5
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> GetAddress(int id)
        {
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            var addressdto = new AddressDTO
            {
                Id = address.Id,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                AddressType = address.AddressType,
                City_Id = address.City_Id,
                City = new CityDTO
                {
                    Id = address.City.Id,
                    CityName = address.City.CityName,
                    ZipCode = address.City.ZipCode
                }
            };
            return Ok(addressdto);
        }

        // PUT: api/Addresses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAddress(int id, Address address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != address.Id)
            {
                return BadRequest();
            }

            db.Entry(address).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
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

        // POST: api/Addresses
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> PostAddress(Address address)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Addresses.Add(address);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = address.Id }, address);
        }

        // DELETE: api/Addresses/5
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> DeleteAddress(int id)
        {
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            db.Addresses.Remove(address);
            await db.SaveChangesAsync();

            return Ok(address);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AddressExists(int id)
        {
            return db.Addresses.Count(e => e.Id == id) > 0;
        }
    }
}