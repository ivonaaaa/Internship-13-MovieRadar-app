using Npgsql;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace MovieRadar.Infrastructure
{
    public class Dapper
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public Dapper(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration?.GetConnectionString("DefaultConnection") ?? throw new Exception("Error while getting connection string");
        }
        public IDbConnection ConnectToDb()
        {
            try
            {
                return new NpgsqlConnection(connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error connecting to database: {ex}, inner: {ex.InnerException}");
            }
        }
    }
}
