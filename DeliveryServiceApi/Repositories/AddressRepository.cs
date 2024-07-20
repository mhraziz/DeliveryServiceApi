using System.Data.SqlClient;
using Dapper;
using DeliveryServiceApi.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace DeliveryServiceApi.Repositories
{
    public class AddressRepository
    {
        private readonly string _connectionString;

        public AddressRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("dbcs");
        }

        public async Task<int> AddAddressAsync(Address address)
        {
            var sql = "INSERT INTO Addresses (Street, City, State, PostalCode, Latitude, Longitude) VALUES (@Street, @City, @State, @PostalCode, @Latitude, @Longitude); SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QuerySingleAsync<int>(sql, address);
            }
        }

        public async Task<Address> GetAddressByIdAsync(int id)
        {
            var sql = "SELECT * FROM Addresses WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QuerySingleOrDefaultAsync<Address>(sql, new { Id = id });
            }
        }
    }
}

