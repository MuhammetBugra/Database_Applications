using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bookShopping
{
    public partial class ManagerAccount : Form
    {
        // Variables required to receive database queries
        MySqlCommand command;
        MySqlConnection connection = new MySqlConnection("Data Source = localhost; Initial Catalog = databaseApp; User ID = root; Password = Aa1EMRE.");
        MySqlDataReader reader;

        private int userID; // userID for accounts
        private int orderID; // orderID for order

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
        public ManagerAccount(int userID)
        {
            sqlConnect();
            InitializeComponent();

            this.userID = userID; // assign incoming values
        }
        private void Form7_Load(object sender, EventArgs e)
        {
            userInfo();
            addComboBox("SELECT id, title FROM book ORDER BY title", comboBox2);
            addComboBox("SELECT id, username FROM  users ORDER BY username", comboBox3);
            listedGridViewItems();

            textBox6.PasswordChar = '*'; // replace password char to *
            textBox7.PasswordChar = '*';
            textBox8.PasswordChar = '*';
            textBox17.PasswordChar = '*';
            textBox36.PasswordChar = '*';
        }

        // user info and delete or update 
        private void userInfo()
        { // see user info help with form 6 functions
            CustomerAccount form6 = new CustomerAccount(userID, null);
            form6.userInfo(textBox1, textBox2, textBox3, textBox4, textBox5, comboBox1, label40, label41, null);
        }
        private async void button1_Click(object sender, EventArgs e)
        { // update user
            CustomerAccount form6 = new CustomerAccount(userID, null);
            await form6.updateUser(textBox1, textBox2, textBox3, textBox4, textBox5, comboBox1,1);




















        }
        private void button2_Click(object sender, EventArgs e)
        { // delete this user all tables with user id 
            CustomerAccount form6 = new CustomerAccount(userID, null);
            form6.button2_Click(sender, e);
        }

        // change password
        private async void button3_Click(object sender, EventArgs e)
        { // change password
            CustomerAccount form6 = new CustomerAccount(userID, null);
            await form6.changePass(textBox8, textBox7, textBox6);
        }

        // book info

        private async Task LoadBookInfoAsync(int bookID)
        {
            string url = $"http://localhost:3000/api/v1/books/{bookID}";

            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(url);
                    var book = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

                    if (book != null)
                    {
                        label44.Text = book["id"].ToString();
                        textBox13.Text = book["title"].ToString();
                        textBox12.Text = book["author_name"].ToString();
                        textBox11.Text = book["isbn"].ToString();
                        textBox10.Text = book["price"].ToString();
                        textBox18.Text = book["publisher_name"].ToString();
                        textBox16.Text = book["number_of_page"].ToString();
                        textBox15.Text = book["genre_name"].ToString();
                        textBox14.Text = book["language_name"].ToString();
                        textBox9.Text = book["stock_quantity"].ToString();
                        dateTimePicker1.Value = Convert.ToDateTime(book["publication_date"]);
                        richTextBox1.Text = book["description"].ToString();

                        // Resmi Images klasöründen yükle
                        string imagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                        string imageFileName = bookID + ".jpg";
                        string imagePath = Path.Combine(imagesFolderPath, imageFileName);

                        if (File.Exists(imagePath))
                        {
                            pictureBox1.Image = Image.FromFile(imagePath);
                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                        else
                        {
                            pictureBox1.Image = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: book info {ex.Message}");
                }
            }
        }

        private async void bookInfo(int bookID)
        {
            if (bookID != -1)
            {
                await LoadBookInfoAsync(bookID);
            }
            else
            {
                // Clear all fields
                label44.Text = "";
                textBox13.Text = "";
                textBox12.Text = "";
                textBox11.Text = "";
                textBox10.Text = "";
                textBox18.Text = "";
                textBox16.Text = "";
                textBox15.Text = "";
                textBox14.Text = "";
                textBox9.Text = "";
                richTextBox1.Text = "";
                pictureBox1.Image = null;
            }
        }









        //private void bookInfo(int bookID)
        //{ // see book info
        //    if (bookID != -1)
        //    {
        //        command = new MySqlCommand("SELECT b.id, b.title, a.author_name, b.isbn, b.price, b.stock_quantity, p.publisher_name, b.publication_date, b.number_of_page, b.description, g.genre_name, l.language_name, b.image " +
        //                                   "FROM book b " +
        //                                   "INNER JOIN Author a ON b.author = a.id " +
        //                                   "INNER JOIN Publisher p ON b.publisher = p.id " +
        //                                   "INNER JOIN Genre g ON b.genre = g.id " +
        //                                   "INNER JOIN Language l ON b.language = l.id " +
        //                                   "WHERE b.id = @bookID", connection);

        //        using (command)
        //        {
        //            command.Parameters.AddWithValue("@bookID", bookID);

        //            using (reader = command.ExecuteReader())
        //            {
        //                if (reader.Read())
        //                {
        //                    label44.Text = reader["id"].ToString(); // write book informations
        //                    textBox13.Text = reader["title"].ToString();
        //                    textBox12.Text = reader["author_name"].ToString();
        //                    textBox11.Text = reader["isbn"].ToString();
        //                    textBox10.Text = Convert.ToDecimal(reader["price"]).ToString("0.00", CultureInfo.InvariantCulture);
        //                    textBox18.Text = reader["publisher_name"].ToString();
        //                    textBox16.Text = reader["number_of_page"].ToString();
        //                    textBox15.Text = reader["genre_name"].ToString();
        //                    textBox14.Text = reader["language_name"].ToString();
        //                    textBox9.Text = reader["stock_quantity"].ToString();
        //                    dateTimePicker1.Value = Convert.ToDateTime(reader["publication_date"]);
        //                    richTextBox1.Text = reader["description"].ToString();

        //                    byte[] imageData = reader["image"] as byte[]; // add image

        //                    if (imageData != null)
        //                    {
        //                        try
        //                        {
        //                            using (MemoryStream ms = new MemoryStream(imageData))
        //                            {
        //                                pictureBox1.Image = Image.FromStream(ms); // image state
        //                                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        //                            }
        //                        }
        //                        catch (ArgumentException ex)
        //                        {
        //                            MessageBox.Show($"Error displaying image: {ex.Message}");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        pictureBox1.Image = null;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        label44.Text = "";
        //        textBox13.Text = "";
        //        textBox12.Text = "";
        //        textBox11.Text = "";
        //        textBox10.Text = "";
        //        textBox18.Text = "";
        //        textBox16.Text = "";
        //        textBox15.Text = "";
        //        textBox14.Text = "";
        //        textBox9.Text = "";
        //        richTextBox1.Text = "";
        //        pictureBox1.Image = null;
        //    }
        //}
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        { // see book info for book id
            if (comboBox2.SelectedItem != null)
            {
                List<int> bookId = comboBox2.Tag as List<int>;
                bookInfo(bookId[comboBox2.SelectedIndex]);
            }
            else
            {
                bookInfo(-1);
            }
        }
        private void addComboBox(string query, ComboBox comboBox)
        { // add combo box items help with form 1 function
            comboBox.Items.Clear();

            HomePage form1 = new HomePage(userID, null);
            form1.AddItemsToComboBox(query, comboBox);
        }

        // book update, delete, create
        //private void addUpdateBook(TextBox textBox, TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, TextBox textBox5, TextBox textBox6, TextBox textBox7, TextBox textBox8, RichTextBox richTextBox, DateTimePicker dateTimePicker, MySqlCommand command)
        //{ // update or add function
        //    try
        //    {
        //        object authorID;
        //        object publisherID;
        //        object genreID;
        //        object languageID;

        //        if (textBox.Text != "" && textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
        //        {
        //            using (MySqlCommand authorCommand = new MySqlCommand("SELECT id FROM author WHERE author_name = @author_name", connection))
        //            {
        //                authorCommand.Parameters.Clear();
        //                authorCommand.Parameters.AddWithValue("@author_name", textBox1.Text);
        //                authorID = authorCommand.ExecuteScalar();

        //                if (authorID == null)
        //                { // If there is no selected author name, it adds it to the table.
        //                    authorCommand.CommandText = "INSERT INTO author (author_name) VALUES (@author_name); SELECT LAST_INSERT_ID();";
        //                    authorID = authorCommand.ExecuteScalar();
        //                }
        //            }

        //            using (MySqlCommand publisherCommand = new MySqlCommand("SELECT id FROM publisher WHERE publisher_name = @publisher_name", connection))
        //            {
        //                publisherCommand.Parameters.Clear();
        //                publisherCommand.Parameters.AddWithValue("@publisher_name", textBox4.Text);
        //                publisherID = publisherCommand.ExecuteScalar();

        //                if (publisherID == null)
        //                { // If there is no selected publisher, it adds it to the table.
        //                    publisherCommand.CommandText = "INSERT INTO publisher (publisher_name) VALUES (@publisher_name); SELECT LAST_INSERT_ID();";
        //                    publisherID = publisherCommand.ExecuteScalar();
        //                }
        //            }

        //            using (MySqlCommand genreCommand = new MySqlCommand("SELECT id FROM genre WHERE genre_name = @genre_name", connection))
        //            {
        //                genreCommand.Parameters.Clear();
        //                genreCommand.Parameters.AddWithValue("@genre_name", textBox7.Text);
        //                genreID = genreCommand.ExecuteScalar();

        //                if (genreID == null)
        //                { // If there is no selected genre, it adds it to the table.
        //                    genreCommand.CommandText = "INSERT INTO genre (genre_name) VALUES (@genre_name); SELECT LAST_INSERT_ID();";
        //                    genreID = genreCommand.ExecuteScalar();
        //                }
        //            }

        //            using (MySqlCommand languageCommand = new MySqlCommand("SELECT id FROM language WHERE language_name = @language_name", connection))
        //            {
        //                languageCommand.Parameters.Clear();
        //                languageCommand.Parameters.AddWithValue("@language_name", textBox8.Text);
        //                languageID = languageCommand.ExecuteScalar();

        //                if (languageID == null)
        //                { // If there is no selected language, it adds it to the table.
        //                    languageCommand.CommandText = "INSERT INTO language (language_name) VALUES (@language_name); SELECT LAST_INSERT_ID();";
        //                    languageID = languageCommand.ExecuteScalar();
        //                }
        //            }
        //            using (command)
        //            { // updating or inserting the table
        //                command.Parameters.AddWithValue("@title", textBox.Text);
        //                command.Parameters.AddWithValue("@authorID", authorID);
        //                command.Parameters.AddWithValue("@isbn", textBox2.Text);
        //                command.Parameters.AddWithValue("@price", textBox3.Text);
        //                command.Parameters.AddWithValue("@stock_quantity", textBox5.Text);
        //                command.Parameters.AddWithValue("@publisherID", publisherID);
        //                command.Parameters.AddWithValue("@publication_date", dateTimePicker.Value);
        //                command.Parameters.AddWithValue("@number_of_page", textBox6.Text);
        //                command.Parameters.AddWithValue("@description", richTextBox.Text);
        //                command.Parameters.AddWithValue("@genreID", genreID);
        //                command.Parameters.AddWithValue("@languageID", languageID);

        //                CustomerAccount form6 = new CustomerAccount(userID, null);
        //                form6.control(command);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error: " + ex.Message);
        //    }
        //}
        private async void Update(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox13.Text) ||
               string.IsNullOrWhiteSpace(richTextBox1.Text) ||
               string.IsNullOrWhiteSpace(textBox12.Text)||
                string.IsNullOrWhiteSpace(textBox11.Text) ||
                 string.IsNullOrWhiteSpace(textBox18.Text) ||
                  string.IsNullOrWhiteSpace(textBox16.Text) ||
                   string.IsNullOrWhiteSpace(textBox10.Text) ||
                    string.IsNullOrWhiteSpace(textBox9.Text) ||
                     string.IsNullOrWhiteSpace(textBox15.Text)||
                             string.IsNullOrWhiteSpace(dateTimePicker1.ToString()))
            //textBox13, textBox12, textBox11, textBox10, textBox18, textBox16, textBox9, textBox15, textBox14, richTextBox1, dateTimePicker1
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            decimal price_X;
            int stockQuantity;
            int numberOfPages;

           
            try
            {
                price_X = decimal.Parse(textBox10.Text);
                stockQuantity = int.Parse(textBox9.Text);
                numberOfPages = int.Parse(textBox16.Text);
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Giriş formatı hatalı: {ex.Message}");
                return;
            }
            catch (OverflowException ex)
            {
                MessageBox.Show("Girilen değerler çok büyük veya çok küçük.");
                return;
            }
            //textBox13, textBox12, textBox11, textBox10, textBox18, textBox16, textBox9, textBox15, textBox14, richTextBox1, dateTimePicker1
            var bookData = new
            {
                title = textBox13.Text,
                author_name = textBox12.Text,
                isbn = textBox11.Text,
                price = price_X,
                stock_quantity = stockQuantity,
                publisher_name = textBox18.Text,
                publication_date = dateTimePicker1.Value.ToString("yyyy-MM-dd"),
                number_of_page = numberOfPages,
                description = richTextBox1.Text,
                genre_name = textBox15.Text,
                language_name = textBox14.Text
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/api/v1/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var content = new StringContent(JsonConvert.SerializeObject(bookData), Encoding.UTF8, "application/json");

                    string bookId = label44.Text; // 
                    var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"books/{bookId}") // Send a PATCH request to update the password
                    {
                        Content = content
                    };

                    var response = await client.SendAsync(request);
                    var responseContent = await response.Content.ReadAsStringAsync();
               
                    

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Book has been successfully updated.");
                    }
                    else
                    {
                        try
                        {
                            var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                            if (errorResponse.ContainsKey("message"))
                            {
                                MessageBox.Show(errorResponse["message"], "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show("An unknown error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (JsonException ex)
                        {
                            // Handle JSON parsing error, if the response is not in expected JSON format
                            MessageBox.Show($"Error parsing server response: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
















            
        }
        private async void İnsert(object sender, EventArgs e)

        {
            if (string.IsNullOrWhiteSpace(textBox28.Text) ||
    string.IsNullOrWhiteSpace(textBox27.Text) ||
    string.IsNullOrWhiteSpace(textBox26.Text) ||
    string.IsNullOrWhiteSpace(textBox25.Text) ||
    string.IsNullOrWhiteSpace(textBox23.Text) ||
    string.IsNullOrWhiteSpace(textBox21.Text) ||
    string.IsNullOrWhiteSpace(textBox24.Text) ||
    string.IsNullOrWhiteSpace(textBox20.Text) ||
    string.IsNullOrWhiteSpace(textBox19.Text) ||
    string.IsNullOrWhiteSpace(richTextBox2.Text))
            {
               
                MessageBox.Show("Lütfen tüm alanları doldurun.");
                return;
            }

            decimal price_X;
            int stockQuantity;
            int numberOfPages;

            try
            {
                price_X = decimal.Parse(textBox25.Text);
                stockQuantity = int.Parse(textBox24.Text);
                numberOfPages = int.Parse(textBox21.Text);
            }
            catch (FormatException ex )
            {
            
                MessageBox.Show($"Giriş formatı hatalı: {ex.Message}");
                return;
            }
            catch (OverflowException ex)
            {
                
                MessageBox.Show("Girilen değerler çok büyük veya çok küçük.");
                return;
            }

            var bookData = new
            {
                title = textBox28.Text,
                author_name = textBox27.Text,
                isbn = textBox26.Text,
                price = price_X.ToString(),
                stock_quantity = stockQuantity,
                publisher_name = textBox23.Text,
                publication_date = dateTimePicker2.Value.ToString("yyyy-MM-dd"),
                number_of_page = numberOfPages,
                description = richTextBox2.Text,
                genre_name = textBox20.Text,
                language_name = textBox19.Text
            };



            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:3000/api/v1/");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var content = new StringContent(JsonConvert.SerializeObject(bookData), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("books", content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                       
                        MessageBox.Show("Book has been successfully Added.");
                      
                      
                    }
                    else
                    {
                        try
                        {
                            var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                            if (errorResponse.ContainsKey("message"))
                            {
                                MessageBox.Show(errorResponse["message"], "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                MessageBox.Show("An unknown error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (JsonException ex)
                        {
                            // Handle JSON parsing error, if the response is not in expected JSON format
                            MessageBox.Show($"Error parsing server response: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
                catch (HttpRequestException ex)
                {
                    
                    MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }




           
        }
        private void button5_Click(object sender, EventArgs e)
        { // delete book
            result = MessageBox.Show("Are you Sure?", "Approve", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                command = new MySqlCommand("DELETE FROM book WHERE id = @bookId", connection);
                command.Parameters.AddWithValue("@bookId", label44.Text);

                CustomerAccount form6 = new CustomerAccount(userID, null);
                form6.control(command);

                command = new MySqlCommand("DELETE FROM rating WHERE book_id = @bookId", connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@bookId", label44.Text);
                form6.control(command);

                command = new MySqlCommand("DELETE FROM comment WHERE book_id = @bookId", connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@bookId", label44.Text);
                form6.control(command);

                bookInfo(-1);

                addComboBox("SELECT id, title FROM book ORDER BY title", comboBox2);
            }
        }

        // user info
        private void customerInfo(int userID)
        {
            try
            {
                if (userID != -1)
                {
                    command = new MySqlCommand("SELECT u.id, u.username, u.first_name, u.last_name, u.gender, u.email, u.phone, u.is_manager, u.password_hash, c.wallet " +
                                               "FROM users u " +
                                               "LEFT JOIN customer c ON c.id = u.id " +
                                               "OR c.id IS NULL " +
                                               "WHERE u.id = @userID", connection);

                    using (command)
                    {
                        command.Parameters.AddWithValue("@userID", userID);

                        using (reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                label49.Text = reader["id"].ToString(); // print customer informations
                                textBox35.Text = reader["username"].ToString();
                                textBox34.Text = reader["first_name"].ToString();
                                textBox33.Text = reader["last_name"].ToString();
                                comboBox4.Text = reader["gender"].ToString();
                                textBox32.Text = reader["email"].ToString();
                                textBox31.Text = reader["phone"].ToString();
                                comboBox5.Text = reader["is_manager"].ToString();
                                if (reader["is_manager"].ToString() == "0") { textBox29.Text = Convert.ToDecimal(reader["wallet"]).ToString("0.00", CultureInfo.InvariantCulture); }
                                else { textBox29.Text = ""; }
                            }
                        }
                    }
                }
                else
                {
                    label49.Text = "";
                    textBox35.Text = "";
                    textBox34.Text = "";
                    textBox33.Text = "";
                    comboBox4.Text = "";
                    textBox32.Text = "";
                    textBox31.Text = "";
                    comboBox5.Text = "";
                    textBox29.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        { // see book info for user id
            if (comboBox3.SelectedItem != null)
            {
                List<int> userId = comboBox3.Tag as List<int>;
                customerInfo(userId[comboBox3.SelectedIndex]);
            }
            else
            {
                bookInfo(-1);
            }
        }

        // customer update, delete, create
        public async Task updateUser(TextBox textBox, TextBox textBox1, TextBox textBox2, TextBox textBox3, TextBox textBox4, TextBox textBox5, ComboBox comboBox, ComboBox comboBox1)
        { // update user
            if (textBox.Text != "" && textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "")
            { // Check if all required fields are filled
                string token = GetTokenForUser(Convert.ToInt32(label49.Text)); // Retrieve the authentication token for the user
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
                        is_manager = comboBox1.Text,
                        wallet = textBox5.Text
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
                            try
                            {
                                var errorResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
                                if (errorResponse.ContainsKey("message"))
                                {
                                    MessageBox.Show(errorResponse["message"], "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    MessageBox.Show("An unknown error occurred.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch (JsonException ex)
                            {
                                // Handle JSON parsing error, if the response is not in expected JSON format
                                MessageBox.Show($"Error parsing server response: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

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
        private async void button10_Click(object sender, EventArgs e)
        { // update current user
            await updateUser(textBox35, textBox34, textBox33, textBox32, textBox31, textBox29, comboBox4, comboBox5);
        }
        private async void button12_Click(object sender, EventArgs e)
        { // insert new user
            SignupPage form4 = new SignupPage();
            await form4.addUser(textBox42, textBox41, textBox40, textBox39, textBox38, textBox36, textBox17, comboBox7, 0);

            addComboBox("SELECT id, username FROM  users ORDER BY username", comboBox3);
        }
        private void button8_Click(object sender, EventArgs e)
        { // delete current customer
            result = MessageBox.Show("Are you Sure?", "Approve", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                command = new MySqlCommand("DELETE FROM users WHERE id = @userID", connection);

                command.Parameters.AddWithValue("@userID", label49.Text);

                CustomerAccount form6 = new CustomerAccount(userID, null);
                form6.control(command);

                customerInfo(-1);

                addComboBox("SELECT id, username FROM  users ORDER BY username", comboBox3);
            }
        }

        // see and update order status
        private void listedGridViewItems()
        { // listed data grid view
            command = new MySqlCommand("SELECT o.id AS 'id', u.username AS 'Username', o.order_date AS 'Order Date', o.total_amount AS 'Total Amount', p.method_name AS 'Method Name', s.status_name AS 'Order Status' " +
                                       "FROM `order` o " +
                                       "LEFT JOIN paymentmethod p ON p.id = o.payment_method " +
                                       "LEFT JOIN users u ON u.id = o.customer_id " +
                                       "LEFT JOIN orderstatus s ON s.id = o.order_status", connection);

            CustomerAccount form6 = new CustomerAccount(userID, null);
            form6.listedGridViewItems(command, dataGridView2);
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        { // for see ordered books
            if (e.ColumnIndex == dataGridView2.Columns["See Order"].Index && e.RowIndex >= 0)
            {
                orderID = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["id"].Value);

                command = new MySqlCommand("SELECT s.status_name FROM `order` o " +
                                           "LEFT JOIN orderstatus s ON s.id = o.order_status " +
                                           "WHERE o.id = @orderID", connection);

                command.Parameters.AddWithValue("@orderID", orderID);

                using (command)
                {
                    using (reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            comboBox8.Text = reader["status_name"].ToString();
                        }
                    }
                }
            }
        }
        private void button11_Click(object sender, EventArgs e)
        { // change order status
            command = new MySqlCommand("UPDATE `order` SET order_status = @order_status WHERE id = @orderID", connection);

            using (command)
            {
                MySqlCommand command1 = new MySqlCommand("SELECT id FROM orderstatus WHERE status_name = @status_name", connection);

                command1.Parameters.AddWithValue("@status_name", comboBox8.Text);

                using (reader = command1.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        command.Parameters.AddWithValue("@orderID", orderID);
                        command.Parameters.AddWithValue("@order_status", Convert.ToInt32(reader["id"]));
                    }
                }

                CustomerAccount form6 = new CustomerAccount(userID, null);
                form6.control(command);
            }
        }

        // form closed
        private void Form7_FormClosed(object sender, FormClosedEventArgs e)
        { // close forms
            HomePage form1 = new HomePage(userID, null);
            form1.Show();
            this.Hide();
        }

        private async void Delete(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                var bookId = label44.Text;
                var url = $"http://localhost:3000/api/v1/books/{bookId}";

                try
                {
                    var response = await client.DeleteAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Book deleted successfully");
                    }
                    else
                    {
                        // Read the response content from the server
                        var serverResponse = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Error deleting book: {serverResponse}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}