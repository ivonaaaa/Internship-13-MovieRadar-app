using MovieRadar.Domain.Entities;
using System.Text.RegularExpressions;

namespace MovieRadar.Application.Helpers
{
    public class UserHelper
    {
        static public bool isUserValid(User user)
        {
            if (user == null || !isPasswordValid(user.Password) || !isEmailValid(user.Email) || !isNameValid(user.FirstName) || !isNameValid(user.LastName))
                return false;

            return true;
        }

        static public bool isPasswordValid(string password)
        {
            if (password == null || password.Length < 8 || !Regex.IsMatch(password, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).+$")) 
                return false;

            return true;
        }

        static public bool isEmailValid(string email)
        {
            if (email == null || !Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                return false;

            return true;
        }

        static public bool isNameValid(string name)
        {
            if (name == null || !Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                return false;

            return true;
        }
    }
}
