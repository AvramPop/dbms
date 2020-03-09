using System;
using System.Data.SqlClient;
using System.Data;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string connString = "Data Source=DESKTOP-T81R7QV; Initial catalog = Theologians; integrated security=true";
            SqlConnection conn = new SqlConnection(connString);
            conn.Open();
            string query = "select * from Book";
            SqlCommand command = new SqlCommand(query, conn);
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                Console.WriteLine("{0} {1} {2} {3}", dataReader[0], dataReader[1], dataReader[2], dataReader[3]);
            }
            conn.Close();

            SqlDataAdapter dataAdapterBook = new SqlDataAdapter(query, conn);
            DataSet dataSet = new DataSet();
            dataAdapterBook.Fill(dataSet, "Book");
            foreach(DataRow row in dataSet.Tables["Book"].Rows)
            {
                Console.WriteLine("{0} {1} {2} {3}", row["book_id"], row["name"], row["year_published"], row["publisher_id"]);
            }
        }
    }
}
