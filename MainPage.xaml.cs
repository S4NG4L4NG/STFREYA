namespace STFREYA
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void ClickedViewStudents(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//StudentView");
        }

        private async void ClieckedListSudents(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//StudentList");
        }

  
    }

}
