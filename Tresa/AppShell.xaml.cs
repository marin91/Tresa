namespace Tresa
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("settings", typeof(Views.Pages.SettingsPage));
            Routing.RegisterRoute("gallery", typeof(Views.Pages.GalleryPage));
        }
    }
}
