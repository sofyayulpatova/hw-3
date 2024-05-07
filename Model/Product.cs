
namespace Homework3.Model;
// Homework 1 is created by Sofya Yulpatova sy21002
/*

Create a class "Product":
With property "Name".
With property "Price" ( number with at least 2 digits after the decimal point).

*/

    public class Product
    {   
    public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; } // requires 2 digits after the decimal point

        public string Mpn { get; set; }

        public override string ToString()
        {
            return $"name is: {Name}, price is: {Price}, MPN is: {Mpn}";
        }
    }





