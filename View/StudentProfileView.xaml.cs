using STFREYA.Model;
using STFREYA.ViewModel;

namespace STFREYA.View
{
    [QueryProperty(nameof(SelectedStudent), "SelectedStudent")]
    public partial class StudentProfileView : ContentPage
    {
        public StudentProfileView()
        {
            InitializeComponent();
            BindingContext = new StudentProfileViewModel();
 
        }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                BindingContext = _selectedStudent; // Bind the data to the view
            }
        }

        private async void ClieckedToListSudents(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//StudentList");
        }


    }
}
