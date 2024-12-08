using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using STFREYA.Model;
using System.Diagnostics;

namespace STFREYA.Services
{
    public class AcademicHistoryService
    {

        private readonly HttpClient _httpClient;

        // Base URL for the API connection
        private const string BaseUrl = "http://localhost/PDC50/";

        public AcademicHistoryService()
        {
            _httpClient = new HttpClient();
        }

        // Fetch all academic history records for a specific student
        public async Task<List<AcademicHistory>> GetHistoryAsync(int studentId)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"{BaseUrl}get_academic_history.php?student_id={studentId}");
                //Debug.WriteLine($"Raw JSON Response: {response}");

                var historyList = JsonSerializer.Deserialize<List<AcademicHistory>>(response, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true // Handles case mismatch in property names
                });
                //Debugging
                //if (historyList != null)
                //{
                //    foreach (var record in historyList)
                //    {
                //        Debug.WriteLine($"Id: {record.Id}, StudentId: {record.StudentId}, Course: {record.Course}, Date: {record.Date}");
                //    }
                //}
                //else
                //{
                //    Debug.WriteLine("Deserialization returned null.");
                //}
                return historyList ?? new List<AcademicHistory>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetHistoryAsync: {ex.Message}");
                return new List<AcademicHistory>();
            }
        }

        // Add a new academic history record
        public async Task<string> AddHistoryAsync(AcademicHistory history)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}add_academic_history.php", history);
                var result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"AddHistoryAsync response: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddHistoryAsync: {ex.Message}");
                return "Error";
            }
        }
    }
}
