namespace STFREYA.View;
using STFREYA.ViewModel;

public partial class StudentView : ContentPage
{
	public StudentView()
	{
		InitializeComponent();
        BindingContext = new StudentViewModel();
    }

}