using MvvmHelpers;
using System.Collections.ObjectModel;
using Homework3.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Homework3;

public class ProductViewModel : BaseViewModel, INotifyPropertyChanged
{
    private readonly IDatabaseManager _db;
    private string productMpn;
    private Product _selectedProduct;

    public ObservableCollection<Product> Products { get; private set; } = new();
    public ICommand LoadProductCommand { get; }
    public ICommand SaveChangesCommand { get; private set; }
    public ICommand DeleteProductCommand { get; private set; }

    private string _deletionMessage;
    public string DeletionMessage
    {
        get => _deletionMessage;
        set => SetProperty(ref _deletionMessage, value);
    }
    private string _name;
    private string _mpn;
    private decimal _price;
    public ICommand CreateProductCommand
    {
        get; private set;
    }


    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Mpn
    {
        get => _mpn;
        set => SetProperty(ref _mpn, value);
    }

    public decimal Price
    {
        get => _price;
        set => SetProperty(ref _price, value);
    }



    public ProductViewModel(IDatabaseManager db)
    {
        _db = db;

        LoadProducts();

        LoadProductCommand = new Command(async () => await LoadProductByMpn());
        SaveChangesCommand = new Command(async () => await SaveProductChanges());
        DeleteProductCommand = new Command(async () => await DeleteProduct());
        CreateProductCommand = new Command(async () => await CreateProduct());




    }
    public string ProductMpn
    {
        get => productMpn;
        set
        {
            if (productMpn != value)
            {
                productMpn = value;
                OnPropertyChanged(nameof(productMpn));
            }
        }
    }


    private async void LoadProducts()
    {
        var productsList = await this._db.GetProducts();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Products = new ObservableCollection<Product>(productsList);
            OnPropertyChanged(nameof(Products));
        });
    }

    public Product SelectedProduct
    {
        get => _selectedProduct;
        set
        {
            if (_selectedProduct != value)
            {
                _selectedProduct = value;
                OnPropertyChanged();
            }
        }
    }


    private async Task LoadProductByMpn()
    {
        if (string.IsNullOrWhiteSpace(ProductMpn))
        {
            // Notify user to provide an MPN
            return;
        }

        var product = await _db.GetProductByMpn(ProductMpn);
        if (product != null)
        {
            SelectedProduct = product;  // Set the loaded product to be the selected product
            Products.Clear();
            Products.Add(product);
        }
        else
        {
            // Notify user that no product was found
            SelectedProduct = null;
            Products.Clear();
        }
    }

    private async Task SaveProductChanges()
    {
        if (SelectedProduct == null)
        {
            // Optionally notify the user that no product is selected
            return;
        }

        await _db.UpdateProduct(SelectedProduct);
        // Optionally notify the user that the product has been successfully updated
    }
    private async Task DeleteProduct()
    {
        if (string.IsNullOrWhiteSpace(ProductMpn))
        {
          
            return;
        }

        var result = await _db.DeleteProductByMpn(ProductMpn);
        if (!result)
        {
            DeletionMessage = "Cannot delete: Product is used in order details.";

        }
        else
        {
            SelectedProduct = null; // Clear selected product
            
        }
    }

    private async Task CreateProduct()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Mpn))
        {
            DeletionMessage = "Please fill in all fields.";
            return;
        }

        await _db.AddProduct(Name, Mpn, Price);
        DeletionMessage = "Product created successfully.";
        // Optionally clear fields or refresh list
    }







}
