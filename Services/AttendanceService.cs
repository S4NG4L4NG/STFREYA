using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using STFREYA.Model;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;

namespace STFREYA.Services
{
    public class AttendanceService
    {
        private readonly HttpClient _httpClient;

        // Base URL for the API connection
        private const string BaseUrl = "http://localhost/PDC50/";

        public AttendanceService()
        {
            _httpClient = new HttpClient();
        }
        public async Task<List<Attendance>> GetAttendanceAsync()
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"{BaseUrl}get_attendance.php");
                Debug.WriteLine($"Raw JSON Response: {response}");
                var attendanceList = JsonSerializer.Deserialize<List<Attendance>>(response);
                return attendanceList ?? new List<Attendance>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetAllAttendanceAsync: {ex.Message}");
                return new List<Attendance>();
            }
        }
        public async Task<string> addAttendanceAsync(Attendance attendance)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}add_attendance.php", attendance);
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"AddAttendanceAsync response: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddAttendanceAsync: {ex.Message}");
                return "Error";
            }
        }
    }
}
