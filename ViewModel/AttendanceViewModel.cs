﻿using System;
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
using static System.Runtime.InteropServices.JavaScript.JSType;
using CommunityToolkit.Maui.Storage;
using System.Windows.Input;

namespace STFREYA.ViewModel
{
    public class AttendanceViewModel : BindableObject
    {
        private readonly AttendanceService _attendanceService;
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
        private DateTime _selectedDate = DateTime.Now;
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
        public ICommand ExportAttendanceCommand { get; }
        public Command OpenMarkAttendanceModalCommand { get; }
        public AttendanceViewModel(ObservableCollection<Student> students)
        {
            _attendanceService = new AttendanceService();
            SelectedDate = SelectedDate;
            Students = students;

            // Initialize AttendanceItems by creating AttendanceItems for each student
            AttendanceItems = new ObservableCollection<AttendanceItem>(
                Students.Select(s => new AttendanceItem
                {
                    Student = s,
                    Status = "Absent" // Default status
                }));
            OpenMarkAttendanceModalCommand = new Command(OpenMarkAttendanceModal);
            SaveAttendanceCommand = new Command(SaveAttendance);
            LoadAttendanceForDate(SelectedDate);
            ExportAttendanceCommand = new Command(ExportAttendance);
        }

        private void OpenMarkAttendanceModal()
        {
            var attendanceViewModel = new AttendanceViewModel(Students);
            var popup = new MarkAttendanceModal
            {
                BindingContext = attendanceViewModel
            };
            App.Current.MainPage.ShowPopup(popup);
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
                        Status = item.Status,                // Status selected in the Picker
                    };

                    var result = await _attendanceService.addAttendanceAsync(attendance);
                    Debug.WriteLine($"Server response: {result}");

                    if (result != "Attendance record added successfully")
                    {
                        Debug.WriteLine($"Error saving attendance for StudentId: {attendance.StudentId}");
                    }
                }

                // Notify user and close modal
                await App.Current.MainPage.DisplayAlert("Success", "Attendance marked successfully!", "OK");
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

        private async void ExportAttendance()
        {
            try
            {
                // Validate attendance data
                if (AttendanceItems == null || !AttendanceItems.Any())
                {
                    await App.Current.MainPage.DisplayAlert("No Data", "No attendance records to export.", "OK");
                    return;
                }

                // Build the CSV content
                var csvBuilder = new StringBuilder();
                csvBuilder.AppendLine("Student ID,Name,Date,Status");

                foreach (var record in AttendanceItems)
                {
                    csvBuilder.AppendLine($"{record.Student.student_id},{record.Student.FullName},{SelectedDate:yyyy-MM-dd},{record.Status}");
                }

                // Generate a unique file name with timestamp
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss"); 
                var fileName = $"AttendanceReport_{SelectedDate:yyyyMMdd}_{timestamp}.csv";
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                // Write to file
                await File.WriteAllTextAsync(filePath, csvBuilder.ToString());

                // Display success message with file location
                await App.Current.MainPage.DisplayAlert("Export Successful", $"Attendance report saved to: {filePath}", "OK");
            }
            catch (UnauthorizedAccessException)
            {
                // Handle permission issues
                await App.Current.MainPage.DisplayAlert("Export Failed", "Permission denied. Cannot save the file.", "OK");
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                await App.Current.MainPage.DisplayAlert("Export Failed", $"An error occurred while exporting: {ex.Message}", "OK");
            }
        }

    }
}