using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class EmployeeController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;

  
    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    
    public IActionResult Signup()
    {
        return View();
    }

    
    [HttpPost]
    public async Task<IActionResult> Signup(Employee employee)
    {
        if (ModelState.IsValid)
        {
            
            var existingEmployee = await _employeeRepository.GetEmployeeByCredentialsAsync(employee.EmployeeName, employee.Password);

            if (existingEmployee != null)
            {
                
                ModelState.AddModelError("", "An employee with this name already exists. Please choose a different name.");
                return View();
            }

            
            var employeeId = await _employeeRepository.AddEmployeeAsync(employee);

            
            return RedirectToAction("Login");
        }

        return View();
    }

    
    public IActionResult Login()
    {
        return View();
    }

    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        
        var employee = await _employeeRepository.GetEmployeeByCredentialsAsync(model.EmployeeName, model.Password);

        if (employee == null)
        {
          
            ModelState.AddModelError("", "Invalid employee name or password. Please try again.");
            return View(model);
        }

        
        HttpContext.Session.SetString("EmployeeName", employee.EmployeeName);
        HttpContext.Session.SetInt32("EmployeeId", employee.EmployeeId);

       
        return RedirectToAction("Dashboard");
    }

    
    public async Task<IActionResult> Dashboard()
    {
        var employeeName = HttpContext.Session.GetString("EmployeeName");
        if (string.IsNullOrEmpty(employeeName))
        {
            return RedirectToAction("Login"); 
        }

        var employeeId = HttpContext.Session.GetInt32("EmployeeId");
        var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId.Value);

        return View(employee);
    }

    
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); 
        return RedirectToAction("Login"); 
    }
}
