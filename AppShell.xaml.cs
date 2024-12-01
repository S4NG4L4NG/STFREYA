
using STFREYA.View;

namespace STFREYA
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("ProfilePage", typeof(StudentProfileView));
        }
    }
}
    