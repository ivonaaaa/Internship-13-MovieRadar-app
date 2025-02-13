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
            else if (!isEmailValid(user.Email))
                return (false, "The email is invalid!");
            else if (!isPasswordValid(user.Password))
                return (false, "The password is invalid!");
            else if (!isNameValid(user.FirstName))
                return (false, "The first name is invalid!");
            else if (!isLastNameValid(user.LastName))
                return (false, "The last name is invalid!");

            return (true, "User is valid");
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
