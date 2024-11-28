namespace STFREYA.View;
using STFREYA.ViewModel;

public partial class StudentList : ContentPage
{
	public StudentList()
	{
		InitializeComponent();
        BindingContext = new StudentViewModel();

    }
}