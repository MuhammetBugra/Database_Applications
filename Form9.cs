using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace bookShopping
{
    public partial class Form9 : Form
    {
        // Variables required to receive database queries
        MySqlCommand command;
        MySqlConnection connection = new MySqlConnection("Data Source = localhost; Initial Catalog = databaseApp; User ID = root; Password = Aa1EMRE.");
        MySqlDataReader reader;

        private int userID; // userID for accounts
        private decimal price; // price for shopping

        private List<int> cart; // book cart for purchases

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
        public Form9(int userID, List<int> cart, decimal price)
        {
            sqlConnect();
            InitializeComponent();

            this.userID = userID; // assign incoming values
            this.cart = cart;
            this.price = price;
        }
        private void Form9_Load(object sender, EventArgs e)
        {
            addComboBox(); // calling required functions
            printWallet();
        }

        // print wallet
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
                        label46.Text = reader["wallet"].ToString();
                    }
                }
            }

            label2.Text = price.ToString();
        }

        // add combobox values
        private void addComboBox()
        {
            CustomerAccount form6 = new CustomerAccount(userID, cart);

            comboBox3.Items.Clear(); // clears comboboxes
            comboBox2.Items.Clear();

            command = new MySqlCommand("SELECT COUNT(*) AS 'Count' FROM address WHERE user_id = @userID", connection);
            command.Parameters.AddWithValue("@userID", userID);
            form6.AddItemsToComboBox(command, comboBox2);

            command = new MySqlCommand("SELECT COUNT(*) AS 'Count' FROM credit_card WHERE user_id = @userID", connection);
            command.Parameters.AddWithValue("@userID", userID);
            form6.AddItemsToComboBox(command, comboBox3);
        }

        // print card and address informations
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        { // It is sent to the print function according to the id and command selected in the combobox.
            command = new MySqlCommand("SELECT a.id, c.city_name AS 'City', t.town_name AS 'Town', d.district_name AS 'District', a.postal_code AS 'Postal', a.address_text AS 'AddressText' " +
                                       "FROM address a " +
                                       "LEFT JOIN cities c ON c.id = a.city_id " +
                                       "LEFT JOIN towns t ON t.id = a.town_id " +
                                       "LEFT JOIN districts d ON d.id = a.district_id " +
                                       "WHERE a.user_id = @userID LIMIT 1 OFFSET @addressID", connection);

            if (comboBox2.SelectedItem != null)
            {
                int addressID = comboBox2.SelectedIndex + 1;
                command.Parameters.AddWithValue("@addressID", addressID - 1);
                CustomerAccount form6 = new CustomerAccount(userID, cart);
                form6.cardAddressInfo(textBox13, textBox12, textBox11, textBox10, textBox9, null, label44, command);
            }
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        { // It is sent to the print function according to the id and command selected in the combobox.
            command = new MySqlCommand("SELECT id, title, cvc, number, credit_card_name, expiration_date FROM credit_card WHERE user_id = @userID LIMIT 1 OFFSET @cardID", connection);
            if (comboBox3.SelectedItem != null)
            {
                int cardID = comboBox3.SelectedIndex + 1;
                command.Parameters.AddWithValue("@cardID", cardID - 1);
                CustomerAccount form6 = new CustomerAccount(userID, cart);
                form6.cardAddressInfo(textBox23, textBox22, textBox21, textBox20, null, dateTimePicker2, label43, command);
            }
        }

        // add address
        private void button6_Click(object sender, EventArgs e)
        { // insert method
            CustomerAccount form6 = new CustomerAccount(userID, cart);

            command = new MySqlCommand("INSERT INTO Address (user_id, city_id, town_id, district_id, postal_code, address_text) " +
                                       "VALUES (@userID, @city, @town, @district, @postalCode, @addressText)", connection);

            form6.addUpdateAddress(textBox18, textBox17, textBox16, textBox15, textBox14, command);

            addComboBox();

            form6.clearTextBox(textBox18, textBox17, textBox16, textBox15, textBox14, null);
        }

        // add card
        private void button9_Click(object sender, EventArgs e)
        { // insert new card
            CustomerAccount form6 = new CustomerAccount(userID, cart);

            command = new MySqlCommand("INSERT INTO credit_card (user_id, title, cvc, number, credit_card_name, expiration_date) " +
                                       "VALUES (@userID, @title, @cvc, @number, @credit_card_name, @expiration_date)", connection);

            form6.addUpdateCard(textBox28, textBox27, textBox26, textBox25, dateTimePicker1, command);

            addComboBox();

            form6.clearTextBox(textBox28, textBox27, textBox26, textBox25, null, null);
        }

        // complete shopping
        private decimal checkPrice(int bookID)
        { // check all book price for add table
            decimal bookPrice = 0;

            MySqlCommand priceCommand = new MySqlCommand("SELECT id, price FROM book WHERE id = @bookID", connection);

            using (priceCommand)
            {
                priceCommand.Parameters.AddWithValue("@bookID", bookID);

                using (reader = priceCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bookPrice = Convert.ToDecimal(reader["price"]);
                    }
                }
            }
            return bookPrice;
        }
        private void insertOrderDetails(int order_id)
        { // insert information to order details table
            MySqlCommand ordDetCommand = new MySqlCommand("INSERT INTO orderdetails (order_id, book_id, quantity, unit_price) " +
                                                     "VALUES (@order_id, @book_id, @quantity, @unit_price)", connection);

            using (ordDetCommand)
            {
                foreach (int bookId in cart.Distinct())
                {
                    int quantity = cart.Count(x => x == bookId);

                    ordDetCommand.Parameters.Clear();
                    ordDetCommand.Parameters.AddWithValue("@order_id", order_id);
                    ordDetCommand.Parameters.AddWithValue("@book_id", bookId);
                    ordDetCommand.Parameters.AddWithValue("@quantity", quantity);

                    decimal bookPrice = checkPrice(bookId);
                    ordDetCommand.Parameters.AddWithValue("@unit_price", bookPrice);

                    try
                    {
                        int rowsAffected = ordDetCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Your order has started to be prepared.");
                        }
                        else
                        {
                            MessageBox.Show("Failed!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }
        private void insertOrder(int method)
        { // insert information to order table
            int order_id = -1;

            command = new MySqlCommand("INSERT INTO `order` (customer_id, order_date, total_amount, payment_method, shipping_address) " +
                                       "VALUES (@customer_id, NOW(), @total_amount, @payment_method, @shipping_address)", connection);

            using (command)
            {
                command.Parameters.AddWithValue("@customer_id", userID);
                command.Parameters.AddWithValue("@total_amount", price);
                command.Parameters.AddWithValue("@payment_method", method); 
                command.Parameters.AddWithValue("@shipping_address", label44.Text);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Successfully!");

                        using (MySqlCommand orderID = new MySqlCommand("SELECT LAST_INSERT_ID()", connection))
                        {
                            order_id = Convert.ToInt32(orderID.ExecuteScalar());
                        }
                        if (method == 1)
                        {
                            updateWallet();
                        }
                        insertOrderDetails(order_id); // call necessary function for sql tables
                    }
                    else
                    {
                        MessageBox.Show("Failed!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        { // wallet shopping
            if (price <= Convert.ToDecimal(label46.Text) && comboBox2.Text != "")
            {
                insertOrder(1);

                HomePage form1 = new HomePage(userID, cart = new List<int>());
                form1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Insufficient balance or you did not choose an address.");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        { // credit card shopping
            if (comboBox3.Text != "" && comboBox2.Text != "")
            {
                insertOrder(2);

                HomePage form1 = new HomePage(userID, cart = new List<int>());
                form1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Please select card and address.");
            }
        }

        // update after shopping
        private void updateWallet()
        { // update account wallet
            MySqlCommand walletCommand = new MySqlCommand("UPDATE customer SET wallet = @wallet WHERE id = @userID", connection);

            using (walletCommand)
            {
                walletCommand.Parameters.AddWithValue("@userID", userID);
                walletCommand.Parameters.AddWithValue("@wallet", Convert.ToDecimal(label46.Text) - price);

                CustomerAccount form6 = new CustomerAccount(userID, cart);
                form6.control(walletCommand);
            }
        }

        // form closed
        private void Form9_FormClosed(object sender, FormClosedEventArgs e)
        { // close forms
            Form5 form5 = new Form5(userID, cart);
            form5.Show();
            this.Hide();
        }
    }
}