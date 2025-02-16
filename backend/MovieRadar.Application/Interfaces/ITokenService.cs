namespace MovieRadar.Application.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(int userId, string username, bool isAdmin);
    }
}
