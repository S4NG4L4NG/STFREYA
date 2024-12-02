using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using STFREYA.Model;
using System.Text;
using System.Threading.Tasks;

namespace STFREYA.Services
{
    public class StudentService
    {

        private readonly HttpClient _httpClient;

        // Base URL for the API connection
        private const string BaseUrl = "http://localhost/PDC50/";

        public StudentService()
        {
            _httpClient = new HttpClient();
        }

        // GET STUDENTS
        public async Task<List<Student>> GetStudentsAsync()
        {
            var response =
                await _httpClient.GetFromJsonAsync<List<Student>>($"{BaseUrl}get_students.php");
            return response ?? new List<Student>();
        }

        // ADD STUDENT
        public async Task<string> AddStudentAsync(Student student)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}add_student.php", student);
                var result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"AddStudentAsync response: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddStudentAsync: {ex.Message}");
                return "Error";
            }
        }

        // UPDATE STUDENT
        public async Task<string> UpdateStudentAsync(Student student)
        {
            var response =
                await _httpClient.PostAsJsonAsync($"{BaseUrl}update_student.php", student);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        // DELETE STUDENT
        public async Task<string> DeleteStudentAsync(int studentId)
        {
            var response =
                await _httpClient.PostAsJsonAsync($"{BaseUrl}delete_student.php", new { student_id = studentId });
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }

}