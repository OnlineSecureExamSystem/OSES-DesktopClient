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
        private static readonly string emailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        // extension method
        public static bool isValid(this string value, DataTypes dataType)
        {
            switch (dataType)
            {
                case DataTypes.Email:
                    return Regex.IsMatch(value, emailRegex) ? true : throw new DataValidationException("Invalide email address");
                case DataTypes.Password:
                    return value.Length > 8 ? true : throw new DataValidationException("The password is too short");
                case DataTypes.PhoneNumber:
                    return value.Length > 8 ? true : throw new DataValidationException("Invalide phone number");
                default:
                    return false;
            }
        }
    }
}
