using BlazorWithFirestore.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorWithFirestore.Client.Pages
{
    public class AddEditEmployeeModel : ComponentBase
    {
        [Inject]
        protected HttpClient Http { get; set; }
        [Inject]
        public NavigationManager UrlNavigationManager { get; set; }
        [Parameter]
        public string empID { get; set; }

        protected string Title = "Add";
        public Employee emp = new Employee();
        protected List<Cities> cityList = new List<Cities>();

        protected override async Task OnInitializedAsync()
        {
            await GetCityList();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrEmpty(empID))
            {
                Title = "Edit";
                emp = await Http.GetJsonAsync<Employee>("/api/Employee/" + empID);
            }
        }

        protected async Task GetCityList()
        {
            cityList = await Http.GetJsonAsync<List<Cities>>("api/Employee/GetCities");
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
            Cancel();
        }

        public void Cancel()
        {
            UrlNavigationManager.NavigateTo("/employeerecords");
        }
    }
}
