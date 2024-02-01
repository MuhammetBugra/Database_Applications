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
    public partial class loginPage : Form
    {
        // Variables required to receive database queries
        MySqlCommand command;
        MySqlConnection connection = new MySqlConnection("Data Source = localhost; Initial Catalog = databaseApp; User ID = root; Password = Aa1EMRE.");
        MySqlDataReader reader;

        private List<int> cart = new List<int>(); // create book cart for purchases

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
        public loginPage()
        {
            sqlConnect();
            InitializeComponent();
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*'; // replace password char to *
            textBox3.PasswordChar = '*';
        }

        // login, register buttons
        private async void button1_Click(object sender, EventArgs e)
        { // customer page
            await loginUser("api/v1/users/loginUser", textBox1, textBox2);
        }
        private async void button3_Click(object sender, EventArgs e)
        { // manager page
            await loginUser("api/v1/users/loginAdmin", textBox4, textBox3);
        }
        private void button2_Click(object sender, EventArgs e)
        { // open create account
            SignupPage form4 = new SignupPage();
            form4.Show();
            this.Hide();
        }

        // For cases of forgetting your password
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { // go to forget password
            Form8 form8 = new Form8();
            form8.Show();
            this.Hide();
        }

        // login function
        private async Task loginUser(string api, TextBox textBox, TextBox textBox1)
        { // customer and manager login
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/"); // Set the base address for HTTP requests

                var loginData = new
                { // Prepare the login data
                    username = textBox.Text,
                    password = textBox1.Text 
                };

                var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json"); // Serialize the data to JSON

                try
                { // Send a POST request to the API
                    var response = await client.PostAsync(api, content); // manager or customer
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    { // Parse the response and write the token and ID to a file
                        var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        File.WriteAllText("loginDetails.txt", $"Token: {result.token}\nID: {result.id}");
                        MessageBox.Show("Login Successful!");

                        updateLogin(Convert.ToInt32(result.id));
                        
                        HomePage form1;
                        if (Convert.ToByte(result.manager_id) == 0) { form1 = new HomePage(Convert.ToInt32(result.id), cart); }
                        else { form1 = new HomePage(Convert.ToInt32(result.id), null); }

                        form1.Show();
                        this.Hide();
                    }
                    else
                    { // Log the error message
                        var errorResult = JsonConvert.DeserializeObject<dynamic>(responseContent);
                        MessageBox.Show($"An error occurred: {errorResult.message}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
        private void updateLogin(int userID)
        { // replace last login date
            using (MySqlCommand updateCommand = new MySqlCommand("UPDATE users SET last_login_date = NOW() WHERE id = @userId", connection))
            {
                updateCommand.Parameters.AddWithValue("@userId", userID);
                updateCommand.ExecuteNonQuery();
            }
        }

        // form close
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        { // close forms
            HomePage form1 = new HomePage(-1, null);
            form1.Show();
            this.Hide();
        }
    }
}