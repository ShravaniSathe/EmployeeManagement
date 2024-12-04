using EmployeeManagement.Models;
using System.Threading.Tasks;

namespace EmployeeManagement.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<int> AddEmployeeAsync(Employee employee);
        Task<Employee> GetEmployeeByCredentialsAsync(string employeeName, string password);
        Task<Employee> GetEmployeeByIdAsync(int employeeId);
    }
}
