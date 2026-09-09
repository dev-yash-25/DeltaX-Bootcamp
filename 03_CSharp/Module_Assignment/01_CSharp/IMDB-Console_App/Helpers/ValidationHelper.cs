using IMDBConsoleApp.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IMDBConsoleApp.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidateString(string value, string fieldVal)    
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ValidationException($"{fieldVal} is Empty.");
        }

        public static void ValidateDate(DateTime date, string fieldVal)
        {
            if (date == default)
                throw new ValidationException($"Invalid {fieldVal}.");
        }

        public static void ValidatePositiveInt(int value, string fieldVal)
        {
            if (value <= 0)
                throw new ValidationException($"{fieldVal} must be greater than zero.");
        }

        public static void ValidateNull(object value, string fieldVal)
        {
            if (value == null)
                throw new ValidationException($"{fieldVal} was not found.");
        }

        public static void ValidateList<T>(IEnumerable<T> list, string fieldVal)
        {
            if (list == null || !list.Any())
                throw new ValidationException($"{fieldVal} is empty.");
        }
    }
}
