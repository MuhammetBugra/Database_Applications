using MySql.Data.MySqlClient;
using System.IO;

namespace bookShopping
{
    internal class addImage
    {
        public static void MainImage()
        {
            string connectionString = "Data Source=localhost; Initial Catalog=databaseApp; User ID=root; Password=Aa1EMRE.;";
            string imagesDirectory = @"C:\Users\MBG\Desktop\bookShopping\image";

            UpdateBookImages(connectionString, imagesDirectory);
        }
        public static void UpdateBookImages(string connectionString, string imagesDirectory)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            for (int i = 1; i <= 64; i++)
            {
                string imagePath = Path.Combine(imagesDirectory, $"{i}.jpg"); // Varsayılan olarak .jpg formatı kabul edilir.
                if (File.Exists(imagePath))
                {
                    byte[] imageBytes = File.ReadAllBytes(imagePath);
                    UpdateImage(connection, i, imageBytes);
                }
            }
            connection.Close();
        }
        public static void UpdateImage(MySqlConnection connection, int id, byte[] imageBytes)
        {
            string query = "UPDATE book SET image = @img WHERE id = @id";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@img", imageBytes);
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }
    }
}