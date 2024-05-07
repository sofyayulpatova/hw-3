using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Homework3.Model;
using Microsoft.Maui.Controls;
using System.Windows.Input;
namespace Homework3
{
    public class OrderViewModel: BaseViewModel
    {
        private readonly IDatabaseManager _db;

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get => _selectedOrder;

            set
            {
                if (_selectedOrder != value)
                {
                    _selectedOrder = value;
                    OnPropertyChanged(nameof(SelectedOrder));
                }
            }
          
        }
        public bool HasSelectedOrder
        {
            get => SelectedOrder != null;
        }
        public ObservableCollection<OrderDetails> OrderDetailsCollection { get; private set; } = new ObservableCollection<OrderDetails>();

        public ObservableCollection<Order> Orders { get; private set; } = new ObservableCollection<Order>();
        public ObservableCollection<Customer> Customers { get; private set; } = new();
        public Customer SelectedCustomer { get; set; }
        public ICommand CreateOrderCommand { get; private set; }

        public ICommand LoadOrderCommand { get; private set; }
        public ICommand AddOrderDetailCommand { get; private set; }
        public ICommand UpdateOrderDetailCommand { get; private set; }
        public ICommand DeleteOrderDetailCommand { get; private set; }
        public string OrderNumberToLookup { get; set; }

        public OrderViewModel(IDatabaseManager db)
        {
            _db = db;
            LoadOrders();
            CreateOrderCommand = new Command(CreateOrder);

            LoadOrderCommand = new Command(async () => await LoadOrder());
            AddOrderDetailCommand = new Command<OrderDetails>(async (detail) => await AddOrderDetail(detail));
            UpdateOrderDetailCommand = new Command<OrderDetails>(async (detail) => await UpdateOrderDetail(detail));
            DeleteOrderDetailCommand = new Command<int>(async (detailId) => await DeleteOrderDetail(detailId));
        }

        private async Task LoadOrder()
        {
            SelectedOrder = await _db.GetOrderByNumber(OrderNumberToLookup);
            if (SelectedOrder != null)
            {
                var details = await _db.GetOrderDetails(SelectedOrder.Id);
                OrderDetailsCollection.Clear();
                foreach (var detail in details)
                {
                    OrderDetailsCollection.Add(detail);
                }
            }
        }

        private async Task UpdateOrderDetail(OrderDetails detail)
        {
            await _db.UpdateOrderDetail(detail);
        }

        private async Task DeleteOrderDetail(int detailId)
        {
            await _db.DeleteOrderDetail(detailId);
            var detail = OrderDetailsCollection.FirstOrDefault(d => d.Id == detailId);
            if (detail != null)
            {
                OrderDetailsCollection.Remove(detail);
            }
        }

        private async Task AddOrderDetail(OrderDetails detail)
        {
            
        }


        

        
        private ObservableCollection<OrderDetails> _productDetails = new ObservableCollection<OrderDetails>();
        public ObservableCollection<OrderDetails> ProductDetails
        {
            get => _productDetails;
            set => SetProperty(ref _productDetails, value);
        }







        private async void CreateOrder()
        {
            if (SelectedCustomer == null)
            {
                
                return;
            }

            var newOrder = new Order
            {
                Number = GenerateOrderNumber(),
                State = "New",
                OrderDate = DateTime.Now,
                CustomerId = SelectedCustomer.Id,
                CustomerName = SelectedCustomer.Name // For display purposes only
            };

            await _db.AddOrder(newOrder);
            Orders.Add(newOrder);
        }

        private string GenerateOrderNumber()
        {
            // Implement logic to generate unique order numbers
            return $"ORD-{DateTime.Now.Ticks}";
        }



        public async void LoadOrders()
        {
            var orders = await _db.GetOrders();
            var customers = await _db.GetCustomers();
            Orders.Clear();
            Customers.Clear();
            foreach (var order in orders)
            {
                var customer = customers.FirstOrDefault(c => c.Id == order.CustomerId);
                if (customer != null)
                {
                    order.CustomerName = customer.Name; 
                }
                Orders.Add(order);
            }

            foreach (var customer in customers)
            {
                Customers.Add(customer);
            }
        }

    }

    


}
