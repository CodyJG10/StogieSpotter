using StogieSpotter.App.Views;

namespace StogieSpotter.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute("home", typeof(Home));
            Routing.RegisterRoute("details", typeof(LocationDetails));
        }
    }
}