namespace Homework3;

using Microsoft.Maui.Controls;
using System;

public partial class CustomerManagementPage : ContentPage
{
    private IDatabaseManager _db;

    public CustomerManagementPage(IDatabaseManager databaseManager)
    {
        InitializeComponent();

        _db = databaseManager;

        BindingContext = new CustomerViewModel(databaseManager);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // https://learn.microsoft.com/en-us/dotnet/api/xamarin.forms.page.onappearing?view=xamarin-forms

        // Explicitly refresh the product list every time the page appears
        // CustomersList.ItemsSource = App.DataManager.FetchCustomers();
    }

    private async void OnCreateCustomerClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ObjectCreationPage(_db));

    }
    private async void OnUpdateCustomerClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(customerUpdationPage);
    }
    private async void OnDeleteCustomerClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(customerDeletionPage);
    }
}
