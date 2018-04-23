using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace DABHandin2SQL
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Program program = new Program();
                program.GetStartedDemo().Wait();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine($"{de.StatusCode} error occurred: {de.Message}, Message: {baseException.Message}");
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine($"Error: {e.Message}, Message: {baseException.Message}");
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit...");
                Console.ReadKey();
            }
        }

        private const string EndpointUrl = "https://localhost:8081";

        private const string PrimaryKey =
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        private DocumentClient client;

        // Connect to the Azure Cosmos DB Emulator running locally
        private async Task GetStartedDemo()
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);

            // Create DB
            await this.client.CreateDatabaseIfNotExistsAsync(new Database { Id = "F184DABH2Gr9DB" });
            // Create Collection
            await this.client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri("F184DABH2Gr9DB"),
                new DocumentCollection { Id = "F184DABH2Gr9Collection" });

            var ouw = new UnitOfWork(this.client, "F184DABH2Gr9DB", "F184DABH2Gr9Collection");

            Person soren = new Person()
            {
                Id = "soren.123",
                FirstName = "Søren",
                MiddleName = "T.",
                LastName = "Skieller",

                Addresses = new Address[]
                {
                    new Address()
                    {
                        AddressType = "House",
                        HouseNumber = "23",
                        StreetName = "bobbys rd",
                        City = new City()
                        {
                            CityName = "London",
                            ZipCode = "70504"
                        }
                    }
                },
                Emails = new Email[]
                {
                    new Email() {MailAddress = "bob@bobby.com"}
                },
                PhoneNumbers = new PhoneNumber[]
                {
                    new PhoneNumber() {Company = "Oister", Number = "12345678", Type = PhoneType.Home}
                }
            };
            await ouw.Repository.Create(soren);
            ouw.CommitChanges();

            Person p = ouw.Repository.Read("soren.123").Result;
            
            Console.WriteLine(p.Id);
            Console.WriteLine(p.Addresses[0].StreetName);

            soren.Addresses = new Address[]
            {
                new Address()
                {
                    AddressType = "House",
                    HouseNumber = "23",
                    StreetName = "bobbys rd",
                    City = new City()
                    {
                        CityName = "London",
                        ZipCode = "70504"
                    }
                },
                new Address()
                {
                    AddressType = "Warehouse",
                    City = new City()
                    {
                        CityName = "New Orleans",
                        ZipCode = "8000"
                    },
                    HouseNumber = "007",
                    StreetName = "Bond Avenue"
                }
            };
            await ouw.Repository.Update("soren.123", soren);
            ouw.CommitChanges();
            
            Console.WriteLine(ouw.Repository.Read("soren.123").Result.Addresses[1].City.CityName);

            soren.Emails[0].MailAddress = "wotmail@wot.com";
            await ouw.Repository.Update("soren.123", soren);
            ouw.CommitChanges();
            
            await ouw.Repository.PrintAll();
            await ouw.Repository.Delete("soren.123");
            ouw.CommitChanges();
        }

        #region PersonRepository
        public interface IDDRepository<T>
        {
            Task Create(T t);
            Task<Person> Read(string documentName);
            Task Update(string documentName, T t);
            Task Delete(string documentName);
            Task PrintAll();
        }
        public class UnitOfWork
        {
            private PersonRepository _personRepository;

            public PersonRepository Repository => _personRepository;

            public UnitOfWork(DocumentClient client, string dbname, string collname)
            {
                _personRepository = new PersonRepository(client,dbname,collname);
            }

            public void CommitChanges()
            {
                _personRepository.ExecuteTasks();
            }
        }
        public class PersonRepository : IDDRepository<Person>
        {
            private List<Task> _tasks;
            private string dbName;
            private string collName;
            private DocumentClient documentClient;

            public void ExecuteTasks()
            {
                foreach (var task in _tasks.ToArray())
                {
                    Task.WaitAny(task);
                    _tasks.Remove(task);
                }
            }

            public PersonRepository(DocumentClient documentClient, string dbName, string collName)
            {
                _tasks = new List<Task>();
                this.dbName = dbName;
                this.collName = collName;
                this.documentClient = documentClient;
            }

            public async Task Create(Person t)
            {
                _tasks.Add(Task.Run(async () =>
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
                }));
            }

            public async Task<Person> Read(string documentName)
            {
                return await documentClient.ReadDocumentAsync<Person>(
                    UriFactory.CreateDocumentUri(dbName, collName, documentName));
            }

            public async Task Update(string documentName, Person t)
            {
                _tasks.Add(Task.Run(async () =>
                {
                    await this.documentClient.ReplaceDocumentAsync(
                        UriFactory.CreateDocumentUri(dbName, collName, documentName), t);
                    Console.WriteLine($"Replaced the person {documentName}");
                }));
            }

            public async Task Delete(string documentName)
            {
                _tasks.Add(Task.Run(async () =>
                {
                    await this.documentClient.DeleteDocumentAsync(
                        UriFactory.CreateDocumentUri(dbName, collName, documentName));
                    Console.WriteLine($"Delete the person {documentName}");
                }));
            }

            public async Task PrintAll()
            {
                IQueryable<Person> queryPersons =
                    this.documentClient.CreateDocumentQuery<Person>(
                        UriFactory.CreateDocumentCollectionUri(dbName, collName), "select * from Person");

                foreach (Person person in queryPersons)
                {
                    Console.WriteLine($"Persons found: {person.FirstName} {person.LastName}");

                    foreach (var address in person.Addresses)
                    {
                        Console.WriteLine($"City:{address.City.CityName}, Zipcode: {address.City.ZipCode}");
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
        #endregion

        #region Person
        public enum Type
        {
            Student,
            Worker
        };
        public class Person : Entity
        {
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public Type Type { get; set; } = Type.Worker;
            public virtual Email[] Emails { get; set; } = new Email[] { };
            public virtual PhoneNumber[] PhoneNumbers { get; set; } = new PhoneNumber[] { };
            public virtual Address[] Addresses { get; set; } = new Address[] { };
        }
        #endregion

        #region Email
        public class Email : Entity
        {
            //public int Id { get; set; }
            public string MailAddress { get; set; }
            public virtual Person[] Persons { get; set; } = new Person[] { };
        }
        #endregion

        #region PhoneNumber
        public enum PhoneType
        {
            Work,
            Home,
            Mobile
        }
        public class PhoneNumber : Entity
        {
            //public int Id { get; set; }
            public string Number { get; set; }
            public PhoneType Type { get; set; } = PhoneType.Mobile;
            public string Company { get; set; }
            //public virtual Person[] Persons { get; set; } = new Person[] { };
        }
        #endregion
        
        #region Address
        public class Address : Entity
        {
            //public int Id { get; set; }
            public string AddressType { get; set; }
            public string StreetName { get; set; }
            public string HouseNumber { get; set; }
            public virtual City City { get; set; }
            //public virtual Person[] Persons { get; set; }
        }
        public class City : Entity
        {
            //public int Id { get; set; }
            public string CityName { get; set; }
            public string ZipCode { get; set; }
        }
        #endregion
    }
}
