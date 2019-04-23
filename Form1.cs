using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        string connectionString = @"Server=localhost;Database=applicant_test;Uid=root;Pwd=Password#13;";
        //if userID is 0 it will insert a new record if not it will update corresponding record
        int userID = 0;
        int lineItemID = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("UserAddOrEdit", mysqlCon);
                MySqlCommand mySqlCmd2 = new MySqlCommand("LineItemsViewAll", mysqlCon);

                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd2.CommandType = CommandType.StoredProcedure;

                //userID is variable parm which captures the data to be saved if 0 and update if not
                mySqlCmd.Parameters.AddWithValue("_UserID", userID);
                mySqlCmd.Parameters.AddWithValue("_UserName", txtUserName.Text.Trim());
                mySqlCmd.Parameters.AddWithValue("_Address", txtAddress.Text.Trim());
                mySqlCmd.ExecuteNonQuery();

                mySqlCmd2.Parameters.AddWithValue("_LineItemID", lineItemID);
                mySqlCmd2.Parameters.AddWithValue("_Description", txtDescription.Text.Trim());
                mySqlCmd2.Parameters.AddWithValue("_Qty", txtQty.Text.Trim());
                mySqlCmd2.Parameters.AddWithValue("_DateAdded", txtDateAdded.Text.Trim());
                mySqlCmd2.ExecuteNonQuery();

                MessageBox.Show("Successfully Submitted");
                Clear();
                ViewGrid();
                ViewGrid2();
            }
        }

        void ViewGrid()
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("UserViewAll",mysqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtblUser = new DataTable();
                sqlDa.Fill(dtblUser);
                dataGridUser.DataSource = dtblUser;
                dataGridUser.Columns[0].Visible = false;
            }
        }

        void ViewGrid2()
        {
            using (MySqlConnection mysqlCon2 = new MySqlConnection(connectionString))
            {
                mysqlCon2.Open();
                MySqlDataAdapter sqlDa2 = new MySqlDataAdapter("LineItemViewAll", mysqlCon2);
                sqlDa2.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtblLine_Item = new DataTable();
                sqlDa2.Fill(dtblLine_Item);
                dataGridLineItem.DataSource = dtblLine_Item;
                dataGridLineItem.Columns[0].Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            ViewGrid();
            ViewGrid2();
        }

        void Clear()
        {
            txtUserName.Text = txtAddress.Text = txtDescription.Text = txtSearch.Text = txtQty.Text = txtDateAdded.Text = "";
            userID = 0;
            lineItemID = 0;
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
        }

        private void dataGridUser_DoubleClick(object sender, EventArgs e)
        {
            if(dataGridUser.CurrentRow.Index != -1)
            {
                txtUserName.Text = dataGridUser.CurrentRow.Cells[1].Value.ToString();
                txtAddress.Text = dataGridUser.CurrentRow.Cells[2].Value.ToString();
                userID = Convert.ToInt32(dataGridUser.CurrentRow.Cells[0].Value.ToString());
                btnSave.Text = "Update";
                btnDelete.Enabled = Enabled;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlDataAdapter sqlDa = new MySqlDataAdapter("UserSearchByValue", mysqlCon);                
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("_SearchValue",txtSearch.Text);
                DataTable dtblUser = new DataTable();
                sqlDa.Fill(dtblUser);
                dataGridUser.DataSource = dtblUser;
                dataGridUser.Columns[0].Visible = false;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (MySqlConnection mysqlCon = new MySqlConnection(connectionString))
            {
                mysqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("UserDeleteByID", mysqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                //userID is variable parm which captures the data to be saved if 0 and update if not
                mySqlCmd.Parameters.AddWithValue("_UserID", userID);               
                mySqlCmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Deleted");
                Clear();
                ViewGrid();
            }
        }

        private void dataGridLineItem_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void dataGridLineItem_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridLineItem.CurrentRow.Index != -1)
            {
                txtQty.Text = dataGridLineItem.CurrentRow.Cells[1].Value.ToString();
                txtDateAdded.Text = dataGridLineItem.CurrentRow.Cells[2].Value.ToString();
                lineItemID = Convert.ToInt32(dataGridLineItem.CurrentRow.Cells[0].Value.ToString());
                btnSave.Text = "Update";
                btnDelete.Enabled = Enabled;
            }
        }
    }
}

