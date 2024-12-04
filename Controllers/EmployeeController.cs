using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class EmployeeController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;

    // Constructor injection for IEmployeeRepository
    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    // Signup Action - Displays the signup form
    public IActionResult Signup()
    {
        return View();
    }

    // POST Action - Handles the signup form submission
    [HttpPost]
    public async Task<IActionResult> Signup(Employee employee)
    {
        if (ModelState.IsValid)
        {
            // Check if the employee already exists by EmployeeName
            var existingEmployee = await _employeeRepository.GetEmployeeByCredentialsAsync(employee.EmployeeName, employee.Password);

            if (existingEmployee != null)
            {
                // Employee already exists, show an error message
                ModelState.AddModelError("", "An employee with this name already exists. Please choose a different name.");
                return View();
            }

            // Create a new employee in the database
            var employeeId = await _employeeRepository.AddEmployeeAsync(employee);

            // Redirect to the login page after successful signup
            return RedirectToAction("Login");
        }

        return View();
    }

    // Login Action - Displays the login form
    public IActionResult Login()
    {
        return View();
    }

    // POST Action - Handles the login form submission
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // Authenticate employee by EmployeeName and Password
        var employee = await _employeeRepository.GetEmployeeByCredentialsAsync(model.EmployeeName, model.Password);

        if (employee == null)
        {
            // If authentication fails, show error message
            ModelState.AddModelError("", "Invalid employee name or password. Please try again.");
            return View(model);
        }

        // Store employee data in session or cookie for future requests
        HttpContext.Session.SetString("EmployeeName", employee.EmployeeName);
        HttpContext.Session.SetInt32("EmployeeId", employee.EmployeeId);

        // Redirect to dashboard or employee-specific page
        return RedirectToAction("Dashboard");
    }

    // Dashboard Action - Displays the logged-in employee's data
    public async Task<IActionResult> Dashboard()
    {
        var employeeName = HttpContext.Session.GetString("EmployeeName");
        if (string.IsNullOrEmpty(employeeName))
        {
            return RedirectToAction("Login"); // Redirect to login if session is not found
        }

        var employeeId = HttpContext.Session.GetInt32("EmployeeId");
        var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId.Value);

        return View(employee);
    }

    // Logout Action - Clears the session data
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear session data
        return RedirectToAction("Login"); // Redirect to login page
    }
}
