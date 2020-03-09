using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private SqlConnection conn;

        public Form1()
        {
            InitializeComponent();
            FillData();
        }

        private void FillData() // populate the form with data
        {
            //sql connection
            // sql data adapter, sql data set, data relation

            string connString = "Data Source=DESKTOP-T81R7QV; Initial catalog = Theologians; integrated security=true";
            conn = new SqlConnection(connString);
            conn.Open();
            string publisherSelectQuery = "select * from Publisher";

            SqlDataAdapter dataAdapterPublisher = new SqlDataAdapter(publisherSelectQuery, conn);
            DataSet dataSet = new DataSet();

            dataAdapterPublisher.Fill(dataSet, "Publisher");
            dataGridView1.DataSource = dataSet.Tables["Publisher"];

            conn.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            DataSet dataSet = new DataSet();
            Debug.WriteLine(dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            string bookSelectQuery = "select * from Book where publisher_id = " + dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value.ToString();
            SqlDataAdapter dataAdapterBook = new SqlDataAdapter(bookSelectQuery, conn);
            dataAdapterBook.Fill(dataSet, "Book");
            dataGridView2.DataSource = dataSet.Tables["Book"];
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            conn.Open();
            DataSet dataSet = new DataSet();
            string addBookQuery = "insert into Book(name, year_published, publisher_id) values ('" +
                dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[1].Value + "', " +
                dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[2].Value + "," +
                dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value.ToString() + ")";
           SqlCommand addBookCommand = new SqlCommand(addBookQuery, conn);
           try
            { 
            addBookCommand.ExecuteNonQuery();
        }
            catch(SqlException ex)
            {
                Debug.WriteLine("sql exception");
            }
    string bookSelectQuery = "select * from Book where publisher_id = " + dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value.ToString();

            SqlDataAdapter dataAdapterBook = new SqlDataAdapter(bookSelectQuery, conn);
            dataAdapterBook.Fill(dataSet, "Book");
            dataGridView2.DataSource = dataSet.Tables["Book"];
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            DataSet dataSet = new DataSet();
            string removeBookQuery = "delete from Book where book_id = " + dataGridView2.SelectedCells[0].OwningRow.Cells[0].Value.ToString();
            SqlCommand removeBookCommand = new SqlCommand(removeBookQuery, conn);
            try
            { 
            removeBookCommand.ExecuteNonQuery();
            }
            catch(SqlException ex)
            {
                Debug.WriteLine("sql exception");
            }
    string bookSelectQuery = "select * from Book where publisher_id = " + dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value.ToString();

            SqlDataAdapter dataAdapterPublisher = new SqlDataAdapter(bookSelectQuery, conn);
            dataAdapterPublisher.Fill(dataSet, "Book");
            dataGridView2.DataSource = dataSet.Tables["Book"];
            conn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            conn.Open();
            DataSet dataSet = new DataSet();
            string updateBookQuery = "update Book set name ='" +
                dataGridView2.Rows[dataGridView2.SelectedCells[0].OwningRow.Index].Cells[1].Value + "', year_published = " +
                dataGridView2.Rows[dataGridView2.SelectedCells[0].OwningRow.Index].Cells[2].Value + ", publisher_id = " + 
                dataGridView2.Rows[dataGridView2.SelectedCells[0].OwningRow.Index].Cells[3].Value + " where book_id = " + dataGridView2.SelectedCells[0].OwningRow.Cells[0].Value.ToString();
            SqlCommand updatePublisherCommand = new SqlCommand(updateBookQuery, conn);
            try
            {
                updatePublisherCommand.ExecuteNonQuery();
            }
            catch(SqlException ex)
            {
                Debug.WriteLine("sql exception");
            }
            string bookSelectQuery = "select * from Book where publisher_id = " + dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value.ToString();

            SqlDataAdapter dataAdapterPublisher = new SqlDataAdapter(bookSelectQuery, conn);
            dataAdapterPublisher.Fill(dataSet, "Book");
            dataGridView2.DataSource = dataSet.Tables["Book"];
            conn.Close();
        }
    }
}
