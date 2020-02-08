using BlazorWithFirestore.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorWithFirestore.Client.Pages
{
    public class EmployeeDataModel : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }

        protected List<Employee> empList = new List<Employee>();
        protected Employee emp = new Employee();
        protected string SearchString { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await GetEmployeeList();
        }

        protected async Task GetEmployeeList()
        {
            empList = await Http.GetJsonAsync<List<Employee>>("api/Employee");
        }

        protected void DeleteConfirm(string empID)
        {
            emp = empList.FirstOrDefault(x => x.EmployeeId == empID);
        }

        protected async Task DeleteEmployee(string empID)
        {
            await Http.DeleteAsync("api/Employee/" + empID);
            await GetEmployeeList();
        }
        protected async Task SearchEmployee()
        {
            await GetEmployeeList();
            if (!string.IsNullOrEmpty(SearchString))
            {
                empList = empList.Where(x => x.EmployeeName.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1).ToList();
            }
        }
    }
}
