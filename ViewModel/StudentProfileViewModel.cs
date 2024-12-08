using Microsoft.Maui.Controls;
using STFREYA.Model;
using System.Windows.Input;
using STFREYA.View;
using System.Collections.ObjectModel;
using STFREYA.Services;
using System.Diagnostics;

namespace STFREYA.ViewModel
{
    public class StudentProfileViewModel : BindableObject
    {
        private readonly AcademicHistoryService _academicHistoryService;

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }
        private string _firstname;
        public string firstname
        {
            get => _firstname;
            set
            {
                _firstname = value;
                OnPropertyChanged();
            }
        }

        private string _lastname;
        public string lastname
        {
            get => _lastname;
            set
            {
                _lastname = value;
                OnPropertyChanged();
            }
        }

        private string _age;
        public string age
        {
            get => _age;
            set
            {
                _age = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _contactno;
        public string contactno
        {
            get => _contactno;
            set
            {
                _contactno = value;
                OnPropertyChanged();
            }
        }

        private string _course;
        public string course
        {
            get => _course;
            set
            {
                _course = value;
                OnPropertyChanged();
            }
        }

        private string _gender;
        public string gender
        {
            get => _gender;
            set
            {
                _gender = value; // Correct assignment here
                OnPropertyChanged();
            }
        }
        public ObservableCollection<AcademicHistory> AcademicHistory { get; set; } = new ObservableCollection<AcademicHistory>();

        public StudentProfileViewModel()
        {
            _academicHistoryService = new AcademicHistoryService();
            AcademicHistory = new ObservableCollection<AcademicHistory>();
        }
        public async Task InitializeViewModelAsync(Student student)
        {
            if (student == null)
            {
                Debug.WriteLine("Error: Student is null.");
                return;
            }

            LoadStudentData(student);
            await LoadAcademicHistory(student.student_id);
        }

        public void LoadStudentData(Student student)
        {
            if (student == null) return;

            firstname = student.name;
            lastname = student.lastname;
            age = student.age;
            email = student.email;
            contactno = student.contactno;
            course = student.course;
            gender = student.gender;
        }

        private async Task LoadAcademicHistory(int studentId)
        {
            try
            {
                var historyRecords = await _academicHistoryService.GetHistoryAsync(studentId);
                Debug.WriteLine($"History Records Count: {historyRecords?.Count}");
                if (historyRecords == null || !historyRecords.Any())
                {
                    Debug.WriteLine("No academic history records found.");
                    return;
                }

                AcademicHistory.Clear();
                foreach (var record in historyRecords)
                {
                    Debug.WriteLine($"Adding record: {record.Course}, {record.Date}");
                    AcademicHistory.Add(record);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading academic history: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to load academic history.", "OK");
            }
        }

    }
}
