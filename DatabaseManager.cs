using Homework3.Model;
using Microsoft.Data.Sqlite;

namespace Homework3
{
 
    public class DatabaseManager
    {
        private SqliteConnection? connection;

        //private readonly ILogger<DatabaseManager> _logger;

        //public DatabaseManager(ILogger<DatabaseManager> logger)
        //{
        //    _logger = logger;
        //}

        public async Task<SqliteConnection> getDatabase()
        {
            if (this.connection != null) {
                return this.connection;
            }
            
            // string dbPath = Path.Combine(FileSystem.AppDataDirectory, "Homework3.db");
            string dbPath = Path.Combine("C:\\Users\\rusla\\source\\repos\\Homework3", "Homework3.db");

            this.connection = new SqliteConnection($"Filename={dbPath}");

            await this.connection.OpenAsync();

            // This can be commented out if we don't want to reset
            await this.DropTables();

            var migration1 = new SqliteCommand("CREATE TABLE Customer (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Surname TEXT, Email TEXT)", this.connection);
            migration1.ExecuteNonQuery();
            
            var migration2 = new SqliteCommand("CREATE TABLE Product (Id INTEGER PRIMARY KEY AUTOINCREMENT,Name TEXT NOT NULL,Price REAL NOT NULL);", this.connection);
            migration2.ExecuteNonQuery();
            
            var migration3 = new SqliteCommand("CREATE TABLE Orders (Id INTEGER PRIMARY KEY AUTOINCREMENT, Number TEXT NOT NULL, State TEXT NOT NULL, OrderDate TEXT NOT NULL, CustomerId INTEGER, FOREIGN KEY (CustomerId) REFERENCES Customer(Id));", this.connection);
            migration3.ExecuteNonQuery();
            
            var migration4 = new SqliteCommand("CREATE TABLE OrderDetails (Id INTEGER PRIMARY KEY AUTOINCREMENT, ProductID INTEGER NOT NULL, Amount INTEGER NOT NULL, OrderId INTEGER NOT NULL, FOREIGN KEY (ProductId) REFERENCES Product(Id), FOREIGN KEY (OrderId) REFERENCES Orders(Id));", this.connection);
            migration4.ExecuteNonQuery();

            this.AddMockData();

            return this.connection;
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

            foreach (var commandText in commands)
            {
                var command = new SqliteCommand(commandText, this.connection);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async void AddMockData()
        {
            if (this.connection == null) {
                return;
            }

            this.InsertMockData(
                "SELECT COUNT(*) FROM Product",
                @"INSERT INTO Product (Name, Price) VALUES
                ('Product 1', 10.99),
                ('Product 2', 12.99),
                ('Product 3', 9.99)"
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
                (1, 1, 'Pending'),
                (1, 2, 'Pending'),
                (2, 1, 'Finished'),
                (2, 2, 'Finished'),
                (2, 3, 'Finished')"
            );
        }

        private async void InsertMockData(string checkQuery, string valuesQuery)
        {
            if (this.connection == null)
            {
                return;
            }

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
        }

        public async void AddData(string name)
        {
            var db = await this.getDatabase();

            var command = new SqliteCommand("INSERT INTO MyTable (Name) VALUES (@Name)", db);
            
            command.Parameters.AddWithValue("@Name", name);
            command.ExecuteNonQuery();
        }

        public async Task<List<string>> GetData()
        {
            var db = await this.getDatabase();

            List<string> names = new List<string>();
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "MyDatabase.db");
            using (var connection = new SqliteConnection($"Filename={dbPath}"))
            {
                connection.Open();
                var command = new SqliteCommand("SELECT Name FROM MyTable", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        names.Add(reader.GetString(0));
                    }
                }
            }
            return names;
        }

        public async Task<List<Customer>> GetCustomers()
        {
            var db = await this.getDatabase();

            var customers = new List<Customer>();

            var command = new SqliteCommand("SELECT Id, Name, Surname, Email FROM Customer", db);

            using (var reader = await command.ExecuteReaderAsync())
            {
                // _logger.LogInformation("Logging customer list:");

                while (await reader.ReadAsync())
                {
                    var customer = new Customer
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Surname = reader.GetString(2),
                        Email =  reader.GetString(3)
                    };

                    customers.Add(customer);

                    // _logger.LogInformation($"Id: {customer.Id}, Name: {customer.Name}, Name: {customer.Surname}, Email: {customer.Email}");
                }
            }

            return customers;
        }
    }
}
