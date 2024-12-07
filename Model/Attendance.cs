using CommunityToolkit.Maui.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using STFREYA.Converters;
namespace STFREYA.Model
{
    public class Attendance
    {
        public int Id { get; set; } // Unique identifier for the attendance record

        [JsonPropertyName("student_id")]
        [JsonConverter(typeof(StringToIntConverter))]
        public int StudentId { get; set; } // Foreign key linking to the Student
        [JsonPropertyName("date")]
        public DateTime Date { get; set; } // Date of attendance
        [JsonPropertyName("status")]
        public string Status { get; set; } // Status: Present, Absent, Late
        
    }
}
