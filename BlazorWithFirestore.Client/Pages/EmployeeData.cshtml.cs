using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorWithFirestore.Shared.Models;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;

namespace BlazorWithFirestore.Client.Pages
{
    public class EmployeeDataModel : BlazorComponent
    {
        [Inject]
        protected HttpClient Http { get; set; }
        protected List<Employee> empList = new List<Employee>();
        protected List<Cities> cityList = new List<Cities>();
        protected Employee emp = new Employee();
        protected string modalTitle { get; set; }
        protected string searchString { get; set; }

        protected override async Task OnInitAsync()
        {
            await GetCityList();
            await GetEmployeeList();
        }
        protected async Task GetCityList()
        {
            cityList = await Http.GetJsonAsync<List<Cities>>("api/Employee/GetCities");
        }
        protected async Task GetEmployeeList()
        {
            empList = await Http.GetJsonAsync<List<Employee>>("api/Employee");
        }
        protected void AddEmployee()
        {
            emp = new Employee();
            modalTitle = "Add Employee";
        }
        protected async Task EditEmployee(string empID)
        {
            emp = await Http.GetJsonAsync<Employee>("/api/Employee/" + empID);
            modalTitle = "Edit Employee";
        }
        protected async Task SaveEmployee()
        {
            if (emp.EmployeeId != null)
            {
                await Http.SendJsonAsync(HttpMethod.Put, "api/Employee/", emp);
            }
            else
            {
                await Http.SendJsonAsync(HttpMethod.Post, "/api/Employee/", emp);
            }
            await GetEmployeeList();
        }
        protected async Task DeleteConfirm(string empID)
        {
            emp = await Http.GetJsonAsync<Employee>("/api/Employee/" + empID);
        }
        protected async Task DeleteEmployee(string empID)
        {
            Console.WriteLine(empID);
            await Http.DeleteAsync("api/Employee/" + empID);
            await GetEmployeeList();
        }
        protected async Task SearchEmployee()
        {
            await GetEmployeeList();
            if (searchString != "")
            {
                empList = empList.Where(
                x => x.EmployeeName.IndexOf(searchString,
                StringComparison.OrdinalIgnoreCase) != -1).ToList();
            }
        }
    }
}