using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.IO;
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
            //
            CreateTableIfNotExist(CameraTable,
                 "id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, description TEXT, specs TEXT, price REAL," +
                 "manufacturer TEXT, rating INTEGER") &&

            CreateTableIfNotExist(AddOnsTable,
                 "id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT, description TEXT, specs TEXT, price REAL," +
                 "manufacturer TEXT") &&

            CreateTableIfNotExist(AccountsTable,
                 "name TEXT, password TEXT, accountType INTEGER");

        public static long InsertAddOn(string Name, string Description, string Specs, double Price, string Manufacturer)
        {
            var cmd = new SQLiteCommand(Connection);
            cmd.CommandText = $"INSERT INTO {AddOnsTable}(name, description, specs, price, manufacturer) " +
                $"VALUES(@b, @c, @d, @e, @f)";

            cmd.Parameters.AddWithValue("b", Name);
            cmd.Parameters.AddWithValue("c", Description);
            cmd.Parameters.AddWithValue("d", Specs);
            cmd.Parameters.AddWithValue("e", Price);
            cmd.Parameters.AddWithValue("f", Manufacturer);


            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            string sql = "SELECT last_insert_rowid()";
            cmd = new SQLiteCommand(sql, Connection);

            return (long)cmd.ExecuteScalar();
        }

        public static long InsertCamera(string Name, string Description, string Specs, double Price, string Manufacturer, long rating)
        {

            var cmd = new SQLiteCommand(Connection);
            cmd.CommandText = $"INSERT INTO {CameraTable}(name, description, specs, price, manufacturer, rating) " +
                $"VALUES(@b, @c, @d, @e, @f, @g)";

            cmd.Parameters.AddWithValue("b", Name);
            cmd.Parameters.AddWithValue("c", Description);
            cmd.Parameters.AddWithValue("d", Specs);
            cmd.Parameters.AddWithValue("e", Price);
            cmd.Parameters.AddWithValue("f", Manufacturer);
            cmd.Parameters.AddWithValue("g", rating);


            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            string sql = "SELECT last_insert_rowid()";
            cmd = new SQLiteCommand(sql, Connection);

            return (long)cmd.ExecuteScalar();
        }

        public static object GetCameras()
        {

            List<dynamic> cameras = new List<dynamic>();

            SQLiteDataReader rdr = new SQLiteCommand($"SELECT * FROM {CameraTable}",
                Connection).ExecuteReader();

            while (rdr.Read())
                cameras.Add(new
                {
                    Id = GetElseDefault<long>(rdr, "id"),
                    Name = GetElseDefault<string>(rdr, "name"),
                    Description = GetElseDefault<string>(rdr, "description"),
                    Specs = GetElseDefault<string>(rdr, "specs"),
                    Price = GetElseDefault<double>(rdr, "price"),
                    Manufacturer = GetElseDefault<string>(rdr, "manufacturer"),
                    Rating = GetElseDefault<long>(rdr, "rating")
                });

            return cameras;


        }
        public static object GetAddOns()
        {

            List<dynamic> addons = new List<dynamic>();

            SQLiteDataReader rdr = new SQLiteCommand($"SELECT * FROM {AddOnsTable}",
                Connection).ExecuteReader();

            while (rdr.Read())
                addons.Add(new
                {
                    Id = GetElseDefault<long>(rdr, "id"),
                    Name = GetElseDefault<string>(rdr, "name"),
                    Description = GetElseDefault<string>(rdr, "description"),
                    Specs = GetElseDefault<string>(rdr, "specs"),
                    Price = GetElseDefault<double>(rdr, "price"),
                    Manufacturer = GetElseDefault<string>(rdr, "manufacturer")  
                });

            return addons;


        }
        public static bool InsertAccount(string name, string pass, long type)
        {
            try
            {
                var cmd = new SQLiteCommand(Connection);
                cmd.CommandText = $"INSERT INTO {AccountsTable}(name, password, accountType) " +
                    $"VALUES(@a, @b, @c)";

                cmd.Parameters.AddWithValue("a", name);
                cmd.Parameters.AddWithValue("b", pass);
                cmd.Parameters.AddWithValue("c", type);


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
        public static IEnumerable<(string Name, string Password, long AccountType)> GetAccounts()
        {

            List<(string Name, string Password, long AccountType)> accounts = new List<(string Name, string Password, long AccountType)>();

            SQLiteDataReader rdr = new SQLiteCommand($"SELECT * FROM {AccountsTable}",
                Connection).ExecuteReader();

            while (rdr.Read())
                accounts.Add(
                    (GetElseDefault<string>(rdr, "name"),
                    GetElseDefault<string>(rdr, "password"),
                    (GetElseDefault<long>(rdr, "accountType"))));

            return accounts;


        }
        public static T GetElseDefault<T>(SQLiteDataReader reader, string name)
        {
            var objVal = reader.GetValue(reader.GetOrdinal(name));
            if (objVal is null || reader.IsDBNull(reader.GetOrdinal(name)))
                return default(T);
            try
            {
                return (T)Convert.ChangeType(objVal, typeof(T));
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
