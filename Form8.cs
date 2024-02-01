using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace bookShopping
{
    public partial class Form8 : Form
    {
        // receive database queries
        MySqlConnection connection = new MySqlConnection("Data Source = localhost; Initial Catalog = databaseApp; User ID = root; Password = Aa1EMRE.");

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
        public Form8()
        {
            sqlConnect();
            InitializeComponent();
        }
        private void Form8_Load(object sender, EventArgs e)
        {
            textBox6.PasswordChar = '*'; // replace password char to *
            textBox7.PasswordChar = '*';
        }

        // for reset password
        private async void button1_Click(object sender, EventArgs e)
        { // Reset password
            string newPassword = textBox6.Text;
            string confirmPassword = textBox7.Text;

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match!");
                return;
            }

            // You need to determine how to obtain the token here
            string resetToken = textBox1.Text; // Place the password reset token here

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/"); // Set the base address for the API

                var resetData = new
                { // Prepare the password and token in JSON format
                    token = resetToken,
                    newPassword = newPassword
                };

                var content = new StringContent(JsonConvert.SerializeObject(resetData), Encoding.UTF8, "application/json"); // Convert the data to JSON

                try
                {
                    var response = await client.PostAsync("api/v1/users/resetPass", content); // Send a POST request
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Your password has been successfully reset.");

                        loginPage form3 = new loginPage();
                        form3.Show();
                        this.Hide();
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

        // send email for reset password
        private async void button2_Click(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/"); // Set the base address for the API

                var emailData = new
                { // Prepare the email address in JSON format
                    email = textBox4.Text
                };

                var content = new StringContent(JsonConvert.SerializeObject(emailData), Encoding.UTF8, "application/json"); // Convert the data to JSON

                try
                {
                    var response = await client.PostAsync("api/v1/users/forgetPass", content); // Send a POST request
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("A password reset token has been sent to your email address.");

                        textBox6.Visible = true; // Make the necessary UI elements visible for password reset
                        textBox7.Visible = true;
                        textBox1.Visible = true;
                        label1.Visible = true;
                        label7.Visible = true;
                        label8.Visible = true;
                        button1.Visible = true;

                        button2.Visible = false; // Hide unnecessary UI elements
                        textBox4.Visible = false;
                        label5.Visible = false;
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

        // form closed
        private void Form8_FormClosed(object sender, FormClosedEventArgs e)
        { // close forms
            loginPage form3 = new loginPage();
            form3.Show();
            this.Hide();
        }
    }
}