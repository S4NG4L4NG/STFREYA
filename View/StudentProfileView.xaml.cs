using STFREYA.Model;
using STFREYA.ViewModel;
using STFREYA.Services;
using System.Diagnostics;

namespace STFREYA.View
{
    [QueryProperty(nameof(Student), "Student")]
    public partial class StudentProfileView : ContentPage
    {
        private Student _student;
        public Student Student
        {
            get => _student;
            set
            {
                _student = value;
                if (BindingContext is StudentProfileViewModel viewModel)
                {
                    viewModel.InitializeViewModelAsync(_student);
                }
            }
        }

        public StudentProfileView()
        {
            InitializeComponent();
            BindingContext = new StudentProfileViewModel(); // No need to pass student here
            Debug.WriteLine($"BindingContext is: {BindingContext}");
        }



        private async void ClickedToListSudents(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//StudentList");
        }


    }
}
