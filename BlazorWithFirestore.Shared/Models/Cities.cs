using System;
using Google.Cloud.Firestore;

namespace BlazorWithFirestore.Shared.Models
{
    [FirestoreData]
    public class Cities
    {
        public string CityName { get; set; }
    }
}
