using System;
using System.Collections.Generic;
using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Domain Model for new Country Add
    /// </summary>
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }

        public Country ToCountry()
        {
            return new Country() { CountryName = CountryName };
        }
    }
}
