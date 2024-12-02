using System;
using System.Collections.Generic;
using ServiceContracts;
using Entities;
using Xunit;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;


        //Constructor
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(false);
        }

        #region AddCountry
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Asset
            Assert.Throws<ArgumentNullException>
           (() => {
               //Act
               _countriesService.AddCountry(request);
           });
        }

        [Fact]
        public void AddCountry_NullCountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            //Asset
            Assert.Throws<ArgumentException>
           (() => {
               //Act
               _countriesService.AddCountry(request);
           });
        }

        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            //Asset
            Assert.Throws<ArgumentException>
           (() =>
           {
               //Act
               _countriesService.AddCountry(request1);
               _countriesService.AddCountry(request2);
           });
        }

        [Fact]
        
            public void AddCountry_ProperCountryDetails()
            {
                //Arrange
                CountryAddRequest? request = new CountryAddRequest()
                {
                    CountryName = "Japan"
                };

                //Act
                  CountryResponse response = _countriesService.AddCountry(request);

               List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();
                
            //Assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountries);
            }
        #endregion

        #region GetAllCountries

        [Fact]
        //The List of Country Should be Empty by Deafult
        public void GetAllCountries_EmptyList()
        {
           List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();

            Assert.Empty(actual_country_response_list);
        }
        [Fact]
        public void GetAllCounties_AddFewCountries()
        {
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest {CountryName = "USA"},
                new CountryAddRequest {CountryName = "UK"}
            };

            List<CountryResponse> country_list_from_add_country = new List<CountryResponse>();   

            foreach (CountryAddRequest country_request in country_request_list)
            {
                country_list_from_add_country.Add(_countriesService.AddCountry(country_request));
            }

            List <CountryResponse> acttualCountryReponseList = _countriesService.GetAllCountries();

            foreach (CountryResponse expected_country in country_list_from_add_country)
            {
                Assert.Contains(expected_country, acttualCountryReponseList);
            }
        }
        #endregion

        #region GetCountryByCountryID

        [Fact]

        public void GetCountryByCountryID_NullCountryID()
        {
            Guid? CountryID = null;
           CountryResponse? country_response_from_method = 
                _countriesService.GetCountryByID(CountryID);
            Assert.Null(country_response_from_method);
        }

        [Fact]
        public void GetCountryByCountryID_ValidCountryID()
        {
            CountryAddRequest? country_add_request = 
                new CountryAddRequest() { CountryName = "China"};

            CountryResponse country_resposne_from_add = 
                _countriesService.AddCountry(country_add_request);

           CountryResponse? country_reponse_from_get =
                _countriesService.GetCountryByID(country_resposne_from_add.CountryID);

            Assert.Equal(country_resposne_from_add, country_reponse_from_get);
        }

        #endregion
    }
}
