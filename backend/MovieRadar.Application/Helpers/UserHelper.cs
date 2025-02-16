using MovieRadar.Domain.Entities;
using System.Text.RegularExpressions;

namespace MovieRadar.Application.Helpers
{
    public class UserHelper
    {
        static public (bool, string) IsUserValid(User user)
        {
            if (user == null)
                return (false, "The user is null!");

            var invalidFields = new List<string>();

            var titleValidation = CheckEmail(user.Email);
            if (!titleValidation.Item1)
                invalidFields.Add(titleValidation.Item2);

            var passwordValidation = CheckPassword(user.Password);
            if (!passwordValidation.Item1)
                invalidFields.Add(passwordValidation.Item2);

            var firstNameValidation = CheckName(user.FirstName);
            if (!firstNameValidation.Item1)
                invalidFields.Add(firstNameValidation.Item2);

            var lastNameValidation = CheckLastName(user.LastName);
            if (!lastNameValidation.Item1)
                invalidFields.Add(lastNameValidation.Item2);

            return invalidFields.Count() > 0 ? (false, string.Join("\n", invalidFields)) : (true, "User is valid");
        }

        static public (bool, string) CheckEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^(?!.*\.\.)[a-zA-Z0-9]+@[a-zA-Z0-9]{2,}\.[a-zA-Z]{2,}$"))
                return (false, "The email is invalid!");

            return (true, "The email is valid");
        }

        static public (bool, string) CheckPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8 || !Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).+$")) 
                return (false, "The password is invalid!");

            return (true, "The password is valid");
        }

        static public (bool, string) CheckName(string name)
        {
            if (string.IsNullOrEmpty(name) || !Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                return (false, "The first name is invalid!");

            return (true, "The first name is valid");
        }

        static public (bool, string) CheckLastName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return (true, "The last name is valid");
            else if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                return (false, "The last name is invalid!");

            return (true, "The last name is valid");
        }
    }
}
