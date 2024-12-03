﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using STFREYA.Model;
using STFREYA.Services;
using System.Net.Mail;
using STFREYA.View;
using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using System.Text.Json;

namespace STFREYA.ViewModel
{

 

    public class StudentViewModel : BindableObject
    {
        private readonly StudentService _studentService;
        public ObservableCollection<Student> Students { get; set; }
        public ObservableCollection<AttendanceRecord> AttendanceRecords { get; set; }
        public ObservableCollection<string> AttendanceStatusOptions { get; set; } = new ObservableCollection<string>
        {
            "Present", "Absent", "Late"
        };

        private AddStudentModal _addPopup;
        private UpdateStudentModal _updatePopup;

        // FOR CLEARING INPUTS
        private void ClearInput()
        {
            NameInput = string.Empty;
            LastNameInput = string.Empty;
            AgeInput = string.Empty;
            EmailInput = string.Empty;
            ContactNoInput = string.Empty;
            CourseInput = string.Empty;
        }


        private ObservableCollection<Student> _allStudents;

        public ObservableCollection<string> CourseOptions { get; set; } = new ObservableCollection<string>
        {
            "BSCS", "BSIT", "BMMA"
        };

        public ObservableCollection<string> GenderOptions { get; set; } = new ObservableCollection<string>
    {
        "Male", "Female"
    };

        

        private string _totalStudentsDisplay;

        public string TotalStudentsDisplay
        {
            get => _totalStudentsDisplay;
            set
            {
                _totalStudentsDisplay = value;
                OnPropertyChanged();
            }
        }
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }


