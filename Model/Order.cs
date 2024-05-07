namespace Homework3.Model;
public class Order
{
    public int Id { get; set; }
    public string Number { get; set; }
    public string State { get; set; }
    public DateTime OrderDate { get; set; }
    public int CustomerId { get; set; } //  orders are linked to customers

    public string CustomerName { get; set; }

}