﻿using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage = "PersonID can't be blank")]
        public Guid PersonID { get; set; }

        [Required(ErrorMessage = "Person Name Can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email Can't be blank")]
        [EmailAddress(ErrorMessage = "Email Should be valid")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        public Person ToPerson()
        {
            return new Person
            {
                PersonID = PersonID,
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
