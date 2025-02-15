using MovieRadar.Application.Helpers;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                return await userRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all users: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<User?> GetById(int id)
        {
            try
            {
                return await userRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by id: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<int> Add(User user)
        {
            var updateUserValidation = UserHelper.IsUserValid(user);
            if (!updateUserValidation.Item1)
                throw new ArgumentException(updateUserValidation.Item2);

            try
            {
                return await userRepository.Add(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new user: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<bool> Update(User user)
        {
            var updateUserValidation = UserHelper.IsUserValid(user);
            if (!updateUserValidation.Item1)
                throw new ArgumentException(updateUserValidation.Item2);

            try
            {
                return await userRepository.Update(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                return await userRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting user: {ex.Message}, inner: {ex.InnerException}");
            }
        }

        public async Task<User?> GetByEmail(string email)
        {
            try
            {
                return await userRepository.GetByEmail(email);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by email: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}
