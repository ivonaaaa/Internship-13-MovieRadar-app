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

            switch (true)
            {
                case var _ when !IsEmailValid(user.Email):
                    return (false, "The email is invalid!");

                case var _ when !IsPasswordValid(user.Password):
                    return (false, "The password is invalid!");

                case var _ when !IsNameValid(user.FirstName):
                    return (false, "The first name is invalid!");

                case var _ when !IsLastNameValid(user.LastName):
                    return (false, "The last name is invalid!");

                default:
                    return (true, "User is valid");
            }
        }

        static public bool IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8 || !Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).+$")) 
                return false;

            return true;
        }

        static public bool IsEmailValid(string email)
        {
            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^(?!.*\.\.)[a-zA-Z0-9]+@[a-zA-Z0-9]{2,}\.[a-zA-Z]{2,}$"))
                return false;

            return true;
        }

        static public bool IsNameValid(string name)
        {
            if (string.IsNullOrEmpty(name) || !Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                return false;

            return true;
        }

        static public bool IsLastNameValid(string name)
        {
            if (string.IsNullOrEmpty(name))
                return true;
            else if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                return false;

            return true;
        }
    }
}
