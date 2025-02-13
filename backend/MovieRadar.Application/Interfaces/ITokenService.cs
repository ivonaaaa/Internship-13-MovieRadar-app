namespace MovieRadar.Application.Services
{
    public interface ITokenService
    {
        public string GenerateToken(int userId, string username);
    }
}
