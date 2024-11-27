using System;
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

        private void UpdateEntryField()
        {
            if (SelectedStudent != null)
            {
                NameInput = SelectedStudent.name;
                LastNameInput = SelectedStudent.lastname;
                AgeInput = SelectedStudent.age; // Age as string
                EmailInput = SelectedStudent.email;
                ContactNoInput = SelectedStudent.contactno;
                CourseInput = SelectedStudent.course;
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
            var students = await _studentService.GetStudentsAsync();
            Students.Clear();
            foreach (var student in students)
            {
                Students.Add(student);
            }
        }

        // ADD STUDENT METHOD
        private async Task AddStudent()
        {
            if (!string.IsNullOrWhiteSpace(NameInput) &&
                !string.IsNullOrWhiteSpace(LastNameInput) &&
                !string.IsNullOrWhiteSpace(AgeInput) &&
                !string.IsNullOrWhiteSpace(EmailInput) &&
                !string.IsNullOrWhiteSpace(ContactNoInput) &&
                !string.IsNullOrWhiteSpace(CourseInput))
            {
                var newStudent = new Student
                {
                    name = NameInput,
                    lastname = LastNameInput,
                    age = AgeInput, // Store Age as string
                    email = EmailInput,
                    contactno = ContactNoInput,
                    course = CourseInput
                };

                var result = await _studentService.AddStudentAsync(newStudent);

                if (result.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    await LoadStudents();
                    ClearInput();
                }
            }
        }


        private async Task DeleteStudent()
        {
            if (SelectedStudent != null)
            {
                var result = await _studentService.DeleteStudentAsync(SelectedStudent.student_id);
                if (result.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    await LoadStudents();
                }
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
                SelectedStudent.course = CourseInput;

                var result = await _studentService.UpdateStudentAsync(SelectedStudent);
                if (result.Equals("Success", StringComparison.OrdinalIgnoreCase))
                {
                    await LoadStudents();
                    ClearInput();
                }
            }

        }
    }
}
