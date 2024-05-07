using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework3
{
    partial class ObjectCreationPage: ContentPage
    {
        private readonly IDatabaseManager _db;

        // public ObservableCollection<Customer> Customers { get; private set; } = new();


        public ObjectCreationPage(IDatabaseManager db)
        {
            _db = db;
            InitializeComponent();

        }

        /*private async void AddCustomerClicked(object sender, EventArgs e)
        {
            var customerName = CustomerNameEntry.Text;
            var customerSurname = CustomerSurnameEntry.Text;
            var customerEmail = CustomerEmailEntry.Text;

            // validate that it is not null and not white space
            if (string.IsNullOrWhiteSpace(customerName) ||
                string.IsNullOrWhiteSpace(customerSurname) ||
                string.IsNullOrWhiteSpace(customerEmail))
            {
                // Ideally, inform the user about the requirement
                await DisplayAlert("Alert", "You should enter customer's data !", "OK");
                return;
            }
            await DisplayAlert("Alert", "Customer is added!", "OK");
            await Navigation.PopAsync();

        }*/

        private async void AddProductClicked(object sender, EventArgs e)
        {
            var productName = ProductNameEntry.Text;
            var productPrice = ProductPriceEntry.Text;
            var productMpn = ProductMpnEntry.Text;

            // validate that it is not null and not white space
            if (string.IsNullOrWhiteSpace(productName) ||
                string.IsNullOrWhiteSpace(productPrice) ||
                string.IsNullOrEmpty(productMpn))
            {
            
                await DisplayAlert("Alert", "You should enter product's data !", "OK");
                return;
            }

            await _db.AddProduct(productName, productMpn, Int32.Parse(productPrice));

            await DisplayAlert("Alert", "Product is added!", "OK");

            await Navigation.PopAsync();



        }
        public async void OrderCreateClicked(object sender, EventArgs e)
        {
            var customerId = Convert.ToInt32(CustomerIdEntry.Text);
            // find customer by the id



            

            /*if (customer == null)
            {
                // Handle the case where the customer or employee wasn't found
                await DisplayAlert("Error", "Customer or Employee not found.", "OK");
                return;
            }
            */

           // create order itslef

            


            await DisplayAlert("Success", "Order created successfully.", "OK");
        }
    }
}
