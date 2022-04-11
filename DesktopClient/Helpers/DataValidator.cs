using Avalonia.Data;
using DesktopClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DesktopClient.Helpers
{
    public static class DataValidator
    {
        private static readonly string nameRegex = @"^[a-zA-Z]*$";
        private static readonly string numberRegex = @"^[0-9]*$";
        private static readonly string emailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        // extension methods

        
        public static bool IsValid(this string value, DataTypes dataType)
        {
            return dataType switch
            {
                DataTypes.Email => Regex.IsMatch(value, emailRegex) ? true : throw new DataValidationException("Invalide email address"),
                DataTypes.Password => value.Length > 8 ? true : throw new DataValidationException("The password is too short"),
                DataTypes.PhoneNumber => value.Length > 8 ? true : throw new DataValidationException("Invalide phone number"),
                DataTypes.Code => value.Length > 8 ? true : throw new DataValidationException("Invalide exam code"),
                DataTypes.Name => Regex.IsMatch(value, nameRegex) ? true : throw new DataValidationException("Invalide name"),
                DataTypes.RegistrationNumber => Regex.IsMatch(value, numberRegex) ? true : throw new DataValidationException("Invalide registration number"),
                _ => false,
            };
        }
        public static bool IsValid(this double? value, DataTypes dataType)
        {
            return dataType switch
            {
                DataTypes.InternetSpeed => value > 1 ? true : throw new DataValidationException("Internet Speed is too slow"),
                _ => false,
            };
        }

        public static bool IsValid(this DateTimeOffset? value, DataTypes dataType)
        {
            return dataType switch
            {
                DataTypes.InternetSpeed => value?.Year > 1 && value?.Month > 1 && value?.Day > 1 ? true : throw new DataValidationException("Wrong date"),
                _ => false,
            };
        }
    }
}
