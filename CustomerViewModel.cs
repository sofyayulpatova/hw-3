using MvvmHelpers;
using System.Collections.ObjectModel;
using Homework3.Model;

namespace Homework3;

public class CustomerViewModel : BaseViewModel
{
    private readonly IDatabaseManager _db;

    public ObservableCollection<Customer> Customers { get; private set; } = new();


    public CustomerViewModel(IDatabaseManager db)
    {
        _db = db;

        LoadCustomers();
    }

    private async void LoadCustomers()
    {
        var customersList = await this._db.GetCustomers();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Customers = new ObservableCollection<Customer>(customersList);
            OnPropertyChanged(nameof(Customers));
        });
    }


}
