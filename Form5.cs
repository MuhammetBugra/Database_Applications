using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace bookShopping
{
    public partial class Form5 : Form
    {
        // Variables required to receive database queries
        MySqlCommand command;
        MySqlConnection connection = new MySqlConnection("Data Source = localhost; Initial Catalog = databaseApp; User ID = root; Password = Aa1EMRE.");
        MySqlDataAdapter adapter;
        MySqlDataReader reader;
        DataSet dataSet;

        private List<int> cart; // book cart for purchases

        private int userID; // userID for accounts
        private decimal totalPrice; // price for shopping
        private Boolean isLinkColumnAdded = false; // control for datagridview link add one times
        private Boolean isQuantityColumnAdded = false; // control for datagridview quantity add one times

        // sql connection
        private void sqlConnect()
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connection Successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection Error: " + ex.Message);
            }
        }

        // form functions
        public Form5(int userID, List<int> cart)
        {
            sqlConnect();
            InitializeComponent();

            this.userID = userID; // assign incoming values
            this.cart = cart;
        }
        private void Form5_Load(object sender, EventArgs e)
        { // calling required functions
            listedGridViewItems();
            printWallet();
        }

        // print book information in cart
        private void printWallet() 
        { // print customer wallet 
            command = new MySqlCommand("SELECT u.id, c.wallet " +
                                       "FROM users u " +
                                       "LEFT JOIN customer c ON u.id = c.id " +
                                       "WHERE u.id = @userID", connection);
            using (command)
            {
                command.Parameters.AddWithValue("@userID", userID);

                using (reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        label3.Text = "Wallet: " + reader["wallet"].ToString() + " TL";
                    }
                }
            }
        }
        private void listedGridViewItems()
        {
            if (cart != null && cart.Count > 0)
            {
                command = new MySqlCommand("SELECT b.id, b.title AS 'Title', a.author_name AS 'Author', p.publisher_name AS 'Publisher', b.price AS 'Price', b.image AS 'Image' " +
                                           "FROM Book b " +
                                           "INNER JOIN Author a ON b.author = a.id " +
                                           "INNER JOIN Publisher p ON b.publisher = p.id " +
                                           $"WHERE b.id IN ({string.Join(",", cart)})", connection);

                using (command)
                {
                    using (adapter = new MySqlDataAdapter(command))
                    {
                        dataSet = new DataSet();
                        adapter.Fill(dataSet, "book");

                        dataGridView1.DataSource = dataSet.Tables["book"];
                        dataGridView1.Columns["id"].Visible = false;

                        DataGridViewImageColumn imageColumn = (DataGridViewImageColumn)dataGridView1.Columns["Image"];
                        imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;

                        if (!isLinkColumnAdded)
                        {
                            DataGridViewLinkColumn linkColumn = new DataGridViewLinkColumn();
                            linkColumn.Name = "Delete Book";
                            linkColumn.UseColumnTextForLinkValue = true;
                            linkColumn.Text = "Del";
                            dataGridView1.Columns.Add(linkColumn);

                            isLinkColumnAdded = true;
                        }

                        // Yeni kolonu ekle
                        if (!isQuantityColumnAdded)
                        {
                            DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn();
                            quantityColumn.Name = "Quantity";
                            quantityColumn.HeaderText = "Quantity";
                            dataGridView1.Columns.Add(quantityColumn);

                            isQuantityColumnAdded = true;
                        }

                        totalPrice = 0;

                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            object idCellValue = dataGridView1.Rows[i].Cells["id"].Value; // Add book quantity to 'Quantity' cell of each row
                            if (idCellValue != null && int.TryParse(idCellValue.ToString(), out int bookId))
                            {
                                int bookQuantity = cart.Count(id => id == bookId);
                                dataGridView1.Rows[i].Cells["Quantity"].Value = bookQuantity;

                                object priceCellValue = dataGridView1.Rows[i].Cells["Price"].Value;
                                if (priceCellValue != null && decimal.TryParse(priceCellValue.ToString(), out decimal price))
                                {
                                    totalPrice += price * bookQuantity;
                                }
                            }
                        }


                        label2.Text = "Total Price: " + totalPrice.ToString() + " TL";

                        button1.Enabled = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Your cart is empty.");
            }
        }


        // delete book in cart
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Delete Book"].Index && e.RowIndex >= 0)
            {
                cart.Remove(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value));
            }

            if (cart != null && cart.Count > 0)
            {
                listedGridViewItems(); // It deletes the clicked data according to its id.
            }
            else
            {
                MessageBox.Show("Your cart is empty.");

                Form5_FormClosed(sender, null);
            }
        }

        // for purchase
        private void button1_Click(object sender, EventArgs e)
        { // to complete the purchase
            Form9 form9 = new Form9(userID, cart, totalPrice);
            form9.Show();
            this.Hide();
        }

        // form closed
        private void Form5_FormClosed(object sender, FormClosedEventArgs e)
        { // close forms
            HomePage form1 = new HomePage(userID, cart);
            form1.Show();
            this.Hide();
        }
    }
}