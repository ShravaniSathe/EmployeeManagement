using Dapper;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using System.Threading.Tasks;

namespace EmployeeManagement.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DapperDbContext _dbContext;

        public EmployeeRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Add a new employee to the database
        public async Task<int> AddEmployeeAsync(Employee employee)
        {
            const string sql = @"
                INSERT INTO Employees (EmployeeName, CompanyName, DOB, Password)
                VALUES (@EmployeeName, @CompanyName, @DOB, @Password);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using (var connection = _dbContext.CreateConnection())
            {
                var id = await connection.ExecuteScalarAsync<int>(sql, employee);
                return id;
            }
        }

        // Get employee details by credentials
        public async Task<Employee> GetEmployeeByCredentialsAsync(string employeeName, string password)
        {
            const string sql = @"
                SELECT * FROM Employees
                WHERE EmployeeName = @EmployeeName AND Password = @Password";

            using (var connection = _dbContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Employee>(sql, new { EmployeeName = employeeName, Password = password });
            }
        }

        // Get employee details by ID
        public async Task<Employee> GetEmployeeByIdAsync(int employeeId)
        {
            const string sql = @"
                SELECT * FROM Employees
                WHERE EmployeeId = @EmployeeId";

            using (var connection = _dbContext.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Employee>(sql, new { EmployeeId = employeeId });
            }
        }
    }
}
