using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using MySqlX.XDevAPI.Relational;
using Newtonsoft.Json.Linq;


namespace bookShopping
{
    public partial class HomePage : Form
    {
        // Variables required to receive database queries
        MySqlCommand command;
        MySqlConnection connection = new MySqlConnection("Data Source = localhost; Initial Catalog = databaseApp; User ID = root; Password = Aa1EMRE."); // database connection
        MySqlDataAdapter adapter;
        MySqlDataReader reader;
        DataTable dataTable;
        DataSet dataSet;

        // List required to place labels and images
        private Label[] linkLabelArray;
        private List<PictureBox> pictureBoxList;

        private List<int> cart; // book cart for purchases

        private int userID; // userID for accounts
        private int isLinkColumnAdded = 0; // control for datagridview link add one times

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
        public HomePage(int userID, List<int> cart)
        {
            sqlConnect();
            InitializeComponent();

            this.userID = userID; // assign incoming values
            this.cart = cart;
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            InitializeTimer();
            upperMenu(); // calling required functions
         await   randomBook();
            //  bestsellers();
            //latestBooks();
            await bestsellers();
            await latestBooks();
         await   AddComboBox();
            seeManager();
            //  listedGridViewItems(null, dataGridView1);
           await LoadDataFromApi(dataGridView1);
          //  listedGridViewItems(null, dataGridView2);
            await LoadDataFromApi(dataGridView2);
        }


        private async Task AddComboBox()
        {
            await LoadDataToComboBox("http://localhost:3000/api/v1/books/authors", comboBox2);
            await LoadDataToComboBox("http://localhost:3000/api/v1/books/genres", comboBox1);
          //  await LoadDataToComboBox("http://localhost:3000/api/v1/books/authors", comboBox2);
            await LoadDataToComboBox("http://localhost:3000/api/v1/books/publishers", comboBox3);
        }

       
       

private async Task LoadDataToComboBox(string url, ComboBox comboBox)
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetStringAsync(url);
            var jsonArray = JArray.Parse(response);

            List<int> itemIds = new List<int> { -1 }; // İlk öğe için -1 ekleyin
            comboBox.Items.Clear(); // Önceki öğeleri temizleyin
            comboBox.Items.Add(""); // Boş bir seçenek ekleyin

            foreach (var item in jsonArray)
            {
                int id = item.Value<int>("id");
                string name = item.Value<string>("genre_name") ?? item.Value<string>("author_name") ?? item.Value<string>("publisher_name");

                comboBox.Items.Add(name);
                itemIds.Add(id);
            }

