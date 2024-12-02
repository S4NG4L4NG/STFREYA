using Microsoft.Maui.Controls;
using STFREYA.Model;
using System.Windows.Input;
using STFREYA.View;

namespace STFREYA.ViewModel
{
    public class StudentProfileViewModel : BindableObject
    {
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
                _course = value;
                OnPropertyChanged();
            }
        }


        public StudentProfileViewModel()
        {

        }

        public void LoadStudentData(Student student)
        {
            if (student == null) return;

            firstname = student.name; // Assuming "name" is the first name
            lastname = student.lastname;
            age = student.age;
            email = student.email;
            contactno = student.contactno;
            course = student.course;
            course = student.gender;
        }

       
    }
}
