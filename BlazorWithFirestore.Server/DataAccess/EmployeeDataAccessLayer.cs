﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorWithFirestore.Shared.Models;
using Google.Cloud.Firestore;
using Newtonsoft.Json;

namespace BlazorWithFirestore.Server.DataAccess
{
    public class EmployeeDataAccessLayer
    {
        string projectId;
        FirestoreDb fireStoreDb;
        public EmployeeDataAccessLayer()
        {
            string filepath = "C:\\FirestoreAPIKey\\blazorwithfirestore-6d0a096b0174.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", filepath);
            projectId = "blazorwithfirestore";
            fireStoreDb = FirestoreDb.Create(projectId);
        }
        public async Task<List<Employee>> GetAllEmployees()
        {
            try
            {
                Query employeeQuery = fireStoreDb.Collection("employees");
                QuerySnapshot employeeQuerySnapshot = await employeeQuery.GetSnapshotAsync();
                List<Employee> lstEmployee = new List<Employee>();

                foreach (DocumentSnapshot documentSnapshot in employeeQuerySnapshot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        Dictionary<string, object> city = documentSnapshot.ToDictionary();
                        string json = JsonConvert.SerializeObject(city);
                        Employee newuser = JsonConvert.DeserializeObject<Employee>(json);
                        newuser.EmployeeId = documentSnapshot.Id;
                        newuser.date = documentSnapshot.CreateTime.Value.ToDateTime();
                        lstEmployee.Add(newuser);
                    }
                }

                List<Employee> sortedEmployeeList = lstEmployee.OrderBy(x => x.date).ToList();
                return sortedEmployeeList;
            }
            catch
            {
                throw;
            }
        }
        public async void AddEmployee(Employee employee)
        {
            try
            {
                CollectionReference colRef = fireStoreDb.Collection("employees");
                await colRef.AddAsync(employee);
            }
            catch
            {
                throw;
            }
        }
        public async void UpdateEmployee(Employee employee)
        {
            try
            {
                DocumentReference empRef = fireStoreDb.Collection("employees").Document(employee.EmployeeId);
                await empRef.SetAsync(employee, SetOptions.Overwrite);
            }
            catch
            {
                throw;
            }
        }
        public async Task<Employee> GetEmployeeData(string id)
        {
            try
            {
                DocumentReference docRef = fireStoreDb.Collection("employees").Document(id);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    Employee emp = snapshot.ConvertTo<Employee>();
                    emp.EmployeeId = snapshot.Id;
                    return emp;
                }
                else
                {
                    return new Employee();
                }
            }
            catch
            {
                throw;
            }
        }
        public async void DeleteEmployee(string id)
        {
            try
            {
                DocumentReference empRef = fireStoreDb.Collection("employees").Document(id);
                await empRef.DeleteAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Cities>> GetCityData()
        {
            try
            {
                Query citiesQuery = fireStoreDb.Collection("cities");
                QuerySnapshot citiesQuerySnapshot = await citiesQuery.GetSnapshotAsync();
                List<Cities> lstCity = new List<Cities>();

                foreach (DocumentSnapshot documentSnapshot in citiesQuerySnapshot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        Dictionary<string, object> city = documentSnapshot.ToDictionary();
                        string json = JsonConvert.SerializeObject(city);
                        Cities newCity = JsonConvert.DeserializeObject<Cities>(json);
                        lstCity.Add(newCity);
                    }
                }
                return lstCity;
            }
            catch
            {
                throw;
            }
        }
    }
}
