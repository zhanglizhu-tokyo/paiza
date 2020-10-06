using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace paiza.DB
{
    class DBHelper
    {
        private static MySqlConnection connection = new MySqlConnection("Database='paiza';Data Source='localhost';User Id='root';Password='';charset='utf8';pooling=true");
        public List<List<string>> Select(string sql)
        {
            //Create a list to store the result
            List<List<string>> list = new List<List<string>>();
            

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    List<string> row = new List<string>();
                    for (int i = 0; i < dataReader.FieldCount;i++ )
                        row.Add(dataReader[i].ToString());
                    list.Add(row);
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }
        public bool Insert(string query)
        {          

            //open connection
            bool flag = false;
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    //Execute command
                    cmd.ExecuteNonQuery();
                    flag = true;
                }
                catch (Exception e)
                {
                    Log.WriteMsg("DB插入有误" +query+"\t"+ e.Message);                    
                }
                //close connection
                this.CloseConnection();               
            }            
            return flag;
        }

        //Update statement
        public bool Update(string query)
        {
            bool flag = false;
            //Open connection
            if (this.OpenConnection() == true)
            {
                try
                {
                    //create mysql command
                    MySqlCommand cmd = new MySqlCommand();
                    //Assign the query using CommandText
                    cmd.CommandText = query;
                    //Assign the connection using Connection
                    cmd.Connection = connection;

                    //Execute query
                    cmd.ExecuteNonQuery();
                }catch(Exception e){
                    Log.WriteMsg("DB更新有误" + query + "\t" + e.Message); 
                }
                //close connection
                this.CloseConnection();
            }
            return flag;
        }

        //Delete statement
        public bool Delete(string query)
        {
            bool flag = false;
            if (this.OpenConnection() == true)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }catch(Exception e){
                    Log.WriteMsg("DB删除有误" + query + "\t" + e.Message); 
                }
                this.CloseConnection();
            }
            return flag;
        }
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
    }
}
