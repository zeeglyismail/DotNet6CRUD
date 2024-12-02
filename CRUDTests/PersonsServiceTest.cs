using ServiceContracts;
using System;
using System.Collections.Generic;
using Xunit;
using Entities;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;
using Xunit.Abstractions;
using System.Linq;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personsService = new PersonsService(false);
            _countriesService = new CountriesService(false);
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson
        
        [Fact]
        public void AddPerson_NullPerson()
        {
            PersonAddRequest? personAddRequest = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.AddPerson(personAddRequest);
            });
        }

        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null};
            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.AddPerson(personAddRequest);
            });
        }

        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person Name ...",
                Email = "zeeglyismail@gmail.com",
                Address = "Sample Adress",
                CountryID = Guid.NewGuid(),
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };
            PersonResponse person_response_from_add  = _personsService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = _personsService.GetAllPersons();
            Assert.True(person_response_from_add.PersonID!=Guid.Empty);
            Assert.Contains(person_response_from_add, persons_list);
        }

        #endregion

        #region GetPersonByPersonID

        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            Guid? personID = null;
           PersonResponse? person_response_from_get = _personsService.GetPersonByPersonID(personID);
            Assert.Null(person_response_from_get);
        }

        [Fact]
        public void GetPersonByPersonID_WithPersonID()
        {
            CountryAddRequest country_request = new CountryAddRequest()
            { CountryName = "Canada" };
            CountryResponse country_response = _countriesService.AddCountry(country_request);
            PersonAddRequest person_request = new PersonAddRequest()
            {
                PersonName = "PersonName...",
                Email = "zeeglyismail@gmail.com",
                Address = "Address",
                CountryID = country_response.CountryID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };
              PersonResponse person_response_from_add = 
                _personsService.AddPerson(person_request);
              PersonResponse? person_response_from_get = 
                _personsService.GetPersonByPersonID(person_response_from_add.PersonID);
            Assert.Equal(person_response_from_add, person_response_from_get);
        }

        #endregion

        #region GetAllPersons

        [Fact]
        public void GetAllPersons_EmptyList()
        {
            List<PersonResponse> persons_from_get =
                _personsService.GetAllPersons();
            Assert.Empty(persons_from_get);
        }
        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            CountryAddRequest country_request_1 = new CountryAddRequest()
            { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest()
            { CountryName = "India" };
            CountryResponse country_response_1 = 
                _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = 
                _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {   PersonName = "Smith",
                Email = "zeeglyismail@gmail.com",
                Gender = GenderOptions.Male,
                Address = "SampleAdress",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "zmaryismail@gmail.com",
                Gender = GenderOptions.Male,
                Address = "SampleAdress",
                CountryID = country_response_2.CountryID,
                DateOfBirth = DateTime.Parse("2002-07-06"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Smiath",
                Email = "zeeglyismaail@gmail.com",
                Gender = GenderOptions.Male,
                Address = "SampleAdaress",
                CountryID = country_response_2.CountryID,
                DateOfBirth = DateTime.Parse("2002-06-06"),
                ReceiveNewsLetters = true
            };
            List<PersonAddRequest> person_request = new List<PersonAddRequest>() 
            { person_request_1,person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new
                List<PersonResponse>();

            foreach (PersonAddRequest person_requests in person_request)
            {
                PersonResponse person_response = 
                    _personsService.AddPerson(person_requests);
                person_response_list_from_add.Add(person_response);
            }

            _testOutputHelper.WriteLine("Expected");

            foreach(PersonResponse person_response_from_add in  
                person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            List <PersonResponse> persons_list_from_get = 
                _personsService.GetAllPersons();

            _testOutputHelper.WriteLine("Actual");

            foreach (PersonResponse person_response_from_get in
                persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            foreach (PersonResponse person_response_from_add in 
                person_response_list_from_add) 
            {
                Assert.Contains(person_response_from_add, persons_list_from_get);
            }
        }
        #endregion

        #region GetFilteredPersons

        [Fact]
        public void GetFilteredPersons_EmptySerachText()
        {
            CountryAddRequest country_request_1 = new CountryAddRequest()
            { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest()
            { CountryName = "India" };
            CountryResponse country_response_1 =
                _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 =
                _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "zeeglyismail@gmail.com",
                Gender = GenderOptions.Male,
                Address = "SampleAdress",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "zmaryismail@gmail.com",
                Gender = GenderOptions.Male,
                Address = "SampleAdress",
                CountryID = country_response_2.CountryID,
                DateOfBirth = DateTime.Parse("2002-07-06"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Smiath",
                Email = "zeeglyismaail@gmail.com",
                Gender = GenderOptions.Male,
                Address = "SampleAdaress",
                CountryID = country_response_2.CountryID,
                DateOfBirth = DateTime.Parse("2002-06-06"),
                ReceiveNewsLetters = true
            };
            List<PersonAddRequest> person_request = new List<PersonAddRequest>()
            { person_request_1,person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new
                List<PersonResponse>();

            foreach (PersonAddRequest person_requests in person_request)
            {
                PersonResponse person_response =
                    _personsService.AddPerson(person_requests);
                person_response_list_from_add.Add(person_response);
            }

            _testOutputHelper.WriteLine("Expected");

            foreach (PersonResponse person_response_from_add in
                person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            List<PersonResponse> persons_list_from_search =
                _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Actual");

            foreach (PersonResponse person_response_from_get in
                persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            foreach (PersonResponse person_response_from_add in
                person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, persons_list_from_search);
            }
        }

        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            CountryAddRequest country_request_1 = new CountryAddRequest()
            { CountryName = "USA" };
            CountryAddRequest country_request_2 = new CountryAddRequest()
            { CountryName = "India" };
            CountryResponse country_response_1 =
                _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 =
                _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "zeeglyismail@gmail.com",
                Gender = GenderOptions.Male,
                Address = "SampleAdress",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("2002-05-06"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "zmaryismail@gmail.com",
                Gender = GenderOptions.Male,
                Address = "SampleAdress",
                CountryID = country_response_2.CountryID,
                DateOfBirth = DateTime.Parse("2002-07-06"),
                ReceiveNewsLetters = true
            };
            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Rahman",
                Email = "zeeglyismaail@gmail.com",
                Gender = GenderOptions.Male,
                Address = "SampleAdaress",
                CountryID = country_response_2.CountryID,
                DateOfBirth = DateTime.Parse("2002-06-06"),
                ReceiveNewsLetters = true
            };
            List<PersonAddRequest> person_request = new List<PersonAddRequest>()
            { person_request_1,person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new
                List<PersonResponse>();

            foreach (PersonAddRequest person_requests in person_request)
            {
                PersonResponse person_response =
                    _personsService.AddPerson(person_requests);
                person_response_list_from_add.Add(person_response);
            }

            _testOutputHelper.WriteLine("Expected");

            foreach (PersonResponse person_response_from_add in
                person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            List<PersonResponse> persons_list_from_search =
                _personsService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            _testOutputHelper.WriteLine("Actual");

            foreach (PersonResponse person_response_from_get in
                persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_response_from_get.ToString());
            }

            foreach (PersonResponse person_response_from_add in
                person_response_list_from_add)
            {
                if(person_response_from_add.PersonName!= null)
                {
                    if (person_response_from_add.PersonName.Contains("ma",
                                    StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person_response_from_add, persons_list_from_search);
                    }
                }
               
                
            }
        }


        #endregion

        #region GetSortedPersons

        [Fact]
        public void GetSortedPersons()
        {
                CountryAddRequest country_request_1 = new CountryAddRequest()
                { CountryName = "USA" };
                CountryAddRequest country_request_2 = new CountryAddRequest()
                { CountryName = "India" };
                CountryResponse country_response_1 =
                    _countriesService.AddCountry(country_request_1);
                CountryResponse country_response_2 =
                    _countriesService.AddCountry(country_request_2);

                PersonAddRequest person_request_1 = new PersonAddRequest()
                {
                    PersonName = "Smith",
                    Email = "zeeglyismail@gmail.com",
                    Gender = GenderOptions.Male,
                    Address = "SampleAdress",
                    CountryID = country_response_1.CountryID,
                    DateOfBirth = DateTime.Parse("2002-05-06"),
                    ReceiveNewsLetters = true
                };
                PersonAddRequest person_request_2 = new PersonAddRequest()
                {
                    PersonName = "Mary",
                    Email = "zmaryismail@gmail.com",
                    Gender = GenderOptions.Male,
                    Address = "SampleAdress",
                    CountryID = country_response_2.CountryID,
                    DateOfBirth = DateTime.Parse("2002-07-06"),
                    ReceiveNewsLetters = true
                };
                PersonAddRequest person_request_3 = new PersonAddRequest()
                {
                    PersonName = "Rahman",
                    Email = "zeeglyismaail@gmail.com",
                    Gender = GenderOptions.Male,
                    Address = "SampleAdaress",
                    CountryID = country_response_2.CountryID,
                    DateOfBirth = DateTime.Parse("2002-06-06"),
                    ReceiveNewsLetters = true
                };
                List<PersonAddRequest> person_request = new List<PersonAddRequest>()
            { person_request_1,person_request_2, person_request_3 };

                List<PersonResponse> person_response_list_from_add = new
                    List<PersonResponse>();

                foreach (PersonAddRequest person_requests in person_request)
                {
                    PersonResponse person_response =
                        _personsService.AddPerson(person_requests);
                    person_response_list_from_add.Add(person_response);
                }

                _testOutputHelper.WriteLine("Expected");

                foreach (PersonResponse person_response_from_add in
                    person_response_list_from_add)
                {
                    _testOutputHelper.WriteLine(person_response_from_add.ToString());
                }
                List<PersonResponse> allPersons = 
                    _personsService.GetAllPersons();
                List<PersonResponse> persons_list_from_sort =
                    _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName),
                    SortOrderOptions.DESC);

                _testOutputHelper.WriteLine("Actual");
                foreach (PersonResponse person_response_from_get in
                    persons_list_from_sort)
                {
                    _testOutputHelper.WriteLine(person_response_from_get.ToString());
                }
            person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp =>
            temp.PersonName).ToList();

            for (int i = 0; i< person_response_list_from_add.Count; i++)
            {
                Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
            }
        }

        #endregion

        #region UpdatePerson

        [Fact]
        public void UpdatePerson_NullPerson()
        {
            PersonUpdateRequest? person_update_request = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.UpdatePerson(person_update_request);
            });
            
        }

        [Fact]
        public void UpdatePerson_InvalidPersonID()
        {
            PersonUpdateRequest? person_update_request = new PersonUpdateRequest()
            {
                PersonID = Guid.NewGuid()
            };
            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.UpdatePerson(person_update_request);
            });

        }

        [Fact]
        public void UpdatePerson_PersonNameIsNull()
        {
            CountryAddRequest country_request = new CountryAddRequest()
            { CountryName = "UK" };
            CountryResponse country_response_from_add =
                _countriesService.AddCountry(country_request);

            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "John", 
                CountryID = country_response_from_add.CountryID,
                Email = "Hello@gmail.com",
                Address = "Address...",
                Gender = GenderOptions.Male
            };
            PersonResponse person_response_from_add = 
                _personsService.AddPerson(person_add_request);

            PersonUpdateRequest? person_update_request = 
                person_response_from_add.ToPersonUpdateRequest();

            person_update_request.PersonName = null;

            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.UpdatePerson(person_update_request);
            });

        }

        [Fact]
        public void UpdatePerson_PersonFullDetailsUpadte()
        {
            CountryAddRequest country_request = new CountryAddRequest()
            { CountryName = "UK" };
            CountryResponse country_response_from_add =
                _countriesService.AddCountry(country_request);

            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "John",
                CountryID = country_response_from_add.CountryID,
                Address = "Sample Address",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Email= "abc@gmail.com",
                Gender= GenderOptions.Male,
                ReceiveNewsLetters= true
            };
            PersonResponse person_response_from_add =
                _personsService.AddPerson(person_add_request);

            PersonUpdateRequest? person_update_request =
                person_response_from_add.ToPersonUpdateRequest();

            person_update_request.PersonName = "William";
            person_update_request.Email = "william@gmail.com";

            PersonResponse person_response_from_update =
                _personsService.UpdatePerson(person_update_request);

            PersonResponse person_response_from_get =
                _personsService.GetPersonByPersonID(person_response_from_update.PersonID);

            Assert.Equal(person_response_from_get, person_response_from_update);

        }
        #endregion

        #region DeletePerson

        [Fact]
        public void DeletePerson_validPersonID()
        {
            CountryAddRequest country_request = new CountryAddRequest()
            { CountryName = "USA" };
            CountryResponse country_response_from_add = 
                _countriesService.AddCountry(country_request);
            PersonAddRequest person_add_request = new PersonAddRequest()
            {
                PersonName = "Shoheb",
                Address = "Address",
                CountryID = country_response_from_add.CountryID,
                DateOfBirth = Convert.ToDateTime("2010-01-01"),
                Email = "shohebchodu@gmail.com",
                Gender = GenderOptions.Female,
                ReceiveNewsLetters = true,
            };
            PersonResponse person_response_from_add =
                _personsService.AddPerson(person_add_request);

           bool isDeleted = 
                _personsService.DeletePerson(person_response_from_add.PersonID);
           
            Assert.True(isDeleted);
        }

        [Fact]
        public void DeletePerson_InvalidPersonID()
        {
            bool isDeleted =
                 _personsService.DeletePerson(Guid.NewGuid());

            Assert.False(isDeleted);
        }

        #endregion
    }
}
