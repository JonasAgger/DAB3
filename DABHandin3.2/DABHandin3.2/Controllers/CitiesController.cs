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
    public class CitiesController : ApiController
    {
        private readonly UnitOfWork<City> _uow = new UnitOfWork<City>(new Context());
        // GET: api/Cities
        public IQueryable<City> GetCities()
        {
            return _uow.Repo.ReadAll();
        }

        // GET: api/Cities/5
        [ResponseType(typeof(City))]
        public async Task<IHttpActionResult> GetCity(int id)
        {
            City city = _uow.Repo.Read(id);
            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        // PUT: api/Cities/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCity(int id, City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != city.Id)
            {
                return BadRequest();
            }

            _uow.Repo.Update(id, city);

            try
            {
                _uow.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
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

        // POST: api/Cities
        [ResponseType(typeof(City))]
        public async Task<IHttpActionResult> PostCity(City city)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _uow.Repo.Create(city);
            //db.Cities.Add(city);
            //await db.SaveChangesAsync();
            _uow.Commit();

            return CreatedAtRoute("DefaultApi", new { id = city.Id }, city);
        }

        // DELETE: api/Cities/5
        [ResponseType(typeof(City))]
        public async Task<IHttpActionResult> DeleteCity(int id)
        {
            City city = _uow.Repo.Read(id);
            if (city == null)
            {
                return NotFound();
            }

            _uow.Repo.Delete(city);
            _uow.Commit();

            return Ok(city);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _uow.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CityExists(int id)
        {
            return _uow.Repo.ReadAll().Count(e => e.Id == id) > 0;
        }
    }
}