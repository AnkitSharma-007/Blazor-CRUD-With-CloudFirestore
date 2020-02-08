using BlazorWithFirestore.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWithFirestore.Server.Interface
{
    public interface IEmployee
    {
        public Task<List<Employee>> GetAllEmployees();

        public void AddEmployee(Employee employee);
        public void UpdateEmployee(Employee employee);

        public Task<Employee> GetEmployeeData(string id);
        public void DeleteEmployee(string id);
        public Task<List<Cities>> GetCityData();
    }
}
