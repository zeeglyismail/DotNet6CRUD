using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //Private List
        private readonly List<Country> _countries;

        //constructor
        public CountriesService(bool intialize = true)
        {

            _countries = new List<Country>();
            if (intialize)
            {

                _countries.AddRange(new List<Country>()
                    {
                       new Country(){CountryID = Guid.Parse("25782A98-5574-4E4C-A932-689F9C4CC0BC"),CountryName = "USA"},
                       new Country(){CountryID = Guid.Parse("98F519C9-358E-40C1-8DE5-366838832757"),CountryName = "China"},
                       new Country(){CountryID = Guid.Parse("09314827-7E24-4DAD-929C-A8FD590B7A7B"),CountryName = "Bangladesh"},
                    });
            }
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByID(Guid? CountryID)
        {
            if (CountryID == null) return null;
           Country? country_response_from_list = 
                _countries.FirstOrDefault(temp => temp.CountryID == CountryID);
            if (country_response_from_list == null) return null;
            return country_response_from_list.ToCountryResponse();

        }

        CountryResponse ICountriesService.AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }
            if (_countries.Where(temp => temp.CountryName ==  countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given Country Name Already Exists!");
            }
            //country object from country add request to country type 
           Country country = countryAddRequest.ToCountry();

            //generate new country ID
            country.CountryID = Guid.NewGuid();

            //Add country object into _countries
            _countries.Add(country);

            return country.ToCountryResponse();
        }
    }
}