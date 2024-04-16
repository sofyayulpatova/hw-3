namespace Homework3
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        private DatabaseManager? _db;

        public MainPage(DatabaseManager databaseManager)
        {
            InitializeComponent();

            _db = databaseManager;
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            var customers = await this._db.GetCustomers();

            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
