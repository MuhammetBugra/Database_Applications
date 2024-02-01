using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bookShopping
{
    public partial class SignupPage : Form
    {
        // Variables required to receive database queries
        MySqlCommand command;
        MySqlConnection connection = new MySqlConnection("Data Source = localhost; Initial Catalog = databaseApp; User ID = root; Password = Aa1EMRE.");
        MySqlDataReader reader;

        private List<int> cart = new List<int>(); // create cart for users

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
        public SignupPage()
        {
            sqlConnect();
            InitializeComponent();
        }
        private void Form4_Load(object sender, EventArgs e)
        {
            textBox6.PasswordChar = '*'; // replace password char to *
            textBox7.PasswordChar = '*';
        }

        // insert functions
        public async Task addUser(TextBox textBox, TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, TextBox textBox5, TextBox textBox6, ComboBox comboBox, int goPage)
        { // insert new user
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/"); // Set the base address for HTTP requests

                var userData = new
                { // Prepare the data to be sent in the request
                    username = textBox.Text,
                    first_name = textBox1.Text,
                    last_name = textBox2.Text,
                    gender = string.IsNullOrWhiteSpace(comboBox.Text) ? null : comboBox.Text,
                    email = textBox3.Text,
                    phone = string.IsNullOrWhiteSpace(textBox4.Text) ? null : textBox4.Text,
                    password = textBox5.Text, // Assuming you have a password field
                    passwordConfirm = textBox6.Text // Assuming you have a password confirm field
                };

                var content = new StringContent(JsonConvert.SerializeObject(userData), Encoding.UTF8, "application/json"); // Serialize the data to JSON

                try
                { // Send a POST request to the API
                    var response = await client.PostAsync("api/v1/users/signup", content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    { // Parse the response and write the token and ID to a file
                        var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        File.WriteAllText("userDetails.txt", $"Token: {result.token}\nID: {result.id}");
                        MessageBox.Show("Registration successful!");

                        if (goPage == 1)
                        {
                            HomePage form1 = new HomePage(Convert.ToInt32(result.id), cart);
                            form1.Show();
                            this.Hide();
                        }
                    }
                    else
                    { // Log the error message
                        var errorResult = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        Console.WriteLine($"Error: {errorResult.message}");
                        MessageBox.Show($"An error occurred: {errorResult.message}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
        private void addCustomer(int userId, int goPage)
        { // insert function for customer table
            command = new MySqlCommand("INSERT INTO customer (id) " +
                                       "VALUES (@userID); ", connection);

            using (command)
            {
                command.Parameters.AddWithValue("@userID", userId);

                try
                {
                    command.ExecuteNonQuery();

                    if (goPage == 1)
                    {
                        HomePage form1 = new HomePage(userId, cart);
                        form1.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
        private async void button1_Click(object sender, EventArgs e)
        { // Calls the required functions for insert
            await addUser(textBox1, textBox2, textBox3, textBox4, textBox5, textBox6, textBox7, comboBox1, 1);
        }

        // form closed
        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        { // close forms
            HomePage form1 = new HomePage(-1, cart);
            form1.Show();
            this.Hide();
        }
    }
}