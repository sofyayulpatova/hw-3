namespace Homework3
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            // MainPage = new AppShell();

            var _page = serviceProvider.GetService<MainPage>();

            MainPage = new NavigationPage(_page);
        }
    }
}