            comboBox.Tag = itemIds; // ID listesini Tag özelliğine atayın
        }
    }



    private async Task LoadDataFromApi(DataGridView dataGridView1)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync("http://localhost:3000/api/v1/books");
                var books = JsonConvert.DeserializeObject<DataTable>(response);

                // DataTable'a Image sütunu ekleyin, eğer zaten yoksa
                if (!books.Columns.Contains("Image"))
                {
                    books.Columns.Add("Image", typeof(System.Drawing.Image));
                }

                // Resimleri DataTable'a yükleyin
                LoadImagesToDataTable(books, "Image");
               

                Invoke(new Action(() => {
                    dataGridView1.DataSource = books;
                    dataGridView1.Columns["id"].Visible = false; 
                    SetupDataGridViewForImages(dataGridView1, "Image");

                    if (isLinkColumnAdded <3) {

                        DataGridViewLinkColumn linkColumn = new DataGridViewLinkColumn(); // datagridview adds a link to go to the book page
                        linkColumn.Name = "Go to Book Page";
                        linkColumn.UseColumnTextForLinkValue = true;
                        linkColumn.Text = "Go";
                        dataGridView1.Columns.Add(linkColumn);

                    }
                    isLinkColumnAdded++;



                }));
            }
        }

        private void LoadImagesToDataTable(DataTable dataTable, string imageColumnName)
        {
            string imagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

            foreach (DataRow row in dataTable.Rows)
            {
                string imageFileName = row["id"].ToString() + ".jpg"; // 
                string imagePath = Path.Combine(imagesFolderPath, imageFileName);

                if (File.Exists(imagePath))
                {
                    Image image = Image.FromFile(imagePath);
                    row[imageColumnName] = image;
                }
            }
        }

        private void SetupDataGridViewForImages(DataGridView dataGridView, string columnName)
        {
            // DataGridViewImageColumn ayarla
            if (!(dataGridView.Columns[columnName] is DataGridViewImageColumn))
            {
                if (dataGridView.Columns.Contains(columnName))
                {
                    dataGridView.Columns.Remove(columnName);
                }

                DataGridViewImageColumn imgCol = new DataGridViewImageColumn();
                imgCol.Name = columnName;
                imgCol.HeaderText = columnName;
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                dataGridView.Columns.Add(imgCol);
            }
        }






        // upper menu
        private void upperMenu()
        {
            if (userID != -1) // Works if user is logged in
            {
                command = new MySqlCommand("SELECT u.id, u.username, u.first_name, u.last_name, c.wallet, u.is_manager " +
                                           "FROM users u " +
                                           "LEFT JOIN customer c ON u.id = c.id " +
                                           "WHERE u.id = @userID", connection);

                using (command)
                {
                    command.Parameters.AddWithValue("@userID", userID); // Returns SQL table according to user id

                    using (reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (Convert.ToInt32(reader["is_manager"]) == 0) // If it is not a manager, it writes the relevant labels.
                            {
                                label1.Text = "Wallet: " + reader["wallet"].ToString() + " TL";
                                linkLabel1.Visible = true;
                                pictureBox18.Visible = true;
                                pictureBox17.Visible = true;
                            }
                            else  // If it is a manager, it writes the relevant labels.
                            {
                                label1.Text = "Manager Login";
                                linkLabel1.Visible = false;
                                pictureBox17.Visible = false;
                                pictureBox18.Visible = false;
                            }

                            label2.Text = "Name and Surname: " + reader["first_name"].ToString() + " " + reader["last_name"].ToString(); // prints user information
                            label3.Text = "Username: " + reader["username"].ToString();
                            linkLabel4.Visible = true;
                            linkLabel2.Text = "Log Out";
                            linkLabel3.Visible = false;
                        }
                    }
                }
            }
            else // // Works if user is not logged in
            {
                pictureBox18.Visible = false;
                pictureBox17.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                linkLabel1.Visible = false;
                linkLabel4.Visible = false;
                linkLabel2.Text = "Login";
            }
        }

        // get recommended books
        //private void getRandomBook(LinkLabel linkLabel, Label label, PictureBox pictureBox)
        //{ // get random book in sql tables
        //    command = new MySqlCommand("SELECT id, title, price, image FROM Book ORDER BY RAND() LIMIT 1", connection);

        //    using (command)
        //    {
        //        using (MySqlDataReader reader = command.ExecuteReader())
        //        {
        //            if (reader.HasRows)
        //            {
        //                if (reader.Read())
        //                {
        //                    linkLabel.Text = reader["title"].ToString(); // print title and price
        //                    linkLabel.Tag = Convert.ToInt32(reader["id"]);
        //                    label.Text = reader["price"].ToString() + " TL";

        //                    byte[] image = reader["image"] as byte[];

        //                    if (image != null && image.Length > 0)
        //                    {
        //                        try
        //                        {
        //                            using (MemoryStream ms = new MemoryStream(image))
        //                            {
        //                                pictureBox.Image = System.Drawing.Image.FromStream(ms); // state of picturebox
        //                                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        //                                pictureBox.Tag = Convert.ToInt32(reader["id"]); // Used to go to book page
        //                            }
        //                        }
        //                        catch (ArgumentException ex)
        //                        {
        //                            MessageBox.Show($"Error: {ex.Message}");
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

       

