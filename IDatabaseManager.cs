using Homework3.Model;
namespace Homework3;
public interface IDatabaseManager
{
    public Task<List<Customer>> GetCustomers();
    public Task<List<Product>> GetProducts();

    public Task AddProduct(string name, string Mpn, decimal price);

    public Task<Product> GetProductByMpn(string mpn);

    public Task UpdateProduct(Product product);

    public Task<bool> DeleteProductByMpn(string mpn);

    public Task<List<Order>> GetOrders();

    public  Task<Customer> GetCustomerById(int id);

    public Task AddOrder(Order order);


    public Task<Order> GetOrderByNumber(string orderNumber);
    public Task<List<OrderDetails>> GetOrderDetails(int orderId);
    public Task AddOrderDetail(OrderDetails detail);
    public Task UpdateOrderDetail(OrderDetails detail);
    public Task DeleteOrderDetail(int detailId);



}
