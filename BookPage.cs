using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bookShopping
{
    public partial class BookPage : Form
    {
        // Variables required to receive database queries
        MySqlCommand command;
        MySqlConnection connection = new MySqlConnection("Data Source = localhost; Initial Catalog = databaseApp; User ID = root; Password = Aa1EMRE.");
        MySqlDataAdapter adapter;
        MySqlDataReader reader;
        DataTable dataTable;
        DataSet dataSet;

        // userID and bookID for for purchasing or browsing
        private int userID;
        private int bookID;

        private List<int> cart; // book cart for purchases

        private Boolean isLinkColumnAdded = false; // control for datagridview link add one times
        private Boolean isDelete = false; // delete control

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
        public BookPage(int bookID, int userID, List<int> cart)
        {
            sqlConnect();
            InitializeComponent();

            this.userID = userID; // assign incoming values
            this.bookID = bookID;
            this.cart = cart;
        }
        private async void Form2_Load(object sender, EventArgs e)
        {
            accountControl(); // calling required functions
           await LoadBookInfo(this.bookID);
            ratingInfo();
            commentInfo(null);
        }
        
        // book pages
        private void accountControl()
        {
            if (userID != -1)
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
                comboBox1.Enabled = true;
                richTextBox2.ReadOnly = false;
            }
        }
        private void ratingInfo()
        { // print average rating
            command = new MySqlCommand("SELECT book_id, AVG(rating) AS average_rating FROM rating WHERE book_id = @bookID GROUP BY book_id", connection);
            
            using (command)
            {
                command.Parameters.AddWithValue("@bookID", bookID);

                try
                {
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        label24.Text = reader["average_rating"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            reader.Close();
        }
        private void commentInfo(MySqlCommand command)
        { // print comment in datagridview
            if (command == null) // If there is no command, it will list all comment
            {
                command = new MySqlCommand("SELECT c.id AS 'id', u.username AS 'Username', u.first_name AS 'First Name', u.last_name AS 'Last Name', c.comment AS 'Comment' " +
                                           "FROM comment c " +
                                           "INNER JOIN users u ON u.id = c.user_id " +
                                           "WHERE c.book_id = @bookID", connection);
            }
            using (command)
            {
                command.Parameters.AddWithValue("@bookID", bookID); // Returns SQL table according to book id

                using (adapter = new MySqlDataAdapter(command))
                {
                    dataSet = new DataSet();
                    adapter.Fill(dataSet, "comment");

                    dataGridView1.DataSource = dataSet.Tables["comment"];
                    dataGridView1.Columns["id"].Visible = false; // Hides the id column

                    if (!isLinkColumnAdded)
                    {
                        DataGridViewLinkColumn linkColumn = new DataGridViewLinkColumn(); // datagridview adds a link for delete comment
                        linkColumn.Name = "Delete";
                        linkColumn.UseColumnTextForLinkValue = true;
                        linkColumn.Text = "Del";
                        dataGridView1.Columns.Add(linkColumn);

                        isLinkColumnAdded = true;
                    }

                    dataGridView1.Columns["Delete"].Visible = isDelete; // It becomes active only when you press the my comment button.
                }
            }
        }

        private async Task LoadBookInfo(int bookID)
        {
            string url = $"http://localhost:3000/api/v1/books/{bookID}";

            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                var book = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

                if (book != null)
                {
                    // Resim dosyası adını oluşturma ve resmi yükleme
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
                        pictureBox1.Image = null; // Resim bulunamadıysa
                    }

                    // Kitap bilgilerini etiketlere yazdırma
                    label1.Text = book["title"].ToString();
                    label4.Text = book["author_name"].ToString();
                    label5.Text = book["publisher_name"].ToString();
                    label14.Text = book["isbn"].ToString();
                    label15.Text = Convert.ToDateTime(book["publication_date"]).ToString("dd.MM.yyyy");
                    label16.Text = book["genre_name"].ToString();
                    label17.Text = book["language_name"].ToString();
                    label18.Text = book["number_of_page"].ToString();
                    label19.Text = book["price"].ToString() + " TL";
                    label20.Text = book["number_of_sales"].ToString();
                    label21.Text = book["stock_quantity"].ToString();
                    richTextBox1.Text = book["description"].ToString();
                }
                else
                {
                    MessageBox.Show("Book not found.");
                }
            }
        }

      








        //private void addBookInfo()
        //{
        //    command = new MySqlCommand("SELECT b.title, a.author_name, b.isbn, b.price, b.stock_quantity, p.publisher_name, b.publication_date, b.number_of_sales, b.number_of_page, b.description, g.genre_name, l.language_name, b.image " +
        //                               "FROM book b " +
        //                               "INNER JOIN Author a ON b.author = a.id " +
        //                               "INNER JOIN Publisher p ON b.publisher = p.id " +
        //                               "INNER JOIN Genre g ON b.genre = g.id " +
        //                               "INNER JOIN Language l ON b.language = l.id " +
        //                               "WHERE b.id = @bookID", connection);

        //    using (command)
        //    {
        //        command.Parameters.AddWithValue("@bookID", bookID); // Returns book information according to book id

        //        using (adapter = new MySqlDataAdapter(command))
        //        {
        //            using (dataTable = new DataTable())
        //            {
        //                adapter.Fill(dataTable);

        //                if (dataTable.Rows.Count > 0)
        //                {
        //                    DataRow row = dataTable.Rows[0];

        //                    byte[] imageData = row["image"] as byte[]; // add image

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

        //                    label1.Text = row["title"].ToString(); // writes book information to labels
        //                    label4.Text = row["author_name"].ToString();
        //                    label5.Text = row["publisher_name"].ToString();
        //                    label14.Text = row["isbn"].ToString();
        //                    label15.Text = Convert.ToDateTime(row["publication_date"]).ToString("dd.MM.yyyy");
        //                    label16.Text = row["genre_name"].ToString();
        //                    label17.Text = row["language_name"].ToString();
        //                    label18.Text = row["number_of_page"].ToString();
        //                    label19.Text = row["price"].ToString() + " TL";
        //                    label20.Text = row["number_of_sales"].ToString();
        //                    label21.Text = row["stock_quantity"].ToString();
        //                    richTextBox1.Text = row["description"].ToString();
        //                }
        //            }
        //        }
        //    }
        //}

        // add or remove book for cart
        private void button1_Click(object sender, EventArgs e)
        { // add book in cart
            command = new MySqlCommand("SELECT stock_quantity FROM book WHERE id = @bookID", connection);  

            using (command)
            {
                command.Parameters.AddWithValue("@bookID", bookID);

                using (reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (Convert.ToInt32(reader["stock_quantity"]) > 0 && cart != null)
                        {
                            reader.Close();

                            cart.Add(bookID);

                            MySqlCommand bookCommand = new MySqlCommand("UPDATE book SET number_of_sales = number_of_sales + 1, stock_quantity = stock_quantity - 1 WHERE id = @BookID", connection);

                            using (bookCommand)
                            {
                                bookCommand.Parameters.AddWithValue("@BookID", bookID);
                                bookCommand.ExecuteNonQuery();
                            }

                            MessageBox.Show("Success");
                        }
                        else
                        {
                            MessageBox.Show("Failed");
                        }
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        { // remove book from cart
            if (cart != null && cart.Contains(bookID))
            {
                cart.Remove(bookID);

                MySqlCommand bookCommand = new MySqlCommand("UPDATE book SET number_of_sales = number_of_sales - 1, stock_quantity = stock_quantity + 1 WHERE id = @BookID", connection);

                using (bookCommand)
                {
                    bookCommand.Parameters.AddWithValue("@BookID", bookID);
                    bookCommand.ExecuteNonQuery();
                }

                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("The selected book is not in the basket.");
            }
        }

        // add or update rating
        private void addUpdateRating(String query, int rating)
        { // add or update rating with query
            command = new MySqlCommand(query, connection);

            using (command)
            {
                command.Parameters.AddWithValue("@bookID", bookID);
                command.Parameters.AddWithValue("@userID", userID);
                command.Parameters.AddWithValue("@rating", rating);

                command.ExecuteNonQuery();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                int selectedRating = Convert.ToInt32(comboBox1.SelectedItem.ToString());

                MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM rating WHERE book_id = @bookID AND user_id = @userID", connection);

                using (command)
                {
                    command.Parameters.AddWithValue("@bookID", bookID);
                    command.Parameters.AddWithValue("@userID", userID);

                    int count = Convert.ToInt32(command.ExecuteScalar()); // control rating in table

                    if (count > 0) 
                    { // update rating
                        addUpdateRating("UPDATE rating SET rating = @rating WHERE book_id = @bookID AND user_id = @userID", selectedRating);
                    }
                    else
                    { // add rating
                        addUpdateRating("INSERT INTO rating (book_id, user_id, rating) VALUES (@bookID, @userID, @rating)", selectedRating);
                    }
                }
            }

            ratingInfo();
        }

        // add or delete comment
        private void button3_Click(object sender, EventArgs e)
        { // add comment
            command = new MySqlCommand("INSERT INTO comment(book_id, user_id, comment) VALUES(@bookID, @userID, @commentText)", connection);

            using (command)
            {
                command.Parameters.AddWithValue("@bookID", bookID); // add parameters
                command.Parameters.AddWithValue("@userID", userID);
                command.Parameters.AddWithValue("@commentText", richTextBox2.Text);

                CustomerAccount form6 = new CustomerAccount(userID, cart);
                form6.control(command);

                richTextBox2.Text = "";
            }

            isDelete = false;
            commentInfo(null);
        }
        private void button5_Click(object sender, EventArgs e)
        { // see my comment
            command = new MySqlCommand("SELECT c.id AS 'id', u.username AS 'Username', u.first_name AS 'First Name', u.last_name AS 'Last Name', c.comment AS 'Comment' " +
                                       "FROM comment c " +
                                       "INNER JOIN users u ON u.id = c.user_id " +
                                       "WHERE c.user_id = @userID " +
                                       "AND c.book_id = @bookID", connection);

            command.Parameters.AddWithValue("@userID", userID); // Returns SQL table according to user id

            isDelete = true;
            commentInfo(command);
        }
        private void button6_Click(object sender, EventArgs e)
        { // see all comment
            isDelete = false;
            commentInfo(null);
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                int delID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);

                command = new MySqlCommand("DELETE FROM comment WHERE id = @commentID", connection);

                command.Parameters.AddWithValue("@commentID", delID);

                CustomerAccount form6 = new CustomerAccount(userID, cart);
                form6.control(command);
            }

            isDelete = false;
            commentInfo(null); // It deletes the clicked data according to its id.
        }

        // form closed
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        { // close forms
            HomePage form1 = new HomePage(userID, cart);
            form1.Show();
            this.Hide();
        }
    }
}