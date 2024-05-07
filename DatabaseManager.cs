using Homework3.Model;
using Microsoft.Data.Sqlite;
using Microsoft.Maui.Controls;
using System.Configuration;

namespace Homework3
{
 
    public class DatabaseManager: IDatabaseManager
    {
        private SqliteConnection? connection;

        public async Task<SqliteConnection> getDatabase()
        {
            if (this.connection != null) {
                return this.connection;
            }
            
            try
            {

                this.connection = new SqliteConnection(ConfigurationManager.ConnectionStrings["MyDB"].ConnectionString);

                await this.connection.OpenAsync();

                // This can be commented out if we don't want to reset
                await this.DropTables();

                var migration1 = new SqliteCommand("CREATE TABLE Customer (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Surname TEXT, Email TEXT)", this.connection);
                migration1.ExecuteNonQuery();

                var migration2 = new SqliteCommand("CREATE TABLE Product (Id INTEGER PRIMARY KEY AUTOINCREMENT,Name TEXT NOT NULL,MPN TEXT NOT NULL,Price REAL NOT NULL);", this.connection);
                migration2.ExecuteNonQuery();

                var migration3 = new SqliteCommand("CREATE TABLE Orders (Id INTEGER PRIMARY KEY AUTOINCREMENT, Number TEXT NOT NULL, State TEXT NOT NULL, OrderDate TEXT NOT NULL, CustomerId INTEGER, FOREIGN KEY (CustomerId) REFERENCES Customer(Id));", this.connection);
                migration3.ExecuteNonQuery();

                var migration4 = new SqliteCommand("CREATE TABLE OrderDetails (Id INTEGER PRIMARY KEY AUTOINCREMENT, ProductID INTEGER NOT NULL, Amount INTEGER NOT NULL, OrderId INTEGER NOT NULL, FOREIGN KEY (ProductId) REFERENCES Product(Id), FOREIGN KEY (OrderId) REFERENCES Orders(Id));", this.connection);
                migration4.ExecuteNonQuery();

                this.AddMockData();

                return this.connection;

            }
            catch (Exception ex)
            {

            }

            return null;
        }

        private async Task DropTables()
        {
            if (this.connection == null)
            {
                return;
            }

            var commands = new[]
                {
                "DROP TABLE IF EXISTS OrderDetails",
                "DROP TABLE IF EXISTS Orders",
                "DROP TABLE IF EXISTS Product",
                "DROP TABLE IF EXISTS Customer"
            };

            try
            {
                foreach (var commandText in commands)
                {
                    var command = new SqliteCommand(commandText, this.connection);
                    await command.ExecuteNonQueryAsync();
                }
            } catch (Exception ex)
            {
                
            }
            
        }

        public async void AddMockData()
        {
            if (this.connection == null) {
                return;
            }

            this.InsertMockData(
                "SELECT COUNT(*) FROM Product",
                @"INSERT INTO Product (Name, MPN, Price) VALUES
                ('Product 1', '123', 10.99),
                ('Product 2', '124', 12.99),
                ('Product 3', '125', 9.99)"
            );

            this.InsertMockData(
                "SELECT COUNT(*) FROM Customer",
                @"INSERT INTO Customer (Name, Surname, Email) VALUES
                ('Jon', 'Doe', 'john.doe@example.com'),
                ('Zoe', 'Bigl', 'zoe.bigl@example.com')"
            );

            this.InsertMockData(
                "SELECT COUNT(*) FROM Customer",
                @"INSERT INTO Customer (Name, Surname, Email) VALUES
                ('Jon', 'Doe', 'john.doe@example.com'),
                ('Zoe', 'Bigl', 'zoe.bigl@example.com')"
            );

            this.InsertMockData(
                "SELECT COUNT(*) FROM Orders",
                @"INSERT INTO Orders (Number, State, CustomerId, OrderDate) VALUES
                ('A-1', 'Pending', 1, '2024-04-12'),
                ('A-2', 'Finished', 2, '2024-04-14')"
            );

            this.InsertMockData(
                "SELECT COUNT(*) FROM OrderDetails",
                @"INSERT INTO OrderDetails (OrderId, ProductId, Amount) VALUES
                (1, 1, 1),
                (1, 2, 2),
                (2, 1, 5),
                (2, 2, 10),
                (2, 3, 15)"
            );
        }

