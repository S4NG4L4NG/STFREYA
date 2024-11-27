﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using STFREYA.Model;
using STFREYA.Services;

namespace STFREYA.ViewModel
{
    public class StudentViewModel : BindableObject
    {
        private readonly StudentService _studentService;
        public ObservableCollection<Student> Students { get; set; }

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

        private void FilterStudents()
        {
            if (_allStudents == null || string.IsNullOrWhiteSpace(SearchTerm))
            {
                // Show all students if no search term
                Students = new ObservableCollection<Student>(_allStudents);
            }
            else
            {
                // Filter students based on name, email, or course
                var filtered = _allStudents.Where(s =>
                    s.name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    s.email.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    s.course.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

                Students = new ObservableCollection<Student>(filtered);
            }

            OnPropertyChanged(nameof(Students));
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
                FilterStudents(); // Trigger filtering when the search term changes
            }
        }


        public StudentViewModel()
        {
            _studentService = new StudentService();
            Students = new ObservableCollection<Student>();
            LoadStudentCommand = new Command(async () => await LoadStudents());
            AddStudentCommand = new Command(async () => await AddStudent());
            DeleteCommand = new Command(async () => await DeleteStudent());
            UpdateStudentCommand = new Command(async () => await UpdateStudent());
        }

        // PUBLIC COMMANDS
        public ICommand LoadStudentCommand { get; }
        public ICommand AddStudentCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UpdateStudentCommand { get; }

        // LOAD STUDENTS FROM THE DATABASE
        private async Task LoadStudents()
        {
            Console.WriteLine("Loading students...");
            var students = await _studentService.GetStudentsAsync();
            _allStudents = new ObservableCollection<Student>(students); // Cache the full list
            Students = new ObservableCollection<Student>(_allStudents); // Initialize the displayed list
            OnPropertyChanged(nameof(Students));
        }

        // ADD STUDENT METHOD
        private async Task AddStudent()
        {
            if (!string.IsNullOrWhiteSpace(NameInput) &&
                !string.IsNullOrWhiteSpace(LastNameInput) &&
                !string.IsNullOrWhiteSpace(AgeInput) &&
                !string.IsNullOrWhiteSpace(EmailInput) &&
                !string.IsNullOrWhiteSpace(ContactNoInput) &&
                !string.IsNullOrWhiteSpace(SelectedCourse)) // Use SelectedCourse
            {
                var newStudent = new Student
                {
                    name = NameInput,
                    lastname = LastNameInput,
                    age = AgeInput, // Store Age as string
                    email = EmailInput,
                    contactno = ContactNoInput,
                    course = SelectedCourse // Use SelectedCourse
                };

                var result = await _studentService.AddStudentAsync(newStudent);

                if (result.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    await LoadStudents();
                    ClearInput();
                }
            }
            else
            {
                Console.WriteLine("Input validation failed: All fields must be filled.");
            }
        }



        private async Task DeleteStudent()
        {
            if (SelectedStudent != null)
            {
                var result = await _studentService.DeleteStudentAsync(SelectedStudent.student_id);
                await LoadStudents();
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

                var result = await _studentService.UpdateStudentAsync(SelectedStudent);

                if (result.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    await LoadStudents();
                    ClearInput();
                    Console.WriteLine("Student updated successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to update student: {result}");
                }
            }
            else
            {
                Console.WriteLine("No student selected for update.");
            }
        }
    }
}
