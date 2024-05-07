using MvvmHelpers;
using System.Collections.ObjectModel;
using Homework3.Model;

namespace Homework3;

 public partial class ProductManagementPage : ContentPage
{
    private readonly IDatabaseManager _db;

    // public ObservableCollection<Product> Products { get; private set; } = new();


    public ProductManagementPage(IDatabaseManager db)
    {
        _db = db;

        InitializeComponent();

        BindingContext = new ProductViewModel(db);

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // https://learn.microsoft.com/en-us/dotnet/api/xamarin.forms.page.onappearing?view=xamarin-forms

        // Explicitly refresh the product list every time the page appears
    }




    private async void OnCreateProductClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ObjectCreationPage(_db));

    }
    private async void OnUpdateProductClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(customerUpdationPage);
    }
    private async void OnDeleteProductClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(customerDeletionPage);
    }

    private async void OnLoadProductClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(customerDeletionPage);
        // await DisplayAlert("Error", ProductMpn, "OK");

    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(customerDeletionPage);
    }




}
