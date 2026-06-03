using System;
using System.Text.RegularExpressions;

namespace Component1.InformationSystem.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidLicensePlate(string value) =>
            !string.IsNullOrWhiteSpace(value) && value.Length <= 10;

        public static bool IsLettersOnly(string value) =>
            !string.IsNullOrWhiteSpace(value) && Regex.IsMatch(value, @"^[a-zA-Z\s]+$");

        public static bool IsValidYear(int year) =>
            year >= 1900 && year <= DateTime.Now.Year;

        public static bool IsPositive(double value) => value > 0;

        public static bool IsNotFutureDate(DateTime date) => date <= DateTime.Now;
    }
}
