using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class alohaDataConnection
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        string myConnectionString = "server=csshrpt.eku.edu;user=csc440;database=csc440;port=3306;password=CSC440student;";
        MySqlDataReader rdr = null;

        public DataSet getCustomerInfo()
        {

            using(conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
            {
                string stm = "SELECT * FROM customerinfo";
                DataSet ds = new DataSet();
                MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(stm, conn);
                mysqlDataAdapter.Fill(ds);
                return ds;
            }
        }

        public DataSet getItemInfo()
        {

            using (conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
            {
                string stm = "SELECT * FROM iteminfo";
                DataSet ds = new DataSet();
                MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(stm, conn);
                mysqlDataAdapter.Fill(ds);
                return ds;
            }
        }
        public DataSet getOrderedItemInfo()
        {

            using (conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
            {
                string stm = "SELECT * FROM orderediteminfo";
                DataSet ds = new DataSet();
                MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(stm, conn);
                mysqlDataAdapter.Fill(ds);
                return ds;
            }
        }
        public DataSet getOrderInfo()
        {

            using (conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
            {
                string stm = "SELECT * FROM orderinfo";
                DataSet ds = new DataSet();
                MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(stm, conn);
                mysqlDataAdapter.Fill(ds);
                return ds;
            }
        }
        public DataSet findCustomerInfo(int customerID)
        {

            using (conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
            {
                string stm = "SELECT * FROM customerinfo WHERE ID = " + customerID + ";";
                DataSet ds = new DataSet();
                MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(stm, conn);
                mysqlDataAdapter.Fill(ds);
                return ds;
            }
        }
        public DataSet findItemInfo(string itemName)
        {

            using (conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
            {
                string stm = "SELECT * FROM iteminfo WHERE itemName = '"+itemName+"';";
                DataSet ds = new DataSet();
                MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(stm, conn);
                mysqlDataAdapter.Fill(ds);
                return ds;
            }
        }
        public DataSet findOrderInfobyCustomer(int ID)
        {

            using (conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
            {
                string stm = "SELECT * FROM orderinfo AS oi INNER JOIN orderediteminfo AS oii ON oi.orderID=oii.orderid WHERE oi.customerID = " + ID + ";";
                DataSet ds = new DataSet();
                MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(stm, conn);
                mysqlDataAdapter.Fill(ds);
                return ds;
            }
        }
        public DataSet findOrderInfobyOrder(int ID)
        {

            using (conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString))
            {
                string stm = "SELECT * FROM orderinfo AS oi INNER JOIN orderediteminfo AS oii ON oi.orderID=oii.orderid WHERE oi.orderID = " + ID + ";";
                DataSet ds = new DataSet();
                MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(stm, conn);
                mysqlDataAdapter.Fill(ds);
                return ds;
            }
        }
        public void insertCustomerInfo(string first_name, string last_name, string phone, string email, string address)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                cmd.Connection = conn;
                cmd.Connection.Open();

                string stm = "INSERT INTO customerinfo(Fname, Lname, Phone, Email, Address) VALUES (@first, @last, @phone,@email,@address);";
                cmd.CommandText = stm;

                cmd.Parameters.AddWithValue("@first", first_name);
                cmd.Parameters.AddWithValue("@last", last_name);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@address", address);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {
                
            }
            finally
            {
                //always close the connection
                cmd.Connection.Close();
            }
        }
        public void insertItemInfo(string iname, string iprice, decimal quantity)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                cmd.Connection = conn;
                cmd.Connection.Open();

                string stm = "INSERT INTO iteminfo(itemName, Price, Quantity) VALUES (@name, @price, @quant);";
                cmd.CommandText = stm;

                cmd.Parameters.AddWithValue("@name", iname);
                cmd.Parameters.AddWithValue("@price", iprice);
                cmd.Parameters.AddWithValue("@quant", quantity);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {

            }
            finally
            {
                //always close the connection
                cmd.Connection.Close();
            }
        }
        public long insertOrderInfo(int customerID, string status, decimal total)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                cmd.Connection = conn;
                cmd.Connection.Open();

                string stm = "INSERT INTO orderinfo(customerID, orderStatus, total, date) VALUES (@customerID, @status, @tot, @date);";
                cmd.CommandText = stm;

                DateTime now = DateTime.Now;

                cmd.Parameters.AddWithValue("@customerID", customerID);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@tot", total);
                cmd.Parameters.AddWithValue("@date", now);

                cmd.ExecuteNonQuery();
                long ID = cmd.LastInsertedId;
                return ID;
            }
            catch (MySqlException)
            {
                return 0;
            }
            finally
            {
                //always close the connection
                cmd.Connection.Close();
            }
        }
        public int insertOrderedItemInfo(string iname, decimal iprice, int quantity, int orderID, int totalQuantity)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                cmd.Connection = conn;
                cmd.Connection.Open();

                string stm = "INSERT INTO orderediteminfo(orderID, itemName, itemQuantity, itemPrice) VALUES (@orderID, @name, @quant, @price);";
                cmd.CommandText = stm;
                int quant = totalQuantity - quantity;
                cmd.Parameters.AddWithValue("@name", iname);
                cmd.Parameters.AddWithValue("@price", iprice);
                cmd.Parameters.AddWithValue("@quant", quantity);
                cmd.Parameters.AddWithValue("@orderID", orderID);

                cmd.ExecuteNonQuery();

                return quant;
            }
            catch (MySqlException)
            {
                return 0;
            }
            finally
            {
                //always close the connection
                cmd.Connection.Close();
            }
        }
        public void removeItem(string itemName)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                cmd.Connection = conn;
                cmd.Connection.Open();

                string stm = "DELETE FROM iteminfo WHERE itemName = @name;";
                cmd.CommandText = stm;

                cmd.Parameters.AddWithValue("@name", itemName);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {

            }
            finally
            {
                //always close the connection
                cmd.Connection.Close();
            }
        }
        public void removeCustomer(string customerID)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                cmd.Connection = conn;
                cmd.Connection.Open();

                string stm = "DELETE FROM customerinfo WHERE ID = @ID;";
                cmd.CommandText = stm;

                cmd.Parameters.AddWithValue("@ID", customerID);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {

            }
            finally
            {
                //always close the connection
                cmd.Connection.Close();
            }
        }
        public void updateItemQuantity(string name, int quantity)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                cmd.Connection = conn;
                cmd.Connection.Open();

                string stm = "UPDATE iteminfo SET Quantity = @quantity WHERE itemName = @name;";
                cmd.CommandText = stm;

                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@name", name);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {

            }
            finally
            {
                //always close the connection
                cmd.Connection.Close();
            }
        }
        public void updateItem(string name, decimal price, int quantity, int id)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                cmd.Connection = conn;
                cmd.Connection.Open();

                string stm = "UPDATE iteminfo SET itemName = @name, Price = @price, Quantity = @quantity WHERE ID = @id;";
                cmd.CommandText = stm;

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {

            }
            finally
            {
                //always close the connection
                cmd.Connection.Close();
            }
        }
        public void updateCustomer(string first, string last, string phone, string email, string address, int id)
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                cmd.Connection = conn;
                cmd.Connection.Open();

                string stm = "UPDATE customerinfo SET Fname = @f, Lname = @l, Phone = @p, Email = @e, Address = @a WHERE ID = @id;";
                cmd.CommandText = stm;

                cmd.Parameters.AddWithValue("@f", first);
                cmd.Parameters.AddWithValue("@l", last);
                cmd.Parameters.AddWithValue("@p", phone);
                cmd.Parameters.AddWithValue("@e", email);
                cmd.Parameters.AddWithValue("@a", address);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
            catch (MySqlException)
            {

            }
            finally
            {
                //always close the connection
                cmd.Connection.Close();
            }
        }
        public decimal profit()
        {
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = myConnectionString;
                cmd.Connection = conn;
                cmd.Connection.Open();

                string stm = "SELECT SUM(total) FROM orderinfo;";
                cmd.CommandText = stm;
                cmd.ExecuteNonQuery();
                decimal tot = Convert.ToDecimal(cmd.ExecuteScalar());
                return tot;
                
            }
            catch (MySqlException)
            {
                return 0;
            }
            finally
            {
                //always close the connection
                cmd.Connection.Close();
            }
        }
    }
}
