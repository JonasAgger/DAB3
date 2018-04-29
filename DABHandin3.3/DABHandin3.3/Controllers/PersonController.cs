using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using DABHandin3._3.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace DABHandin3._3.Controllers
{
    public class PersonController : ApiController
    {
        private const string EndpointUrl = "https://localhost:8081";

        private const string PrimaryKey =
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        
        private DocumentClient _client;

        private PersonRepository _personRepository;

        
        public PersonController()
        {
            _client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            // Create DB
            _client.CreateDatabaseIfNotExistsAsync(new Database { Id = "PersonDB1" }).Wait(2000);
            // Create Collection
            _client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("PersonDB1"),
                new DocumentCollection { Id = "PersonColl1" }).Wait(2000);

            _personRepository = new PersonRepository(_client, "PersonDB1", "PersonColl1");
        }


        // GET: api/Person
        public async Task<IEnumerable<PersonDTO>> Get()
        {
            return await _personRepository.ReadAll();
        }

        // GET: api/Person/5
        public async Task<PersonDTO> Get(string id)
        {
            return await _personRepository.Read(id);
        }

        // POST: api/Person
        public async Task Post([FromBody]PersonDTO value)
        {
            await _personRepository.Create(value);
        }

        // PUT: api/Person/5
        public async Task Put(string id, [FromBody]PersonDTO value)
        {
            if (value != null)
            {
                await _personRepository.Update(id, value);
            }
        }

        // DELETE: api/Person/5
        public async Task Delete(string id)
        {
            if (id != null)
            {
                await _personRepository.Delete(id);
            }
        }
    }
}
