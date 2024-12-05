using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagement.Data
{
    public class DapperDbContext
    {
        private readonly IConfiguration _configuration;

        public DapperDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            
            return new SqlConnection(_configuration.GetConnectionString("EmployeeDB"));
        }
    }
}
