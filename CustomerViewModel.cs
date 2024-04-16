using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Homework3.Model;

namespace Homework3;

public class CustomerViewModel : BaseViewModel
{
    private readonly DatabaseManager _db;
    public ObservableCollection<string> Items { get; }

    public CustomerViewModel(DatabaseManager db)
    {
        _db = db;

        Items = new ObservableCollection<string>();
        
        LoadData();
    }

    public async Task<List<Customer>> LoadData()
    {
        var customers = await _db.GetCustomers();

        return customers;
    }
}