        private async void InsertMockData(string checkQuery, string valuesQuery)
        {
            if (this.connection == null)
            {
                return;
            }

            try
            {
                var checkCount = new SqliteCommand(checkQuery, this.connection);
                var count = (long)await checkCount.ExecuteScalarAsync();

                if (count == 0)
                {
                    using (var transaction = this.connection.BeginTransaction())
                    {
                        var insertCommand = connection.CreateCommand();
                        insertCommand.CommandText = valuesQuery;
                        await insertCommand.ExecuteNonQueryAsync();
                        transaction.Commit();
                    }
                }
            } catch (Exception ex)
            {

            }

            
        }

        public async Task<List<Customer>> GetCustomers()
        {
            var customers = new List<Customer>();


            try
            {
                var db = await this.getDatabase();

                var command = new SqliteCommand("SELECT Id, Name, Surname, Email FROM Customer", db);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var customer = new Customer
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Surname = reader.GetString(2),
                            Email = reader.GetString(3)
                        };

                        customers.Add(customer);

                    }
                }
            } catch (Exception ex)
            {

            }


            return customers;
        }


        public async Task<List<Product>> GetProducts()
        {
            var products = new List<Product>();

            try
            {
                var db = await this.getDatabase();

                var command = new SqliteCommand("SELECT Id, Name, MPN, Price FROM Product", db);

                using (var reader = await command.ExecuteReaderAsync())
                {

                    while (await reader.ReadAsync())
                    {
                        var product = new Product
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Mpn = reader.GetString(2),
                            Price = reader.GetDecimal(3)
                        };

                        products.Add(product);

                    }
                }
            } catch (Exception ex)
            {

            }
            

            return products;
        }

        public async Task AddProduct(string name, string Mpn, decimal price)
        {
            
            try
            {
                var db = await this.getDatabase();
                var command = new SqliteCommand($"INSERT INTO Product (Name, MPN, Price) VALUES ('{name}','{Mpn}',{price})", db);
                await command.ExecuteNonQueryAsync();
            } catch (Exception ex)
            {

            }
            
        }

        public async Task<Product> GetProductByMpn(string mpn)
        {
           

            try
            {
                var db = await this.getDatabase();
                var command = new SqliteCommand("SELECT Id, Name, MPN, Price FROM Product WHERE MPN = @Mpn", db);
                command.Parameters.AddWithValue("@Mpn", mpn);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Mpn = reader.GetString(reader.GetOrdinal("MPN")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                        };
                    }
                }
            } catch (Exception ex)
            {

            }
            

            return null; // Return null if no product found
        }
        public async Task UpdateProduct(Product product)
        {
            try
            {
                var db = await this.getDatabase();
                var command = new SqliteCommand("UPDATE Product SET Name = @Name, Price = @Price WHERE MPN = @Mpn", db);

                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Mpn", product.Mpn);

                await command.ExecuteNonQueryAsync();
            } catch (Exception ex)
            {

            }
            
        }


        public async Task AddOrder(Order order)
        {
            try
            {
                var db = await this.getDatabase();
                var command = new SqliteCommand("INSERT INTO Orders (Number, State, OrderDate, CustomerId) VALUES (@Number, @State, @OrderDate, @CustomerId)", db);
                command.Parameters.AddWithValue("@Number", order.Number);
                command.Parameters.AddWithValue("@State", order.State);
                command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                command.Parameters.AddWithValue("@CustomerId", order.CustomerId);

                await command.ExecuteNonQueryAsync();
            } catch (Exception ex)
            {

            }
            
        }

        public async Task<List<Order>> GetOrders()
        {
            var orders = new List<Order>();
            try
            {
                var db = await this.getDatabase();
                var command = new SqliteCommand("SELECT Id, Number, State, OrderDate, CustomerId FROM Orders", db);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Order
                        {
                            Id = reader.GetInt32(0),
                            Number = reader.GetString(1),
                            State = reader.GetString(2),
                            OrderDate = reader.GetDateTime(3),
                            CustomerId = reader.GetInt32(4)
                        });
                    }
                }
            } catch (Exception ex) { 

            }

            return orders;

        }


        public async Task<Customer> GetCustomerById(int id)
        {
            try
            {
                var db = await this.getDatabase();
                var command = new SqliteCommand("SELECT Id, Name, Surname, Email FROM Customer WHERE Id = @id", db);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Customer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Surname = reader.GetString(reader.GetOrdinal("Surname")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        };
                    }
                }
            } catch (Exception ex)
            {

            }
           

            return null; // Return null if no product found

        }


        public async Task<Order> GetOrderByNumber(string orderNumber)
        {
            try
            {
                var db = await this.getDatabase();
                var command = new SqliteCommand("SELECT Id, Number, State, OrderDate, CustomerId FROM `Orders` WHERE Number = @number", db);
                command.Parameters.AddWithValue("@number", orderNumber);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Order
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Number = reader.GetString(reader.GetOrdinal("Number")),
                            State = reader.GetString(reader.GetOrdinal("State")),
                            OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId"))
                        };
                    }
                }
            } catch (Exception ex)
            {

            }
            

            return null; // Return null if no product found

        }

        public async Task<List<OrderDetails>> GetOrderDetails(int orderId)
        {
            var orderDetails = new List<OrderDetails>();

            try
            {
                var db = await this.getDatabase();
                var command = new SqliteCommand("SELECT  OD.Id,  OD.ProductID,  OD.Amount, P.Name as ProductName FROM OrderDetails OD LEFT JOIN Product P ON P.Id = OD.ProductID WHERE OD.OrderID = @id", db);
                command.Parameters.AddWithValue("@id", orderId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        orderDetails.Add(new OrderDetails
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                            Amount = reader.GetInt32(reader.GetOrdinal("Amount"))
                        });
                    }
                }
            } catch (Exception ex)
            {

            }
           

            return orderDetails; // Return null if no product found
        }

        public async Task AddOrderDetail(OrderDetails detail)
        {
            // Implementation to add a new order detail to the database
        }

        public async Task UpdateOrderDetail(OrderDetails detail)
        {
            // Implementation to update an existing order detail
        }

        public async Task DeleteOrderDetail(int detailId)
        {
            // Implementation to delete an order detail from the database
        }


        public async Task<bool> DeleteProductByMpn(string mpn)
        {
            try
            {
                var db = await this.getDatabase();
                // Check if the product is used in any order details
                var checkCommand = new SqliteCommand("SELECT COUNT(*) FROM OrderDetails WHERE ProductID = (SELECT Id FROM Product WHERE MPN = @Mpn)", db);
                checkCommand.Parameters.AddWithValue("@Mpn", mpn);

                var count = (long)await checkCommand.ExecuteScalarAsync();
                if (count > 0)
                {
                    // If the product is used in order details, return false indicating the product cannot be deleted
                    return false;
                }

                // If not used, proceed to delete
                var deleteCommand = new SqliteCommand("DELETE FROM Product WHERE MPN = @Mpn", db);
                deleteCommand.Parameters.AddWithValue("@Mpn", mpn);
                await deleteCommand.ExecuteNonQueryAsync();
            } catch (Exception ex)
            {
                return false;
            }
            
            return true; // Return true indicating the product was successfully deleted
        }

    }
}
