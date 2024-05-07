using Microsoft.Extensions.DependencyInjection;

namespace Homework3
{
    public partial class MainPage : ContentPage
    {
        private IDatabaseManager _db;

        public MainPage(IDatabaseManager databaseManager)
        {
            InitializeComponent();

            _db = databaseManager;
        }

        private async void OnCustomerManagementClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CustomerManagementPage(_db));
        }
        private async void OnProductManagementClick(object sender, EventArgs e)
        {
        
            await Navigation.PushAsync(new ProductManagementPage(_db));
        }
        private async void OnOrderManagementClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrderManagementPage(_db));
        }
    }

}
