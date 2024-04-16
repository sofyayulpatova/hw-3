namespace Homework3
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            // MainPage = new AppShell();

            MainPage = serviceProvider.GetService<MainPage>();
        }
    }
}
