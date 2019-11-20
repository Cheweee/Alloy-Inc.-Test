using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TestApp.Utilities
{
    public class ValidationUtilities
    {
        public static bool NotNullRule(object source)
        {
            return source != null;
        }
        
        public static bool NotEmptyRule(string source)
        {
            return !string.IsNullOrWhiteSpace(source);
        }

        public static bool OnlyLettersNumbersAndUnderscorcesRule(string source)
        {
            return Regex.IsMatch(source, @"[a-zA-Z0-9а-яА-я]");
        }

        public static bool OnlyNumbersRule(string source)
        {
            return Regex.IsMatch(source, @"[0-9,.]");
        }

        public static bool MoreThanValueLengthRule(string source, int value = 0)
        {
            return source.Length >= value;
        }

        public static bool MoreThanValueRule(int source, int value = 0)
        {
            return source > 0;
        }

        public static bool MoreThanValueRule(double source, int value = 0)
        {
            return source > 0;
        }

        public static bool MoreThanValueRule(float source, int value = 0)
        {
            return source > 0;
        }

        public static bool IsPositive(int source)
        {
            return source >= 0;
        }

        public static string ValidateEmail(string source)
        {
            if (!NotEmptyRule(source))
                return "Email should not be empty.";

            try
            {
                // Normalize the domain
                source = Regex.Replace(source, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch
            {
                return "Email is not validated.";
            }

            try
            {
                if (!Regex.IsMatch(source,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
                    return "Email is not validated.";
            }
            catch (RegexMatchTimeoutException)
            {
                return "Email is not validated.";
            }

            return string.Empty;
        }
    }
}