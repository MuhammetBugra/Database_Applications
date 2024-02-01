using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bookShopping
{
    public partial class CustomerAccount : Form
    {
        // Variables required to receive database queries
        MySqlCommand command;
        MySqlConnection connection = new MySqlConnection("Data Source = localhost; Initial Catalog = databaseApp; User ID = root; Password = Aa1EMRE.");
        MySqlDataAdapter adapter;
        MySqlDataReader reader;
        DataSet dataSet;

        private Boolean isLinkColumnAdded = false; // control for datagridview link add one times

        private int userID; // userID for accounts

        private List<int> cart; // book cart for purchases

        DialogResult result; // for approve

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
        public CustomerAccount(int userID, List<int> cart)
        {
            sqlConnect();
            InitializeComponent();

            this.userID = userID; // assign incoming values
            this.cart = cart;
        }
        private void Form6_Load(object sender, EventArgs e)
        {
            userInfo(textBox1, textBox2, textBox3, textBox4, textBox5, comboBox1, label40, label41, label46); // calling required functions
            addComboBox();
            seeOrder();

            textBox6.PasswordChar = '*'; // replace password char to *
            textBox7.PasswordChar = '*';
            textBox8.PasswordChar = '*';
        }

        // user delete or update
        public void userInfo(TextBox textBox, TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, ComboBox comboBox, Label label, Label label1, Label label2)
        { // print user info in textboxess
            command = new MySqlCommand("SELECT username, first_name, last_name, gender, email, phone, create_date, last_login_date, c.wallet " +
                                       "FROM users " +
                                       "LEFT JOIN customer c ON c.id = users.id " +
                                       "WHERE users.id = @userID", connection);

            using (command)
            {
                command.Parameters.AddWithValue("@userID", userID);

                using (reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        textBox.Text = reader["username"].ToString();
                        textBox1.Text = reader["first_name"].ToString();
                        textBox2.Text = reader["last_name"].ToString();
                        textBox3.Text = reader["email"].ToString();
                        textBox4.Text = reader["phone"].ToString();
                        comboBox.Text = reader["gender"].ToString();
                        label.Text = reader["create_date"].ToString();
                        label1.Text = reader["last_login_date"].ToString();

                        if (label2 != null) { label2.Text = reader["wallet"].ToString(); }
                    }
                }
            }
        }
        public async Task updateUser(TextBox textBox, TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, ComboBox comboBox,int a)
        { // update user
            if (textBox.Text != "" && textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            { // Check if all required fields are filled
                string token = GetTokenForUser(userID); // Retrieve the authentication token for the user
                if (token == null)
                {
                    MessageBox.Show("Token not found. Please log in.");
                    return;
                }

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:3000/"); // Set the base address for the API

                    var updateData = new
                    { // Prepare the updated user information in JSON format
                        username = textBox.Text,
                        first_name = textBox1.Text,
                        last_name = textBox2.Text,
                        gender = comboBox.Text,
                        email = textBox3.Text,
                        phone = textBox4.Text,
                        is_manager=a
                    };
                    Console.WriteLine("Request Payload: " + JsonConvert.SerializeObject(updateData));

                    var content = new StringContent(JsonConvert.SerializeObject(updateData), Encoding.UTF8, "application/json"); // Convert the data to JSON

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token); // Add the token to the request headers

                    try
                    {
                        var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"api/v1/users/{userID}") // Send a PATCH request
                        {
                            Content = content
                        };

                        var response = await client.SendAsync(request);
                        var responseContent = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            MessageBox.Show("Your user information has been successfully updated.");
                        }
                        else
                        {
                            MessageBox.Show("An error occurred: " + responseContent);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An exceptional error occurred: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please fill in all required fields.");
            }

        }
        private string GetTokenForUser(int userId)
        { // get token function
            string line;
            string token = null;

            using (StreamReader file = new StreamReader("jwt-tokens.txt"))
            { // Open the "jwt-tokens.txt" file for reading
                while ((line = file.ReadLine()) != null)
                { // Read each line in the file
                    if (line.StartsWith("User ID: " + userId))
                    { // Check if the line contains the specified user ID
                        token = line.Split(new string[] { "JWT Token: " }, StringSplitOptions.None).Last().Trim(); // Extract the JWT token from the line
                        break;
                    }
                }
            }

            return token;
        }
        private async void button1_Click(object sender, EventArgs e)
        { // update user informations
            await updateUser(textBox1, textBox2, textBox3, textBox4, textBox5, comboBox1,0);
        }
        public void button2_Click(object sender, EventArgs e)
        { // delete this user all tables with user id 
            result = MessageBox.Show("Are you Sure?", "Approve", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                command = new MySqlCommand("SELECT 1 FROM customer WHERE id = @userID", connection);
                command.Parameters.AddWithValue("@userID", userID);
                object customerExists = command.ExecuteScalar();

                if (customerExists != null)
                {
                    command = new MySqlCommand("DELETE FROM customer WHERE id = @userID", connection);
                    command.Parameters.AddWithValue("@userID", userID);
                    control(command);
                }

                command = new MySqlCommand("SELECT 1 FROM address WHERE user_id = @userID", connection);
                command.Parameters.AddWithValue("@userID", userID);
                object addressExists = command.ExecuteScalar();

                if (addressExists != null)
                {
                    command = new MySqlCommand("DELETE FROM address WHERE user_id = @userID", connection);
                    command.Parameters.AddWithValue("@userID", userID);
                    control(command);
                }

                command = new MySqlCommand("SELECT 1 FROM credit_card WHERE user_id = @userID", connection);
                command.Parameters.AddWithValue("@userID", userID);
                object creditCardExists = command.ExecuteScalar();

                if (creditCardExists != null)
                {
                    command = new MySqlCommand("DELETE FROM credit_card WHERE user_id = @userID", connection);
                    command.Parameters.AddWithValue("@userID", userID);
                    control(command);
                }

                command = new MySqlCommand("SELECT 1 FROM `order` WHERE customer_id = @userID", connection);
                command.Parameters.AddWithValue("@userID", userID);
                object orderExists = command.ExecuteScalar();

                if (orderExists != null)
                {
                    command = new MySqlCommand("DELETE FROM `order` WHERE customer_id = @userID", connection);
                    command.Parameters.AddWithValue("@userID", userID);
                    control(command);
                }

                command = new MySqlCommand("DELETE FROM users WHERE id = @userID", connection);
                command.Parameters.AddWithValue("@userID", userID);
                control(command);

                HomePage form1 = new HomePage(-1, null);
                form1.Show();
                this.Hide();
            }
        }

        // change password
        public async Task changePass(TextBox textBox, TextBox textBox1, TextBox textBox2)
        {
            if (textBox1.Text != textBox2.Text)
            { // Check if the new passwords match
                MessageBox.Show("New passwords do not match! Please try again.");
                return;
            }

            string token = GetTokenForUser(userID); // Retrieve the authentication token for the user
            if (token == null)
            {
                MessageBox.Show("Token not found. Please log in.");
                return;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/"); // Set the base address for the API

                var passwordData = new
                { // Prepare the passwords in JSON format
                    currentPassword = textBox.Text,
                    newPassword = textBox1.Text
                };

                var content = new StringContent(JsonConvert.SerializeObject(passwordData), Encoding.UTF8, "application/json"); // Convert the data to JSON

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token); // Add the token to the request headers

                try
                {
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), "api/v1/users/updatePass") // Send a PATCH request to update the password
                    {
                        Content = content
                    };

                    var response = await client.SendAsync(request);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Your password has been successfully updated.");

                        clearTextBox(textBox, textBox1, textBox2, null, null, null);
                    }
                    else
                    {
                        MessageBox.Show("An error occurred: " + responseContent);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An exceptional error occurred: " + ex.Message);
                }
            }
        }
        private async void button3_Click(object sender, EventArgs e)
        { // change password
            await changePass(textBox8, textBox7, textBox6);
        }

        // add combobox values
        private void addComboBox()
        {
            comboBox4.Items.Clear(); // clears comboboxes
            comboBox3.Items.Clear();
            comboBox2.Items.Clear();

            command = new MySqlCommand("SELECT COUNT(*) AS 'Count' FROM address WHERE user_id = @userID", connection);
            command.Parameters.AddWithValue("@userID", userID);
            AddItemsToComboBox(command, comboBox2);

            command = new MySqlCommand("SELECT COUNT(*) AS 'Count' FROM credit_card WHERE user_id = @userID", connection);
            command.Parameters.AddWithValue("@userID", userID);
            AddItemsToComboBox(command, comboBox3);
            AddItemsToComboBox(command, comboBox4);
        }
        public void AddItemsToComboBox(MySqlCommand command, ComboBox comboBox)
        { // add items in combobox -> 1 .. n
            using (command)
            {
                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int count = reader.GetInt32("Count");
                        for (int i = 1; i <= count; i++)
                        {
                            comboBox.Items.Add(i);
                        }
                    }
                }
            }
        }

        // print card and address informations
        public void cardAddressInfo(TextBox textBox, TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, DateTimePicker dateTimePicker, Label label, MySqlCommand command)
        { // print function
            using (command)
            {
                command.Parameters.AddWithValue("@userID", userID);

                using (reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        label.Text = reader[0].ToString();
                        textBox.Text = reader[1].ToString();
                        textBox1.Text = reader[2].ToString();
                        textBox2.Text = reader[3].ToString();
                        textBox3.Text = reader[4].ToString();
                        if (textBox4  != null) { textBox4.Text = reader[5].ToString(); }
                        if (dateTimePicker != null) { dateTimePicker.Value = Convert.ToDateTime(reader[5]); }
                    }
                }
            }
        }
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
                cardAddressInfo(textBox13, textBox12, textBox11, textBox10, textBox9, null, label44, command);
            }
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        { // It is sent to the print function according to the id and command selected in the combobox.
            command = new MySqlCommand("SELECT id, title, cvc, number, credit_card_name, expiration_date FROM credit_card WHERE user_id = @userID LIMIT 1 OFFSET @cardID", connection);
            if (comboBox3.SelectedItem != null)
            {
                int cardID = comboBox3.SelectedIndex + 1;
                command.Parameters.AddWithValue("@cardID", cardID - 1);
                cardAddressInfo(textBox23, textBox22, textBox21, textBox20, null, dateTimePicker1, label43, command);
            }
        }

        // add, update, delete address
        public void addUpdateAddress(TextBox textBox, TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, MySqlCommand command)
        {
            if (textBox.Text != "" && textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            { // control textboxes
                using (MySqlCommand commandCity = new MySqlCommand("SELECT id FROM Cities WHERE city_name = @cityName", connection))
                { // control city name
                    commandCity.Parameters.Clear();
                    commandCity.Parameters.AddWithValue("@cityName", textBox.Text);
                    object cityId = commandCity.ExecuteScalar();

                    if (cityId == null)
                    { // If there is no selected city, it adds it to the table.
                        commandCity.CommandText = "INSERT INTO Cities (city_name) VALUES (@cityName); SELECT LAST_INSERT_ID();";
                        cityId = commandCity.ExecuteScalar();
                    }

                    using (MySqlCommand commandTown = new MySqlCommand("SELECT id FROM Towns WHERE town_name = @townName", connection))
                    { // control town name
                        commandTown.Parameters.Clear();
                        commandTown.Parameters.AddWithValue("@townName", textBox1.Text);
                        object townId = commandTown.ExecuteScalar();

                        if (townId == null)
                        { // If there is no selected town, it adds it to the table.
                            commandTown.CommandText = "INSERT INTO Towns (city_id, town_name) VALUES (@cityId, @townName); SELECT LAST_INSERT_ID();";
                            commandTown.Parameters.AddWithValue("@cityId", cityId);
                            townId = commandTown.ExecuteScalar();
                        }

                        using (MySqlCommand commandDistrict = new MySqlCommand("SELECT id FROM Districts WHERE district_name = @districtName", connection))
                        { // control district name
                            commandDistrict.Parameters.Clear();
                            commandDistrict.Parameters.AddWithValue("@districtName", textBox2.Text);
                            object districtId = commandDistrict.ExecuteScalar();

                            if (districtId == null)
                            { // If there is no selected district, it adds it to the table.
                                commandDistrict.CommandText = "INSERT INTO Districts (town_id, district_name) VALUES (@townId, @districtName); SELECT LAST_INSERT_ID();";
                                commandDistrict.Parameters.AddWithValue("@townId", townId);
                                districtId = commandDistrict.ExecuteScalar();
                            }

                            using (command)
                            { // updating or inserting the table
                                command.Parameters.AddWithValue("@city", cityId);
                                command.Parameters.AddWithValue("@town", townId);
                                command.Parameters.AddWithValue("@district", districtId);
                                command.Parameters.AddWithValue("@postalCode", textBox3.Text);
                                command.Parameters.AddWithValue("@addressText", textBox4.Text);
                                command.Parameters.AddWithValue("@addressID", label44.Text);
                                command.Parameters.AddWithValue("@userID", userID);

                                control(command);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Fill in the required fields.");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        { // update address
            command = new MySqlCommand("UPDATE Address " +
                                       "SET city_id = @city, " +
                                       "town_id = @town, " +
                                       "district_id = @district, " +
                                       "postal_code = @postalCode, " +
                                       "address_text = @addressText " +
                                       "WHERE id = @addressID", connection);

            addUpdateAddress(textBox13, textBox12, textBox11, textBox10, textBox9, command);
        }
        private void button5_Click(object sender, EventArgs e)
        { // delete address
            result = MessageBox.Show("Are you Sure?", "Approve", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes) 
            { // approve button
                command = new MySqlCommand("DELETE FROM address WHERE id = @addressID", connection);
                command.Parameters.AddWithValue("@addressID", label44.Text);

                control(command);

                addComboBox();

                clearTextBox(textBox13, textBox12, textBox11, textBox10, textBox9, label44);
            }
        }
        private void button6_Click(object sender, EventArgs e)
        { // insert method
            command = new MySqlCommand("INSERT INTO Address (user_id, city_id, town_id, district_id, postal_code, address_text) " +
                                       "VALUES (@userID, @city, @town, @district, @postalCode, @addressText)", connection);

            addUpdateAddress(textBox18, textBox17, textBox16, textBox15, textBox14, command);

            addComboBox();

            clearTextBox(textBox18, textBox17, textBox16, textBox15, textBox14, null);
        }

        // add, update, delete card
        public void addUpdateCard(TextBox textBox, TextBox textBox1, TextBox textBox2, TextBox textBox3, DateTimePicker dateTimePicker, MySqlCommand command)
        {
            if (!string.IsNullOrWhiteSpace(textBox.Text) &&
                !string.IsNullOrWhiteSpace(textBox1.Text) &&
                !string.IsNullOrWhiteSpace(textBox2.Text) &&
                !string.IsNullOrWhiteSpace(textBox3.Text))
            { // control textboxes
                using (command)
                {  // updating or inserting the table
                    command.Parameters.AddWithValue("@cardID", label43.Text);
                    command.Parameters.AddWithValue("@title", textBox.Text);
                    command.Parameters.AddWithValue("@cvc", textBox1.Text);
                    command.Parameters.AddWithValue("@number", textBox2.Text);
                    command.Parameters.AddWithValue("@credit_card_name", textBox3.Text);
                    command.Parameters.AddWithValue("@expiration_date", dateTimePicker.Value);
                    command.Parameters.AddWithValue("@userID", userID);

                    control(command);
                }
            }
            else
            {
                MessageBox.Show("Fill in the required fields.");
            }
        }
        private void button7_Click(object sender, EventArgs e)
        { // delete card
            result = MessageBox.Show("Are you Sure?", "Approve", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                command = new MySqlCommand("DELETE FROM credit_card WHERE id = @cardID", connection);
                command.Parameters.AddWithValue("@cardID", label43.Text);

                control(command);

                addComboBox();

                clearTextBox(textBox23, textBox22, textBox21, textBox20, null, label43);
            }
        }
        private void button8_Click(object sender, EventArgs e)
        { // update card
            command = new MySqlCommand("UPDATE credit_card " +
                                       "SET title = @title, " +
                                       "cvc = @cvc, " +
                                       "number = @number, " +
                                       "credit_card_name = @credit_card_name, " +
                                       "expiration_date = @expiration_date " +
                                       "WHERE id = @cardID ", connection);

            addUpdateCard(textBox23, textBox22, textBox21, textBox20, dateTimePicker1, command);
        }
        private void button9_Click(object sender, EventArgs e)
        { // insert new card
            command = new MySqlCommand("INSERT INTO credit_card (user_id, title, cvc, number, credit_card_name, expiration_date) " +
                                       "VALUES (@userID, @title, @cvc, @number, @credit_card_name, @expiration_date)", connection);

            addUpdateCard(textBox28, textBox27, textBox26, textBox25, dateTimePicker2, command);

            addComboBox();

            clearTextBox(textBox28, textBox27, textBox26, textBox25, null, null);
        }

        // control and clear method
        public void control(MySqlCommand command)
        { // control success or fail
            using (command)
            {
                try
                {
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Successfully!");
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
        public void clearTextBox(TextBox textBox, TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, Label label)
        { // clear these textbox
            if (textBox != null) { textBox.Text = ""; }
            if (textBox1 != null) { textBox1.Text = ""; }
            if (textBox2 != null) { textBox2.Text = ""; }
            if (textBox3 != null) { textBox3.Text = ""; }
            if (textBox4 != null) { textBox4.Text = ""; }
            if (label != null) { label.Text = ""; }
        }
        
        // add money for wallet
        private void button10_Click(object sender, EventArgs e)
        { // add money in account wallet
            if (comboBox4.Text != "")
            {
                command = new MySqlCommand("UPDATE customer " +
                                           "SET wallet = @wallet " +
                                           "WHERE id = @userID", connection);

                using (command)
                {
                    if (decimal.TryParse(label46.Text, out decimal money))
                    {
                        money = Convert.ToDecimal(label46.Text); 
                    }

                    if (!string.IsNullOrEmpty(textBox29.Text) && decimal.TryParse(textBox29.Text, out decimal addMoney))
                    {
                        money += addMoney;
                    }
                    command.Parameters.AddWithValue("@userID", userID);
                    command.Parameters.AddWithValue("@wallet", money); // update wallet

                    label46.Text = money.ToString();

                    control(command);
                }
            }
            else
            {
                MessageBox.Show("Please choose credit card.");
            }
        }

        // see order information
        public void listedGridViewItems(MySqlCommand command, DataGridView dataGrid)
        { // listed data grid view
            using (command)
            {
                using (adapter = new MySqlDataAdapter(command))
                {
                    dataSet = new DataSet();
                    adapter.Fill(dataSet, "orderInfo");

                    dataGrid.DataSource = dataSet.Tables["orderInfo"];
                    dataGrid.Columns["id"].Visible = false; // Hides the id column

                    if (isLinkColumnAdded == false)
                    { // add link for see books
                        DataGridViewLinkColumn linkColumn = new DataGridViewLinkColumn();
                        linkColumn.Name = "See Order";
                        linkColumn.UseColumnTextForLinkValue = true;
                        linkColumn.Text = "See";
                        dataGrid.Columns.Add(linkColumn);

                        isLinkColumnAdded = true;
                    }
                }
            }
        }
        private void seeOrder()
        { // see old orders
            command = new MySqlCommand("SELECT o.id AS 'id', o.order_date AS 'Order Date', o.total_amount AS 'Total Amount', p.method_name AS 'Method Name', s.status_name AS 'Order Status' " +
                                       "FROM `order` o " +
                                       "LEFT JOIN paymentmethod p ON p.id = o.payment_method " +
                                       "LEFT JOIN orderstatus s ON s.id = o.order_status " +
                                       "WHERE o.customer_id = @userID", connection);

            command.Parameters.AddWithValue("@userID", userID);

            listedGridViewItems(command, dataGridView2);
        }
        private void seeBook(int orderID)
        { // see book
            command = new MySqlCommand("SELECT b.id, b.title AS 'Book Title', o.quantity AS 'Quantity' " +
                                       "FROM book b " +
                                       "LEFT JOIN orderdetails o ON b.id = o.book_id " +
                                       "WHERE o.order_id = @orderID", connection);

            command.Parameters.AddWithValue("@orderID", orderID);

            listedGridViewItems(command, dataGridView1);
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { // for see ordered books
            if (e.ColumnIndex == dataGridView2.Columns["See Order"].Index && e.RowIndex >= 0)
            {
                int orderID = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["id"].Value);

                seeBook(orderID);
            }
        }

        // form closed
        private void Form6_FormClosed(object sender, FormClosedEventArgs e)
        { // close forms
            HomePage form1 = new HomePage(userID, cart);
            form1.Show();
            this.Hide();
        }
    }
}