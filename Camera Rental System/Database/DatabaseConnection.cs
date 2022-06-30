using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera_Rental_System.Database
{
    public static class DatabaseConnection
    {
        /// <summary>
        /// The main SQLite connection object.
        /// </summary>
        private static SQLiteConnection Connection;


        public static bool Connect()
        {
            try
            {
                string cs = "Data Source=Database.db";
                string stm = "SELECT SQLITE_VERSION()";

                Connection = new SQLiteConnection(cs);
                Connection.Open();

                var cmd = new SQLiteCommand(stm, Connection);
                string version = cmd.ExecuteScalar().ToString();

                Debug.WriteLine($"[localdatabase] version: {version}");

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }

            return false;
        }

        const string SupplierTable = "Supplier";
        const string ClientTable = "Client";
        const string RentalOrderTable = "RentalOrder";
        const string RentalDetailsTable = "RentalDetails";
        const string ShippingTable = "Shipping";
        const string CameraTable = "Camera";
        const string AddOnsTable = "AddOns";
        const string AccountsTable = "Accounts";
        public static bool InitializeTables() =>
             CreateTableIfNotExist(SupplierTable,
                 "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                 "name TEXT, " +
                 "number TEXT") &&
            CreateTableIfNotExist(ClientTable,
                 "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                 "name TEXT, " +
                 "address TEXT, " +
                 "proofOfBilling TEXT") &&
            CreateTableIfNotExist(RentalOrderTable,
                 "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                 "cameraType TEXT, " +
                 "rentedOn DATE, " +
                 "amountPaid REAL") &&
            CreateTableIfNotExist(RentalDetailsTable,
                 "fee REAL, " +
                 "acceptedOn DATE") &&
            CreateTableIfNotExist(ShippingTable,
                 "name TEXT, address TEXT, " +
                 "company TEXT") &&
            CreateTableIfNotExist(CameraTable,
                 "name TEXT, description TEXT, specs TEXT, price REAL," +
                 "manufacturer TEXT") &&
            CreateTableIfNotExist(AddOnsTable,
                 "name TEXT, description TEXT, specs TEXT, price REAL," +
                 "manufacturer TEXT") &&
            CreateTableIfNotExist(AccountsTable,
                 "name TEXT, password TEXT")
            ;

        public static bool InsertAccount(string name, string pass)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = $"INSERT INTO {AccountsTable}(name, password) " +
                    $"VALUES(@a, @b)";

                cmd.Parameters.AddWithValue("a", name);
                cmd.Parameters.AddWithValue("b", pass);


                cmd.Prepare();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }
            return false;
        }
        public static IEnumerable<(string Name, string Password)> GetAccounts()
        {

            List<(string Name, string Password)> accounts = new List<(string Name, string Password)>();

            SQLiteDataReader rdr = new SQLiteCommand($"SELECT * FROM {AccountsTable}",
                Connection).ExecuteReader();

            while (rdr.Read())
                accounts.Add(
                    (GetElseDefault<string>(rdr, "name"), (GetElseDefault<string>(rdr, "password"))));

            return accounts;


        }
        public static T GetElseDefault<T>(SQLiteDataReader reader, string name)
        {
            var objVal = reader.GetValue(reader.GetOrdinal(name));
            if (objVal is null)
                return default(T);
            try
            {
                return (T)objVal;
            }
            catch { }

            return default(T);
        }



        public static bool Execute(string command)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();

                Debug.WriteLine($"[localdatabase] executed: {command}");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[localdatabase] error: {ex}");
            }

            return false;
        }

        public static bool CreateTableIfNotExist(string tableName, string typeAndNames) =>
            Execute($"CREATE TABLE IF NOT EXISTS {tableName}({typeAndNames})");

    }
}
