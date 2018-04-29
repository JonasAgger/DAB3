using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;


namespace DABHandin3._3.Models
{
    public class PersonRepository
    {
        private List<Task> _tasks;
        private string dbName;
        private string collName;
        private DocumentClient documentClient;

        public PersonRepository(DocumentClient documentClient, string dbName, string collName)
        {
            _tasks = new List<Task>();
            this.dbName = dbName;
            this.collName = collName;
            this.documentClient = documentClient;
        }

        public async Task Create(PersonDTO t)
        {
            
                try
                {
                    await this.documentClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(dbName, collName, t.Id));
                }
                catch (DocumentClientException de)
                {
                    if (de.StatusCode == HttpStatusCode.NotFound)
                    {
                        await this.documentClient.CreateDocumentAsync(
                            UriFactory.CreateDocumentCollectionUri(dbName, collName), t);
                        Console.WriteLine($"Created the person {t.Id}");
                    }
                    else
                    {
                        throw;
                    }
                }
            
        }

        public async Task<IEnumerable<PersonDTO>> ReadAll()
        {
            IQueryable<Person> queryPersons =
                this.documentClient.CreateDocumentQuery<Person>(
                    UriFactory.CreateDocumentCollectionUri(dbName, collName), "select * from Person");

            List<PersonDTO> persons = new List<PersonDTO>();

            foreach (Person person in queryPersons)
            {
                Console.WriteLine($"Persons found: {person.FirstName} {person.LastName}");

                persons.Add(new PersonDTO(person));
            }

            return persons;
        }

        public async Task<PersonDTO> Read(string documentName)
        {
            return await documentClient.ReadDocumentAsync<PersonDTO>(
                UriFactory.CreateDocumentUri(dbName, collName, documentName));
        }

        public async Task Update(string documentName, PersonDTO t)
        {
            
            
                await this.documentClient.ReplaceDocumentAsync(
                    UriFactory.CreateDocumentUri(dbName, collName, documentName), t);
                Console.WriteLine($"Replaced the person {documentName}");
            
        }

        public async Task Delete(string documentName)
        {
            
                await this.documentClient.DeleteDocumentAsync(
                    UriFactory.CreateDocumentUri(dbName, collName, documentName));
                Console.WriteLine($"Delete the person {documentName}");
            
        }

        public async Task PrintAll()
        {
            IQueryable<PersonDTO> queryPersons =
                this.documentClient.CreateDocumentQuery<PersonDTO>(
                    UriFactory.CreateDocumentCollectionUri(dbName, collName), "select * from Person");

            foreach (PersonDTO person in queryPersons)
            {
                Console.WriteLine($"Persons found: {person.FirstName} {person.LastName}");

                foreach (var address in person.Addresses)
                {
                    Console.WriteLine($"City:{address.CityDTO.CityName}, Zipcode: {address.CityDTO.ZipCode}");
                    Console.WriteLine($"StreetName: {address.StreetName}");
                }
                foreach (var email in person.Emails)
                {
                    Console.WriteLine($"Emails: {email.MailAddress}");
                }

                foreach (var number in person.PhoneNumbers)
                {
                    Console.WriteLine($"Numbers: {number.Number}, Provider: {number.Company}, Phonetype: {number.Type}");
                }

                Console.WriteLine($"This person is a {person.Type}");
            }
        }
    }
}