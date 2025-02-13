using MovieRadar.Domain.Entities;
using System.Text.RegularExpressions;

namespace MovieRadar.Application.Helpers
{
    public class UserHelper
    {
        static public (bool, string) isUserValid(User user)
        {
            if (user == null)
                return (false, "The user is null!");

            switch (true)
            {
                case var _ when !isEmailValid(user.Email):
                    return (false, "The email is invalid!");

                case var _ when !isPasswordValid(user.Password):
                    return (false, "The password is invalid!");

                case var _ when !isNameValid(user.FirstName):
                    return (false, "The first name is invalid!");

                case var _ when !isLastNameValid(user.LastName):
                    return (false, "The last name is invalid!");

                default:
                    return (true, "User is valid");
            }
        }

        static public bool isPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8 || !Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).+$")) 
                return false;

            return true;
        }

        static public bool isEmailValid(string email)
        {
            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^(?!.*\.\.)[a-zA-Z0-9]+@[a-zA-Z0-9]{2,}\.[a-zA-Z]{2,}$"))
                return false;

            return true;
        }

        static public bool isNameValid(string name)
        {
            if (string.IsNullOrEmpty(name) || !Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                return false;

            return true;
        }

        static public bool isLastNameValid(string name)
        {
            if (string.IsNullOrEmpty(name))
                return true;
            else if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                return false;

            return true;
        }
    }
}
