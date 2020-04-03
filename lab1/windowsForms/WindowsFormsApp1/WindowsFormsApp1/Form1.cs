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
        private MyQuery queryData;
        public Form1(MyQuery query)
        {
            queryData = query;
            InitializeComponent();
            FillData();
            label1.Text = query.parent;
            label2.Text = query.child;
        }

        private void FillData()
        {
            string connString = "Data Source=DESKTOP-VP9PPF3; Initial catalog = Theologians; integrated security=true";
            conn = new SqlConnection(connString);
            conn.Open();
            SqlDataAdapter dataAdapterPublisher = new SqlDataAdapter(queryData.select_parent, conn);
            DataSet dataSet = new DataSet();
            dataAdapterPublisher.Fill(dataSet, queryData.parent);
            dataGridView1.DataSource = dataSet.Tables[queryData.parent];
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            DataSet dataSet = new DataSet();

            SqlCommand command = new SqlCommand(null, conn);
            command.CommandText = queryData.select_children;
            SqlParameter parentId = new SqlParameter("@parent_id", SqlDbType.Int, 0);
            parentId.Value = Int32.Parse(dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            command.Parameters.Add(parentId);
            command.Prepare();

            SqlDataAdapter dataAdapterBook = new SqlDataAdapter(command);
            dataAdapterBook.Fill(dataSet, queryData.child);
            dataGridView2.DataSource = dataSet.Tables[queryData.child];
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            conn.Open();
            DataSet dataSet = new DataSet();
            SqlCommand command = new SqlCommand(null, conn);
            command.CommandText = queryData.add_children;
            for (int i = 1; i < dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells.Count - 1; i++)
            {
                command.Parameters.AddWithValue("@param" + i.ToString(), dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells[i].Value);
            }
            int colNumber = dataGridView2.Rows[dataGridView2.Rows.Count - 2].Cells.Count - 1;
            Console.WriteLine("@param" + colNumber.ToString());
            Console.WriteLine(dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value);
            command.Parameters.AddWithValue("@param" + colNumber.ToString(), dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("sql exception");
            }

            SqlCommand selectCommand = new SqlCommand(null, conn);
            selectCommand.CommandText = queryData.select_children;
            SqlParameter parentId = new SqlParameter("@parent_id", SqlDbType.Int, 0);
            parentId.Value = Int32.Parse(dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            selectCommand.Parameters.Add(parentId);
            selectCommand.Prepare();

            SqlDataAdapter dataAdapterBook = new SqlDataAdapter(selectCommand);
            dataAdapterBook.Fill(dataSet, queryData.child);
            dataGridView2.DataSource = dataSet.Tables[queryData.child];
            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            DataSet dataSet = new DataSet();

            SqlCommand command = new SqlCommand(null, conn);
            command.CommandText = queryData.remove_children;
            SqlParameter id = new SqlParameter("@id", SqlDbType.Int, 0);
            id.Value = Int32.Parse(dataGridView2.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            command.Parameters.Add(id);
            command.Prepare();
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("sql exception");
            }
            SqlCommand selectCommand = new SqlCommand(null, conn);
            selectCommand.CommandText = queryData.select_children;
            SqlParameter parentId = new SqlParameter("@parent_id", SqlDbType.Int, 0);
            parentId.Value = Int32.Parse(dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            selectCommand.Parameters.Add(parentId);
            selectCommand.Prepare();

            SqlDataAdapter dataAdapterBook = new SqlDataAdapter(selectCommand);
            dataAdapterBook.Fill(dataSet, queryData.child);
            dataGridView2.DataSource = dataSet.Tables[queryData.child];
            conn.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            conn.Open();
            DataSet dataSet = new DataSet();
            SqlCommand command = new SqlCommand(null, conn);
            command.CommandText = queryData.update_children;
            for (int i = 1; i < dataGridView2.Rows[dataGridView2.SelectedCells[0].OwningRow.Index].Cells.Count; i++)
            {
                command.Parameters.AddWithValue("@param" + i.ToString(), dataGridView2.Rows[dataGridView2.SelectedCells[0].OwningRow.Index].Cells[i].Value);
            }
            int colNumber = dataGridView2.Rows[dataGridView2.SelectedCells[0].OwningRow.Index].Cells.Count;
            command.Parameters.AddWithValue("@param" + colNumber.ToString(), dataGridView2.SelectedCells[0].OwningRow.Cells[0].Value);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Debug.WriteLine("sql exception");
            }
            SqlCommand selectCommand = new SqlCommand(null, conn);
            selectCommand.CommandText = queryData.select_children;
            SqlParameter parentId = new SqlParameter("@parent_id", SqlDbType.Int, 0);
            parentId.Value = Int32.Parse(dataGridView1.SelectedCells[0].OwningRow.Cells[0].Value.ToString());
            selectCommand.Parameters.Add(parentId);
            selectCommand.Prepare();

            SqlDataAdapter dataAdapterBook = new SqlDataAdapter(selectCommand);
            dataAdapterBook.Fill(dataSet, queryData.child);
            dataGridView2.DataSource = dataSet.Tables[queryData.child];
            conn.Close();
        }
    }
}
