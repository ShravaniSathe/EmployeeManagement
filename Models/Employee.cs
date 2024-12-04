namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string CompanyName { get; set; }
        public DateTime DOB { get; set; }
        public string Password { get; set; }
    }
}
