using System;
using System.Collections.Generic;
using ServiceContracts.Enums;
using Entities;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage ="Person Name Can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email Can't be blank")]
        [EmailAddress(ErrorMessage ="Email Should be valid")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please Select a Date")]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "Please Select a Gender")]
        public GenderOptions? Gender { get; set; }
        [Required(ErrorMessage ="Please Select a Country!")]
        public Guid? CountryID { get; set; }

        [Required(ErrorMessage = "Please Enter Address!")]
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        public Person ToPerson()
        {
            return new Person
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryID = CountryID,
                Address = Address,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}
