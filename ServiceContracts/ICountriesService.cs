using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manupulating
    /// country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Add a country object to list of countries
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns></returns>
        CountryResponse AddCountry (CountryAddRequest?
            countryAddRequest);

        /// <summary>
        /// Returns all countries from the list of country
        /// </summary>
        /// <returns></returns>
        List<CountryResponse> GetAllCountries();

        /// <summary>
        /// Returns the country object based on given ID
        /// </summary>
        /// <param name="CountryID"></param>
        /// <returns></returns>
        CountryResponse? GetCountryByID (Guid? CountryID);
    }
}