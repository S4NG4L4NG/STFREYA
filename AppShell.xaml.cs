
using STFREYA.View;

namespace STFREYA
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("ProfilePage", typeof(StudentProfileView));
            this.Navigated += OnNavigated;
        }

        private async void OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
            var currentPage = this.CurrentPage;
            if (currentPage != null)
            {
                // Start the page off-screen below the view and fully transparent
                currentPage.TranslationY = 800;
                currentPage.Opacity = 0;

                // Animate the page to slide up with a gentle bounce and fade in simultaneously
                await Task.WhenAll(
                    currentPage.TranslateTo(0, 0, 600, Easing.CubicOut), // Slide-up with deceleration over 600ms
                    currentPage.FadeTo(1, 600)                           // Fade-in over 600ms
                );

                // Add a subtle bounce effect after the initial slide-up
                await currentPage.TranslateTo(0, -10, 100, Easing.CubicOut); // Small bounce up
                await currentPage.TranslateTo(0, 0, 100, Easing.CubicIn);   // Settle back down
            }
        }
    }
}