        private string _selectedCourse;
        public string SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                _selectedCourse = value;
                OnPropertyChanged();
            }
        }

        private string _selectedGender;
        public string SelectedGender
        {
            get => _selectedGender;
            set
            {
                _selectedGender = value;
                OnPropertyChanged();
            }
        }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
                UpdateEntryField();
            }
        }

        private string _qrCodeData;
        public string QRCodeData
        {
            get => _qrCodeData;
            set
            {
                _qrCodeData = value;
                OnPropertyChanged();
            }
        }

        private string _nameInput;
        public string NameInput
        {
            get => _nameInput;
            set
            {
                _nameInput = value;
                OnPropertyChanged();
            }
        }

        private string _lastNameInput;
        public string LastNameInput
        {
            get => _lastNameInput;
            set
            {
                _lastNameInput = value;
                OnPropertyChanged();
            }
        }

        private string _ageInput;
        public string AgeInput
        {
            get => _ageInput;
            set
            {
                _ageInput = value;
                OnPropertyChanged();
            }
        }

        private string _emailInput;
        public string EmailInput
        {
            get => _emailInput;
            set
            {
                _emailInput = value;
                OnPropertyChanged();
            }
        }

        private string _contactNoInput;
        public string ContactNoInput
        {
            get => _contactNoInput;
            set
            {
                _contactNoInput = value;
                OnPropertyChanged();
            }
        }

        private string _courseInput;
        public string CourseInput
        {
            get => _courseInput;
            set
            {
                _courseInput = value;
                OnPropertyChanged();
            }
        }

        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged();
                FilterStudents(); // Filter students whenever the search term changes
            }
        }

        public StudentViewModel()
        {
            _studentService = new StudentService();
            Students = new ObservableCollection<Student>();
            AttendanceRecords = new ObservableCollection<AttendanceRecord>();
            LoadStudentCommand = new Command(async () => await LoadStudents());
            AddStudentCommand = new Command(async () => await AddStudent());
            DeleteCommand = new Command(async () => await DeleteStudent());
            UpdateStudentCommand = new Command(async () => await UpdateStudent());
            FilterByCourseCommand = new Command<string>(FilterByCourse); // Initialize the command
            BackCommand = new Command(async () => await Shell.Current.GoToAsync("//MainPage"));
            ExportToCSVCommand = new Command(ExportToCSV);
            NavigateToProfileCommand = new Command<Student>(async (student) => await NavigateToProfile(student));
            GenerateReportCommand = new Command(GenerateReport);
            ExportAttendanceCommand = new Command(ExportAttendance);
            AddScoreCommand = new Command<Student>(AddScore);
            ExportPerformanceCommand = new Command(ExportPerformance);
            OpenAddStudentModalCommand = new Command(OpenAddStudentModal);
            OpenUpdateStudentModalCommand = new Command(OpenUpdateStudentModal);
            CloseModalCommand = new Command(CloseModal);
            CloseUpdateModalCommand = new Command(CloseUpdateModal);
            OpenMarkAttendanceModalCommand = new Command(OpenMarkAttendanceModal);
            LoadStudents();
        }

        // PUBLIC COMMANDS
        public ICommand LoadStudentCommand { get; }
        public ICommand AddStudentCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UpdateStudentCommand { get; }
        public ICommand FilterByCourseCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand ExportToCSVCommand { get; }
        public ICommand NavigateToProfileCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand MarkAttendanceCommand { get; }
        public ICommand ExportAttendanceCommand { get; }

        public ICommand AddScoreCommand { get; }
        public ICommand ExportPerformanceCommand { get; }

        public Command OpenAddStudentModalCommand { get; }
        public Command OpenUpdateStudentModalCommand { get; }
        public Command CloseModalCommand { get; }
        public Command CloseUpdateModalCommand { get; }

        public Command OpenMarkAttendanceModalCommand { get; }
        private async Task LoadStudents()
        {
            var students = await _studentService.GetStudentsAsync();
            _allStudents = new ObservableCollection<Student>(students); // Cache the full list
            Students = new ObservableCollection<Student>(_allStudents); // Initialize displayed list
            TotalStudentsDisplay = $"Overall Total Students: {_allStudents.Count}";
            OnPropertyChanged(nameof(Students));
            FilterStudents();
            ClearInput();
        }

        private void OpenAddStudentModal()
        {
            _addPopup = new AddStudentModal
            {
                BindingContext = this
            };
            App.Current.MainPage.ShowPopup(_addPopup);
        }
        private void OpenUpdateStudentModal()
        {
            if (SelectedStudent == null)
            {
                App.Current.MainPage.DisplayAlert("Error", "Please select a student to update.", "OK");
                return;
            }

            // Populate fields with selected student data
            NameInput = SelectedStudent.name;
            LastNameInput = SelectedStudent.lastname;
            AgeInput = SelectedStudent.age;
            EmailInput = SelectedStudent.email;
            ContactNoInput = SelectedStudent.contactno;
            SelectedCourse = SelectedStudent.course;
            SelectedGender = SelectedStudent.gender;

            _updatePopup = new UpdateStudentModal
            {
                BindingContext = this
            };
            App.Current.MainPage.ShowPopup(_updatePopup);
        }

        private void CloseModal()
        {
            _addPopup?.Close();
        }
        private void CloseUpdateModal()
        {
            _updatePopup?.Close();
        }

        private void OpenMarkAttendanceModal()
        {
            var attendanceViewModel = new AttendanceViewModel(SelectedDate, Students);
            var popup = new MarkAttendanceModal
            {
                BindingContext = attendanceViewModel
            };
            App.Current.MainPage.ShowPopup(popup);
        }
        // ADD STUDENT METHOD
        public async Task AddStudent()
        {
            // Validate inputs
            if (!string.IsNullOrWhiteSpace(NameInput) &&
                !string.IsNullOrWhiteSpace(LastNameInput) &&
                !string.IsNullOrWhiteSpace(AgeInput) &&
                !string.IsNullOrWhiteSpace(EmailInput) &&
                !string.IsNullOrWhiteSpace(ContactNoInput) &&
                !string.IsNullOrWhiteSpace(SelectedCourse) &&
                !string.IsNullOrWhiteSpace(SelectedGender))
            {
                var newStudent = new Student
                {
                    name = NameInput,
                    lastname = LastNameInput,
                    age = AgeInput,
                    email = EmailInput,
                    contactno = ContactNoInput,
                    course = SelectedCourse,
                    gender = SelectedGender
                };

                var result = await _studentService.AddStudentAsync(newStudent);
                var response = JsonSerializer.Deserialize<Dictionary<string, string>>(result);
                if (response != null && response.ContainsKey("message") && response["message"].Contains("successfully", StringComparison.OrdinalIgnoreCase))
                {
                    // Close modal, reset fields, and reload data
                    CloseModal();
                    ClearInput();
                    await LoadStudents();

                    // Show success alert
                    await App.Current.MainPage.DisplayAlert(
                        "Success",
                        "New student added successfully!",
                        "OK"
                    );
                }
                else
                {
                    // Show error alert
                    await App.Current.MainPage.DisplayAlert(
                        "Error",
                        $"Failed to add student: {result}",
                        "OK"
                    );
                }
            }
            else
            {
                // Show an alert for missing inputs
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "Please fill in all required fields.",
                    "OK"
                );
            }
        }

        private async Task DeleteStudent()
        {
            if (SelectedStudent != null)
            {
                // Show confirmation dialog
                bool isConfirmed = await App.Current.MainPage.DisplayAlert(
                    "Confirm Deletion",
                    $"Are you sure you want to delete {SelectedStudent.name} {SelectedStudent.lastname}?",
                    "Yes",
                    "No"
                );

                // Proceed only if the user confirms
                if (isConfirmed)
                {
                    var result = await _studentService.DeleteStudentAsync(SelectedStudent.student_id);
                    var response = JsonSerializer.Deserialize<Dictionary<string, string>>(result);
                    if (response != null && response.ContainsKey("message") && response["message"].Contains("successfully", StringComparison.OrdinalIgnoreCase))
                    {
                        await App.Current.MainPage.DisplayAlert(
                            "Success",
                            $"{SelectedStudent.name} has been deleted successfully.",
                            "OK"
                        );

                        await LoadStudents();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(
                            "Error",
                            $"Failed to delete the student: {result}",
                            "OK"
                        );
                    }
                }
            }
            else
            {
                // Show an alert if no student is selected
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "No student selected for deletion.",
                    "OK"
                );
            }
        }

        // UPDATE METHOD
        private async Task UpdateStudent()
        {
            if (SelectedStudent != null)
            {
                SelectedStudent.name = NameInput;
                SelectedStudent.lastname = LastNameInput;
                SelectedStudent.age = AgeInput; // Store Age as string
                SelectedStudent.email = EmailInput;
                SelectedStudent.contactno = ContactNoInput;
                SelectedStudent.course = SelectedCourse; // Use SelectedCourse
                SelectedStudent.gender = SelectedGender;

                var result = await _studentService.UpdateStudentAsync(SelectedStudent);
                var response = JsonSerializer.Deserialize<Dictionary<string, string>>(result);
                
                if (response != null && response.ContainsKey("message") && response["message"].Contains("successfully", StringComparison.OrdinalIgnoreCase))
                {
                    CloseUpdateModal();

                    // Clear input fields
                    ClearInput();
                    await LoadStudents();
                    // Show success alert
                    await App.Current.MainPage.DisplayAlert("Success", "Student updated successfully.", "OK");
                }
                else
                {
                    // Show error alert if the update fails
                    await App.Current.MainPage.DisplayAlert("Error", $"Failed to update student: {result}", "OK");

                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "No student selected for update.", "OK");
            }
        }

        private async Task NavigateToProfile(Student student)
        {
            if (student == null) return;

            await Shell.Current.GoToAsync($"///{nameof(StudentProfileView)}", new Dictionary<string, object>
            {
                { "SelectedStudent", student }
            });
        }

        //generate report method
        private void GenerateReport()
        {
            var report = new StringBuilder();
            report.AppendLine("Student Statistics Report");
            report.AppendLine($"Total Students: {TotalStudents}");
            report.AppendLine($"Average Age: {AverageAge:F2}");
            report.AppendLine("\nStudents Per Course:");
            foreach (var course in StudentsPerCourse)
            {
                report.AppendLine($"{course.Key}: {course.Value}");
            }
            report.AppendLine("\nPercentage Distribution:");
            foreach (var course in CoursePercentageDistribution)
            {
                report.AppendLine($"{course.Key}: {course.Value:F2}%");
            }
            report.AppendLine("\nGender Distribution:");
            foreach (var gender in GenderCounts)
            {
                report.AppendLine($"{gender.Key}: {gender.Value} student(s)");
            }

            report.AppendLine("\nGender Percentage Distribution:");
            foreach (var genderPercentage in GenderPercentageDistribution)
            {
                report.AppendLine($"{genderPercentage.Key}: {genderPercentage.Value:F2}%");
            }

            // Display the report
            App.Current.MainPage.DisplayAlert("Student Report", report.ToString(), "OK");

            //// Save to CSV file
            //var filePath = Path.Combine(FileSystem.AppDataDirectory, "StudentReport.csv");
            //File.WriteAllText(filePath, report.ToString());

            //// Notify the user
            //App.Current.MainPage.DisplayAlert("Export Successful", $"Report saved to {filePath}", "OK");
        }

        private void ExportAttendance()
        {
            try
            {
                var fileName = "AttendanceReport.csv";
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                var csvBuilder = new StringBuilder();
                csvBuilder.AppendLine("Student ID,Name,Date,Status");

                foreach (var record in AttendanceRecords)
                {
                    csvBuilder.AppendLine($"{record.StudentID},{record.Name},{record.Date},{record.Status}");
                }

                File.WriteAllText(filePath, csvBuilder.ToString());

                App.Current.MainPage.DisplayAlert("Export Successful", $"Attendance report saved to {filePath}", "OK");
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Export Failed", $"Error: {ex.Message}", "OK");
            }
        }

        private void AddScore(Student student)
        {
            if (student == null) return;

            var input = App.Current.MainPage.DisplayPromptAsync(
                "Add Score",
                $"Enter a score for {student.name} {student.lastname}:",
                "OK",
                "Cancel",
                "Enter score",
                keyboard: Keyboard.Numeric);

            input.ContinueWith(t =>
            {
                if (t.Result != null && int.TryParse(t.Result, out var score))
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        student.Scores.Add(score);
                        App.Current.MainPage.DisplayAlert(
                            "Success",
                            $"Score {score} added for {student.name} {student.lastname}.",
                            "OK");
                    });
                }
            });
        }

        private void ExportPerformance()
        {
            try
            {
                var fileName = "StudentPerformanceReport.csv";
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                var csvBuilder = new StringBuilder();
                csvBuilder.AppendLine("Student ID,Name,Average Score,Performance Level");

                foreach (var student in Students)
                {
                    var averageScore = student.Scores.Any() ? student.Scores.Average() : 0;
                    var performanceLevel = averageScore switch
                    {
                        > 90 => "Excellent",
                        >= 75 => "Good",
                        _ => "Needs Improvement"
                    };

                    csvBuilder.AppendLine($"{student.student_id},{student.name} {student.lastname},{averageScore:F2},{performanceLevel}");
                }

                File.WriteAllText(filePath, csvBuilder.ToString());

                App.Current.MainPage.DisplayAlert("Export Successful", $"Performance report saved to {filePath}", "OK");
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Export Failed", $"Error: {ex.Message}", "OK");
            }
        }

        private void FilterStudents()
        {
            if (_allStudents == null) return; // Avoid null reference errors

            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                // Reset to the full list if the search term is empty
                Students = new ObservableCollection<Student>(_allStudents);
            }
            else
            {
                // Filter the list by name, email, or course
                var filtered = _allStudents.Where(s =>
                    (!string.IsNullOrEmpty(s.name) && s.name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(s.email) && s.email.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (!string.IsNullOrEmpty(s.course) && s.course.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                Students = new ObservableCollection<Student>(filtered);
            }

            OnPropertyChanged(nameof(Students));
        }

        private void FilterByCourse(string course)
        {
            if (_allStudents == null || string.IsNullOrEmpty(course))
            {
                Console.WriteLine("FilterByCourse: No students or course is empty.");
                return; // Avoid null reference errors
            }

            // Filter students matching the selected course
            var filtered = _allStudents.Where(s => s.course.Equals(course, StringComparison.OrdinalIgnoreCase)).ToList();

            Students = new ObservableCollection<Student>(filtered);
            TotalStudentsDisplay = $"Total Students in {course}: {filtered.Count}";
            OnPropertyChanged(nameof(Students)); // Notify the UI
        }

        private void UpdateEntryField()
        {
            if (SelectedStudent != null)
            {
                NameInput = SelectedStudent.name;
                LastNameInput = SelectedStudent.lastname;
                AgeInput = SelectedStudent.age; // Age as string
                EmailInput = SelectedStudent.email;
                ContactNoInput = SelectedStudent.contactno;
                SelectedCourse = SelectedStudent.course; // Set the dropdown to the selected course
                SelectedGender = SelectedStudent.gender;
            }
            else
            {
                ClearInput();
            }
        }

        //generating report
        public int TotalStudents => Students?.Count ?? 0;

        public Dictionary<string, int> StudentsPerCourse =>
            Students?.GroupBy(s => s.course)
                     .ToDictionary(g => g.Key, g => g.Count()) ?? new Dictionary<string, int>();

        public double AverageAge =>
            Students?.Any() == true
                ? Students.Average(s => int.TryParse(s.age, out var age) ? age : 0)
                : 0;

        public Dictionary<string, double> CoursePercentageDistribution =>
            StudentsPerCourse?.ToDictionary(kvp => kvp.Key, kvp => (kvp.Value / (double)TotalStudents) * 100) ?? new Dictionary<string, double>();

        public Dictionary<string, int> GenderCounts =>
    Students?.GroupBy(s => s.gender)
             .ToDictionary(g => g.Key, g => g.Count()) ?? new Dictionary<string, int>();

        public Dictionary<string, double> GenderPercentageDistribution =>
            GenderCounts?.ToDictionary(kvp => kvp.Key, kvp => (kvp.Value / (double)TotalStudents) * 100) ?? new Dictionary<string, double>();

        private Dictionary<string, int> _courseCounts;
        public Dictionary<string, int> CourseCounts
        {
            get => _courseCounts;
            set
            {
                _courseCounts = value;
                OnPropertyChanged();
            }
        }

        private void CalculateCourseCounts()
        {
            if (_allStudents == null) return;

            CourseCounts = _allStudents
                .Where(s => !string.IsNullOrEmpty(s.course)) // Ignore null or empty courses
                .GroupBy(s => s.course)
                .ToDictionary(g => g.Key, g => g.Count());

            Console.WriteLine("CourseCounts updated:");
            foreach (var entry in CourseCounts)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
        }


        private void ExportToCSV()
        {
            try
            {
                var fileName = "StudentList.csv";
                var filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                var csvBuilder = new StringBuilder();
                csvBuilder.AppendLine("Name,Last Name,Age,Email,Contact No,Course");

                foreach (var student in Students)
                {
                    csvBuilder.AppendLine($"{student.name},{student.lastname},{student.age},{student.email},{student.contactno},{student.course}");
                }

                File.WriteAllText(filePath, csvBuilder.ToString());

                // Notify user of successful export
                App.Current.MainPage.DisplayAlert("Export Successful", $"CSV saved to: {filePath}", "OK");
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Export Failed", $"Error: {ex.Message}", "OK");
            }
        }

        // Email Validation Helper
        private bool IsValidEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                var mailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
    public class AttendanceRecord
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
    }

}

