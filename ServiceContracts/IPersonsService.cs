using System;
using System.Collections.Generic;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    public interface IPersonsService
    {
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        List<PersonResponse> GetAllPersons();

        PersonResponse GetPersonByPersonID(Guid? PersonID);

        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);

        List<PersonResponse> GetSortedPersons(List <PersonResponse>
            allPersons, string sortBy, SortOrderOptions sortOrder);
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);
        bool DeletePerson(Guid? personID);
    }
}
