using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dracula
{
    class DBConnection
    {
        private SqlConnection connection;
        public DBConnection()
        {
            //constructor
        }
        ~DBConnection()
        {
            //destructor
            connection = null;
        }

        public void Add(StringBuilder text, string author, DateTime date, String medium, string recipient = "" )
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "INSERT Region(Text, Author, Date, Medium, Recipient) VALUES (@text, @author, @date, @medium, @recipient)";

            cmd.Parameters.AddWithValue("@text", text);
            cmd.Parameters.AddWithValue("@author", author);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@medium", medium);
            cmd.Parameters.AddWithValue("@recipient", recipient);

            cmd.ExecuteNonQuery();
        }

        public void Disconnect()
        {
            connection.Close();
        }
        public string ConnectToDatabase()
        {
            try
            {
                connection = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\Users\Reza\Documents\Visual Studio 2013\Projects\Dracula\Dracula\draculaBase.mdf';Integrated Security=True");

                connection.Open();
                return "Connected";
            }
            catch (SqlException e)
            {
                connection = null;
                return e.Message;
            }
        }
    }
}
