using IMDBConsoleApp.Exceptions;
using IMDBConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IMDBConsoleApp.Helpers
{
    public static class PersonValidator
    {
        public static void ValidatePerson(string name, DateTime dob)
        {
            ValidationHelper.ValidateString(name, "Name");
            ValidationHelper.ValidateDate(dob, "Birthdate");

            if (!name.Any(char.IsLetter))
                throw new ValidationException("Name must contain Letters.");

            if (dob > DateTime.Today)
                throw new ValidationException("Date of Birth cannot be in Future.");
        }
    }
}