private async Task GetRandomBookAsync(LinkLabel linkLabel, Label label, PictureBox pictureBox)
    {
        string url = "http://localhost:3000/api/v1/books/random";

        using (var client = new HttpClient())
        {
            try
            {
                var response = await client.GetStringAsync(url);
                var books = JArray.Parse(response);

                if (books != null && books.Count > 0)
                {
                    var book = books[0]; // JSON dizisinin ilk elemanını al

                    linkLabel.Text = book["title"].ToString();
                    linkLabel.Tag = Convert.ToInt32(book["id"]);
                    label.Text = book["price"].ToString() + " TL";

                    // Resmi Images klasöründen yükle
                    int bookID = Convert.ToInt32(book["id"]);
                    string imagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                    string imageFileName = bookID + ".jpg";
                    string imagePath = Path.Combine(imagesFolderPath, imageFileName);

                    if (File.Exists(imagePath))
                    {
                        pictureBox.Image = Image.FromFile(imagePath);
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                            // Set the Tag property of the PictureBox to the book ID
                            pictureBox.Tag = bookID;
                        }
                    else
                    {
                        pictureBox.Image = null; // Resim bulunamadıysa
                    }
                }
                else
                {
                    MessageBox.Show("No books found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }



    private async Task randomBook()
        { // random two books
          await  GetRandomBookAsync(linkLabel15, label13, pictureBox12); 
          await    GetRandomBookAsync(linkLabel16, label12, pictureBox13);
        }
        private void InitializeTimer()
        { // timer work per 3 seconds for random book
            timer1 = new Timer(); 
            timer1.Interval = 3000;
            timer1.Tick += Timer_Tick;
            timer1.Start();
        }
        private  async void Timer_Tick(object sender, EventArgs e)
        {
         await   randomBook();
        }

        // home page



        private async Task LoadBooksFromAPIAsync(string url, Label[] linkLabelArray, List<PictureBox> pictureBoxList)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetStringAsync(url);
                    var books = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response);

                    if (books != null && books.Count > 0)
                    {
                        for (int i = 0; i < Math.Min(5, books.Count); i++)
                        {
                            var book = books[i];
                            linkLabelArray[i].Text = book["title"].ToString();
                            linkLabelArray[i].Tag = Convert.ToInt32(book["id"]);

                            // Resmi Images klasöründen yükle
                            int bookID = Convert.ToInt32(book["id"]);
                            string imagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                            string imageFileName = bookID + ".jpg";
                            string imagePath = Path.Combine(imagesFolderPath, imageFileName);

                            if (File.Exists(imagePath))
                            {
                                pictureBoxList[i].Image = Image.FromFile(imagePath);
                                pictureBoxList[i].SizeMode = PictureBoxSizeMode.Zoom;
                                // Set the Tag property of the PictureBox to the book ID
                                pictureBoxList[i].Tag = bookID;
                            }
                            else
                            {
                                pictureBoxList[i].Image = null; // Resim bulunamadıysa
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private async Task bestsellers()
        {
            // LinkLabel ve PictureBox listelerini oluştur
            Label[] linkLabelArray = { linkLabel5, linkLabel11, linkLabel6, linkLabel12, linkLabel7 };
            List<PictureBox> pictureBoxList = new List<PictureBox> { pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6 };

            await LoadBooksFromAPIAsync("http://localhost:3000/api/v1/books/best", linkLabelArray, pictureBoxList);
        }

        private async Task latestBooks()
        {
            // LinkLabel ve PictureBox listelerini oluştur
            Label[] linkLabelArray = { linkLabel8, linkLabel13, linkLabel9, linkLabel14, linkLabel10 };
            List<PictureBox> pictureBoxList = new List<PictureBox> { pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11 };

            await LoadBooksFromAPIAsync("http://localhost:3000/api/v1/books/latest", linkLabelArray, pictureBoxList);
        }


        //private void bestsellers()
        //{ // Brings the 5 best-selling books
        //    linkLabelArray = new Label[]
        //    {
        //        linkLabel5, linkLabel11, linkLabel6, linkLabel12, linkLabel7

        //    };

        //    pictureBoxList = new List<PictureBox>
        //    {
        //        pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6
        //    };

        //    using (command = new MySqlCommand("SELECT id, title, image FROM book ORDER BY number_of_sales DESC LIMIT 5", connection))
        //    {
        //        getBooks(command, linkLabelArray, pictureBoxList);
        //    }
        //}
        //private void latestBooks()
        //{ // Brings the latest 5 books
        //    linkLabelArray = new Label[]
        //    {
        //        linkLabel8, linkLabel13, linkLabel9, linkLabel14, linkLabel10
        //    };

        //    pictureBoxList = new List<PictureBox>
        //    {
        //        pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11
        //    };

        //    using (command = new MySqlCommand("SELECT id, title, image FROM book ORDER BY publication_date DESC LIMIT 5", connection))
        //    {
        //        getBooks(command, linkLabelArray, pictureBoxList);
        //    }
        //}
        //private void getBooks(MySqlCommand command, Label[] linkLabelArray, List<PictureBox> pictureBoxList)
        //{ // Prints the name and image information of the book on the homepage screen
        //    try
        //    {
        //        using (adapter = new MySqlDataAdapter(command))
        //        {
        //            dataTable = new DataTable();
        //            adapter.Fill(dataTable);

        //            if (dataTable.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < 5; i++)
        //                {
        //                    linkLabelArray[i].Text = dataTable.Rows[i]["title"].ToString();
        //                    linkLabelArray[i].Tag = Convert.ToInt32(dataTable.Rows[i]["id"]); // Used to go to book page

        //                    byte[] imageBytes = dataTable.Rows[i]["image"] as byte[]; // To take a picture from a sql table into a picture box

        //                    if (imageBytes != null && imageBytes.Length > 0)
        //                    {
        //                        try
        //                        {
        //                            using (MemoryStream ms = new MemoryStream(imageBytes))
        //                            {
        //                                pictureBoxList[i].Image = System.Drawing.Image.FromStream(ms); // state of image
        //                                pictureBoxList[i].SizeMode = PictureBoxSizeMode.Zoom;
        //                                pictureBoxList[i].Tag = Convert.ToInt32(dataTable.Rows[i]["id"]); // Used to go to book page
        //                            }
        //                        }
        //                        catch (ArgumentException ex)
        //                        {
        //                            MessageBox.Show($"Error loading image: {ex.Message}");
        //                        }
        //                    }
        //                    else
        //                    {
        //                        pictureBoxList[i].Image = null;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show("No data found in the DataTable.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An error occurred: {ex.Message}");
        //    }
        //}
        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel clickedLabel = (LinkLabel)sender;
            int bookId = (int)clickedLabel.Tag;

            BookPage form2 = new BookPage(bookId, userID, cart); // goes to form 2 according to the selected book id
            form2.Show();
            this.Hide();
        }
        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickedPictureBox = (PictureBox)sender;

            if (clickedPictureBox.Tag != null && int.TryParse(clickedPictureBox.Tag.ToString(), out int bookId))
            {
                BookPage form2 = new BookPage(bookId, userID, cart); // goes to form 2 according to the selected book id
                form2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid image.");
            }
        }
        
        // book and author, publisher page
        //private void listedGridViewItems(MySqlCommand command, DataGridView dataGrid)
        //{
        //    if (command == null) // If there is no command, it will list all books
        //    {
        //        command = new MySqlCommand("SELECT b.id, b.title AS 'Title', a.author_name AS 'Author', p.publisher_name AS 'Publisher', g.genre_name AS 'Genre', l.language_name AS 'Language', b.price AS 'Price', b.image AS 'Image' " +
        //                                   "FROM Book b " +
        //                                   "INNER JOIN Author a ON b.author = a.id " +
        //                                   "INNER JOIN Publisher p ON b.publisher = p.id " +
        //                                   "INNER JOIN Genre g ON b.genre = g.id " +
        //                                   "INNER JOIN Language l ON b.language = l.id " +
        //                                   "ORDER BY b.title", connection);
        //    }
        //    using (command)
        //    {
        //        using (adapter = new MySqlDataAdapter(command))
        //        {
        //            dataSet = new DataSet();
        //            adapter.Fill(dataSet, "book");

        //            dataGrid.DataSource = dataSet.Tables["book"];
        //            dataGrid.Columns["id"].Visible = false; // Hides the id column

        //            DataGridViewImageColumn imageColumn = (DataGridViewImageColumn)dataGrid.Columns["Image"]; // Adds image from SQL table to datagridview
        //            imageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;

        //            if (isLinkColumnAdded )
        //            {
        //                DataGridViewLinkColumn linkColumn = new DataGridViewLinkColumn(); // datagridview adds a link to go to the book page
        //                linkColumn.Name = "Go to Book Page";
        //                linkColumn.UseColumnTextForLinkValue = true;
        //                linkColumn.Text = "Go";
        //                dataGrid.Columns.Add(linkColumn);

        //             //   isLinkColumnAdded++;
        //            }
        //        }
        //    }
        //}


        // Form yüklendiğinde bu metodu çağır
        //private void addComboBox()
        //{ // Adds data to comboboxes according to commands
        //    AddItemsToComboBox("SELECT id, genre_name FROM genre ORDER BY genre_name", comboBox1); 
        //    AddItemsToComboBox("SELECT id, author_name FROM author ORDER BY author_name", comboBox2);
        //    AddItemsToComboBox("SELECT id, publisher_name FROM publisher ORDER BY publisher_name", comboBox3);
        //}
        public void AddItemsToComboBox(string query, ComboBox comboBox)
        {
            using (command = new MySqlCommand(query, connection))
            {
                using (reader = command.ExecuteReader())
                {
                    List<int> itemIds = new List<int> { -1 }; // To keep ID data of objects added to comboboxes
                    comboBox.Items.Add("");

                    while (reader.Read())
                    {
                        string itemName = reader.GetString(1);

                        if (!comboBox.Items.Contains(itemName))
                        {
                            itemIds.Add(reader.GetInt32(0));
                            comboBox.Items.Add(itemName);
                        }
                    }

                    comboBox.Tag = itemIds;
                }
            }
        }


        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (comboBox1.Text == "") // If it is empty, it does not send commands.
        //    {
        //        listedGridViewItems(null, dataGridView1);
        //    }
        //    else
        //    {
        //        List<int> genreId = comboBox1.Tag as List<int>; // To get the ID information of the selected type

        //        command = new MySqlCommand("SELECT b.id, b.title AS 'Title', a.author_name AS 'Author', p.publisher_name AS 'Publisher', g.genre_name AS 'Genre', l.language_name AS 'Language', b.price AS 'Price', b.image AS 'Image' " +
        //                                   "FROM Book b " +
        //                                   "INNER JOIN Author a ON b.author = a.id " +
        //                                   "INNER JOIN Publisher p ON b.publisher = p.id " +
        //                                   "INNER JOIN Genre g ON b.genre = g.id " +
        //                                   "INNER JOIN Language l ON b.language = l.id " +
        //                                   "WHERE g.id = @selectedGenreId " +
        //                                   "ORDER BY b.title", connection);

        //        command.Parameters.AddWithValue("@selectedGenreId", genreId[comboBox1.SelectedIndex]);

        //        listedGridViewItems(command, dataGridView1); // sends commands to datagridview according to filtering
        //    }
        //}




        private async void button1_Click(object sender, EventArgs e)
        {
            string selectedGenre = comboBox1.Text;
            string url;

            if (string.IsNullOrEmpty(selectedGenre))
            {
                // Eğer tür seçilmemişse, tüm kitapları yükle
                url = "http://localhost:3000/api/v1/books";
            }
            else
            {
                // Seçilen türe göre kitapları yükle
                url = $"http://localhost:3000/api/v1/books/genre/{Uri.EscapeDataString(selectedGenre)}";
            }

            await LoadDataFromApi(dataGridView1, url);
        }

        private async Task LoadDataFromApi(DataGridView dataGridView, string url)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync(url);

                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            MessageBox.Show($"No results found for the search term.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"Error occurred: {response.ReasonPhrase}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        await LoadDataFromApi(dataGridView1, "http://localhost:3000/api/v1/books");

                        return;
                    }

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var books = JsonConvert.DeserializeObject<DataTable>(responseContent);

                    if (!books.Columns.Contains("Image"))
                    {
                        books.Columns.Add("Image", typeof(System.Drawing.Image));
                    }

                    LoadImagesToDataTable(books, "Image");

                    Invoke(new Action(() => {
                        dataGridView.DataSource = books;
                        dataGridView.Columns["id"].Visible = false;
                        SetupDataGridViewForImages(dataGridView, "Image");

                      
                    }));
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"An error occurred while accessing the server: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ... [LoadImagesToDataTable ve SetupDataGridViewForImages metodları] ...

        private async void button2_Click(object sender, EventArgs e)
        {



            string selectedAuthor = comboBox2.Text;
            string url;

            if (string.IsNullOrEmpty(selectedAuthor))
            {
                // Eğer yazar seçilmemişse, tüm kitapları yükle
                url = "http://localhost:3000/api/v1/books";
            }
            else
            {
                // Seçilen yazara göre kitapları yükle
                url = $"http://localhost:3000/api/v1/books/author/{Uri.EscapeDataString(selectedAuthor)}";
            }

            await LoadDataFromApi(dataGridView2, url);
        }










            //comboBox3.Text = "";

            //if (comboBox2.Text == "") // If it is empty, it does not send commands.
            //{
            //  //  listedGridViewItems(null, dataGridView2);
            //}
            //else
            //{
            //    List<int> authorId = comboBox2.Tag as List<int>; // To get the ID information of the selected type

            //    command = new MySqlCommand("SELECT b.id, b.title AS 'Title', a.author_name AS 'Author', p.publisher_name AS 'Publisher', g.genre_name AS 'Genre', l.language_name AS 'Language', b.price AS 'Price', b.image AS 'Image' " +
            //                               "FROM Book b " +
            //                               "INNER JOIN Author a ON b.author = a.id " +
            //                               "INNER JOIN Publisher p ON b.publisher = p.id " +
            //                               "INNER JOIN Genre g ON b.genre = g.id " +
            //                               "INNER JOIN Language l ON b.language = l.id " +
            //                               "WHERE a.id = @selectedAuthorID " +
            //                               "ORDER BY b.title", connection);

            //    command.Parameters.AddWithValue("@selectedAuthorID", authorId[comboBox2.SelectedIndex]);

            //    listedGridViewItems(command, dataGridView2); // sends commands to datagridview according to filtering
            //}
        
        private async void button3_Click(object sender, EventArgs e)
        {




            string selectedPublisher = comboBox3.Text;
            string url;

            if (string.IsNullOrEmpty(selectedPublisher))
            {
                // Eğer yazar seçilmemişse, tüm kitapları yükle
                url = "http://localhost:3000/api/v1/books";
            }
            else
            {
                // Seçilen yazara göre kitapları yükle
                url = $"http://localhost:3000/api/v1/books/Publisher/{Uri.EscapeDataString(selectedPublisher)}";
            }

            await LoadDataFromApi(dataGridView2, url);














            //comboBox2.Text = "";

            //if (comboBox3.Text == "") // If it is empty, it does not send commands.
            //{
            //    listedGridViewItems(null, dataGridView2);
            //}
            //else
            //{
            //    List<int> publisherId = comboBox3.Tag as List<int>; // To get the ID information of the selected type

            //    command = new MySqlCommand("SELECT b.id, b.title AS 'Title', a.author_name AS 'Author', p.publisher_name AS 'Publisher', g.genre_name AS 'Genre', l.language_name AS 'Language', b.price AS 'Price', b.image AS 'Image' " +
            //                               "FROM Book b " +
            //                               "INNER JOIN Author a ON b.author = a.id " +
            //                               "INNER JOIN Publisher p ON b.publisher = p.id " +
            //                               "INNER JOIN Genre g ON b.genre = g.id " +
            //                               "INNER JOIN Language l ON b.language = l.id " +
            //                               "WHERE p.id = @selectedPublisherID " +
            //                               "ORDER BY b.title", connection);

            //    command.Parameters.AddWithValue("@selectedPublisherID", publisherId[comboBox3.SelectedIndex]);

            //    listedGridViewItems(command, dataGridView2); // sends commands to datagridview according to filtering
            //}
        }
        private async void textBox1_TextChanged(object sender, EventArgs e)
        { // Allows the datagridview to be listed according to the text entered in the textbox.



            string searchTerm = textBox1.Text;
            string url = $"http://localhost:3000/api/v1/books/search/{Uri.EscapeDataString(searchTerm)}";

            await LoadDataFromApi(dataGridView1, url);







            //    comboBox1.Text = "";

            //    command = new MySqlCommand("SELECT b.id, b.title AS 'Title', a.author_name AS 'Author', p.publisher_name AS 'Publisher', g.genre_name AS 'Genre', l.language_name AS 'Language', b.price AS 'Price', b.image AS 'Image' " +
            //                               "FROM Book b " +
            //                               "INNER JOIN Author a ON b.author = a.id " +
            //                               "INNER JOIN Publisher p ON b.publisher = p.id " +
            //                               "INNER JOIN Genre g ON b.genre = g.id " +
            //                               "INNER JOIN Language l ON b.language = l.id " +
            //                               "WHERE b.title LIKE '" + textBox1.Text + "%'" +
            //                               "ORDER BY b.title", connection);

            ////    listedGridViewItems(command, dataGridView1);
        }
        private void dataGridView_CellContentClick(DataGridView dataGridView, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView.Columns["Go to Book Page"].Index && e.RowIndex >= 0)
            {
                int bookId = Convert.ToInt32(dataGridView.Rows[e.RowIndex].Cells["id"].Value);

                BookPage form2 = new BookPage(bookId, userID, cart); // goes to form 2 according to the selected book id
                form2.Show();
                this.Hide();
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView_CellContentClick(dataGridView1, e);
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView_CellContentClick(dataGridView2, e);
        }

        // see manager tables
        private void seeManager()
        { // see manager tables
            command = new MySqlCommand("SELECT username AS 'Username', first_name AS 'First Name', last_name AS 'Last Name', email AS 'Email', phone AS 'Phone'" +
                                       "FROM users WHERE is_manager = 1", connection);

            using (command)
            {
                using (adapter = new MySqlDataAdapter(command))
                {
                    dataSet = new DataSet();
                    adapter.Fill(dataSet, "user");

                    dataGridView3.DataSource = dataSet.Tables["user"];
                    dataGridView3.Columns["email"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        // Go to other forms from the top menu
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { // cart form
            if (cart != null && cart.Count > 0)
            {
                Form5 form5 = new Form5(userID, cart);
                form5.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Your cart is empty.");
            }
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { // login logout label
            if (userID != -1)
            {
                HomePage form1 = new HomePage(-1, cart = new List<int>());
                form1.Show();
            }
            else
            {
                loginPage form3 = new loginPage();
                form3.Show();
            }

            this.Hide();
        }
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { // create account form
            SignupPage form4 = new SignupPage();
            form4.Show();
            this.Hide();
        }
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { // account settings form
            using (command = new MySqlCommand("SELECT is_manager FROM users WHERE id = @id", connection))
            {
                command.Parameters.AddWithValue("@id", userID);

                using (reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (Convert.ToInt32(reader["is_manager"]) == 0) // control manager or customer
                        {
                            CustomerAccount form6 = new CustomerAccount(userID, cart);
                            form6.Show();
                        }
                        else
                        {
                            ManagerAccount form7 = new ManagerAccount(userID);
                            form7.Show();
                        }

                        this.Hide();
                    }
                }
            }
        }

        // form closed
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        { // close forms
            connection.Close();
            System.Windows.Forms.Application.Exit();
        }
    }
}