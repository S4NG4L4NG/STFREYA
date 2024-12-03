using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STFREYA.Model;
using STFREYA.View;
using STFREYA.Services;
using System.Text.Json;
using CommunityToolkit.Maui.Views;
using System.Diagnostics;

namespace STFREYA.ViewModel
{
    public class AttendanceViewModel : BindableObject
    {
        private readonly AttendanceService _attendanceService;
        private MarkAttendanceModal _attendancePopup;
        public ObservableCollection<AttendanceItem> AttendanceItems { get; set; } // A collection of attendance items
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<string> AttendanceStatusOptions { get; set; } = new ObservableCollection<string>
        {
            "Present", "Late", "Absent"
        };
        private string _selectedStatus;
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                if (_selectedStatus != value)
                {
                    _selectedStatus = value;
                    OnPropertyChanged();
                }
            }
        }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                    LoadAttendanceForDate(_selectedDate); // Load attendance for the new date
                }
            }
        }

        public Command SaveAttendanceCommand { get; }
        public Command CloseModalCommand { get; }

        public AttendanceViewModel(DateTime selectedDate, ObservableCollection<Student> students)
        {
            _attendanceService = new AttendanceService();
            SelectedDate = selectedDate;
            Students = students;

            // Initialize AttendanceItems by creating AttendanceItems for each student
            AttendanceItems = new ObservableCollection<AttendanceItem>(
                Students.Select(s => new AttendanceItem
                {
                    Student = s,
                    Status = "Absent" // Default status
                }));

            SaveAttendanceCommand = new Command(SaveAttendance);
            CloseModalCommand = new Command(CloseModal);
        }

        private async void SaveAttendance()
        {
            try
            {
                // Loop through each attendance item and save its status
                foreach (var item in AttendanceItems)
                {
                    var attendance = new Attendance
                    {
                        StudentId = item.Student.student_id, // Get student ID
                        Date = SelectedDate,                // Selected date
                        Status = item.Status                // Status selected in the Picker
                    };

                    var result = await _attendanceService.addAttendanceAsync(attendance);
                    Debug.WriteLine($"Server response: {result}");

                    if (result != "Success")
                    {
                        Debug.WriteLine($"Error saving attendance for StudentId: {attendance.StudentId}");
                    }
                }

                // Notify user and close modal
                await App.Current.MainPage.DisplayAlert("Success", "Attendance marked successfully!", "OK");
                CloseModal();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in SaveAttendance: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("Error", "An error occurred while marking attendance.", "OK");
            }
        }

        private async void LoadAttendanceForDate(DateTime date)
        {
            try
            {
                // Fetch all attendance records
                var allAttendanceRecords = await _attendanceService.GetAttendanceAsync();
                string formattedDate = date.ToString("yyyy-MM-dd");
                // Filter records for the selected date
                var attendanceRecords = allAttendanceRecords
                    .Where(r => r.Date.Date == date.Date)
                    .ToList();
                foreach (var record in allAttendanceRecords)
                {
                    Debug.WriteLine($"StudentId: {record.StudentId}, Date: {record.Date.Date}, Status: {record.Status}\n");
                }
                // Update AttendanceItems with fetched records
                foreach (var item in AttendanceItems)
                {
                    var record = attendanceRecords.FirstOrDefault(r => r.StudentId == item.Student.student_id);
                    if (record != null)
                    {
                        item.Status = record.Status; // Update only if a record exists
                    }
                    else
                    {
                        item.Status = "Absent"; // Default to "Absent" only if Status is blank
                    }
                }

                OnPropertyChanged(nameof(AttendanceItems));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in LoadAttendanceForDate: {ex.Message}");
            }
        }

        private void CloseModal()
        {
            _attendancePopup?.Close();
        }
    }
}