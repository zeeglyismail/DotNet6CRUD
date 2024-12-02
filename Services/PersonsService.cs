using System;
using System.Collections.Generic;
using ServiceContracts.DTO;
using ServiceContracts;
using System.Security.Cryptography.X509Certificates;
using Entities;
using System.ComponentModel.DataAnnotations;
using Services.Helpers;
using ServiceContracts.Enums;
using System.Reflection;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;
        public PersonsService(bool intialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
            if (intialize )
            {
                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("90F360C6-C969-4F3F-9472-D21C71D35B78"),
                    PersonName = "Dewitt",
                    Email = "djohnson0@berkeley.edu",
                    DateOfBirth = DateTime.Parse("20/06/2003"),
                    Gender = "Male",
                    Address = "288 Fulton Place",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("25782A98-5574-4E4C-A932-689F9C4CC0BC")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("C6834D07-E553-4B22-AEE5-75593A21202F"),
                    PersonName = "Anatollo",
                    Email = "aswateridge1@vimeo.com",
                    DateOfBirth = DateTime.Parse("14/03/1997"),
                    Gender = "Male",
                    Address = "90122 Eliot Circle",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("98F519C9-358E-40C1-8DE5-366838832757")
                });

                _persons.Add(new Person()
                {
                    PersonID = Guid.Parse("6B61BEED-023C-40F7-BCEA-C692AC77B576"),
                    PersonName = "Berny",
                    Email = "bgriffen2@tmall.com",
                    DateOfBirth = DateTime.Parse("11/10/2023"),
                    Gender = "Female",
                    Address = "1824 Northport Terrace",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("09314827-7E24-4DAD-929C-A8FD590B7A7B")
                });
            }
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country =
                _countriesService.GetCountryByID(person.CountryID)?.CountryName;
            return personResponse;
        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(PersonAddRequest));
            }

            ValidationHelper.ModelValidation(personAddRequest);

            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();
            _persons.Add(person);
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
           return _persons.Select(temp => ConvertPersonToPersonResponse(temp)).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? PersonID)
        {
            if (PersonID == null) return null;
            Person? person = _persons.FirstOrDefault(temp => temp.PersonID == PersonID);
            if (person == null) return null;

            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allpersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allpersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty
                (searchString)) 
                return matchingPersons;

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allpersons.Where(temp => 
                    (!string.IsNullOrEmpty(temp.PersonName)?
                    temp.PersonName.Contains(searchString, 
                    StringComparison.OrdinalIgnoreCase): true)).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPersons = allpersons.Where(temp =>
                    (string.IsNullOrEmpty(temp.Email) ?
                    temp.Email.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allpersons.Where(temp =>
                    (temp.DateOfBirth!=null) ?
                    temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    matchingPersons = allpersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Gender) ?
                    temp.Gender.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.CountryID):
                    matchingPersons = allpersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Country) ?
                    temp.Country.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    matchingPersons = allpersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Address) ?
                    temp.Address.Contains(searchString,
                    StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                deafult: matchingPersons = allpersons; break;
            }
            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy)) return allPersons;
            List<PersonResponse> sortedPersons = (sortBy, sortOrder)
                switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) =>
                allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) =>
                allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                _ => allPersons
            };
            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) 
                throw new ArgumentNullException(nameof(Person));

            ValidationHelper.ModelValidation(personUpdateRequest);

            Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonID == 
            personUpdateRequest.PersonID);

            if (matchingPerson == null)
            {
                throw new ArgumentException("Given Person ID Doesnt EXIST!");
            }

            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();

            return ConvertPersonToPersonResponse(matchingPerson);
        }

        public bool DeletePerson(Guid? personID)
        {
            if (personID == null)
                throw new ArgumentNullException(nameof(personID));

           Person? person = 
                _persons.FirstOrDefault(temp => temp.PersonID == personID);
            if (person == null) 
                return false;

            _persons.RemoveAll(temp => temp.PersonID == personID);
                return true;
        }
    }
}
