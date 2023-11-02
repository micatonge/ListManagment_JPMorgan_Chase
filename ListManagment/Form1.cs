using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace ListManagment
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            getEmptyList();
        }

        string connetionString = "server=localhost;port=3306;database=reviews;uid=root;pwd=mochi;";

        private void button1_Click(object sender, EventArgs e)
        {
            string connetionString = "server=localhost;port=3306;database=reviews;uid=root;pwd=mochi;";
            using (MySqlConnection cnn = new MySqlConnection(connetionString))
            {
                cnn.Open();
                string reviews_username = textBox1.Text;
                string city = comboBox1.Text;
                string name = textBox4.Text;
                DateTime dateAdded = dateTimePicker1.Value;
                string reviews_text = textBox6.Text;
                string reviews_rating = comboBox2.Text; // Assuming reviews_rating is an integer

                string query = "CALL InsertResturauntReview(@reviews_username, @city, @name, @dateAdded, @reviews_text, @reviews_rating)";
                MySqlCommand cmd = new MySqlCommand(query, cnn);
                cmd.Parameters.AddWithValue("@reviews_username", reviews_username);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@dateAdded", dateAdded.ToString("yyyy-MM-dd")); // Format the date
                cmd.Parameters.AddWithValue("@reviews_text", reviews_text);
                cmd.Parameters.AddWithValue("@reviews_rating", reviews_rating);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Success");
                    getEmptyList();
                }
                else
                {
                    MessageBox.Show("Failed to insert data");
                }
            }
        }

        void getEmptyList()
        {
            string connetionString = "server=localhost;port=3306;database=reviews;uid=root;pwd=mochi;";

            using (MySqlConnection cnn = new MySqlConnection(connetionString))
            {
                cnn.Open();
                string query = "SELECT * FROM data";
                using (MySqlCommand cmd = new MySqlCommand(query, cnn))
                {
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            getEmptyList();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            MySqlConnection cnn;
            cnn = new MySqlConnection(connetionString);
            try
            {
                cnn.Open();
                MessageBox.Show("Connection Open ! ");
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! ");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string connetionString = "server=localhost;port=3306;database=reviews;uid=root;pwd=mochi;";
            using (MySqlConnection cnn = new MySqlConnection(connetionString))
            {
                cnn.Open();
                int reviewId; // Replace with the specific ReviewId you want to delete

                if (int.TryParse(textBox1.Text, out reviewId))
                {
                    using (MySqlCommand cmd = new MySqlCommand("DeleteReviewById", cnn)) // Use the new stored procedure
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_ReviewId", reviewId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Review deleted successfully");
                            getEmptyList();
                        }
                        else
                        {
                            MessageBox.Show("No matching review found for the given ReviewId");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid ReviewId. Please enter a valid integer.");
                }
            }
        }


  
        private void button4_Click(object sender, EventArgs e)
        {
            string connetionString = "server=localhost;port=3306;database=reviews;uid=root;pwd=mochi;";
            using (MySqlConnection cnn = new MySqlConnection(connetionString))
            {
                cnn.Open();
                string reviews_username = textBox1.Text;
                string city = comboBox1.Text;
                string name = textBox4.Text;
                DateTime dateAdded = dateTimePicker1.Value;
                string reviews_text = textBox6.Text;
                string reviews_rating = comboBox2.Text;
                int reviewId; // Replace with the specific ReviewId you want to update

                if (int.TryParse(textBox1.Text, out reviewId))
                {
                    // Create a MySqlCommand for the stored procedure
                    using (MySqlCommand cmd = new MySqlCommand("UpdateReviewsById", cnn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure; // Specify that it's a stored procedure
                        cmd.Parameters.AddWithValue("@p_ReviewId", reviewId); // Specify the ReviewId to update
                        cmd.Parameters.AddWithValue("@p_ReviewsUsername", "updated User");
                        cmd.Parameters.AddWithValue("@p_City", city);
                        cmd.Parameters.AddWithValue("@p_Name", name);
                        cmd.Parameters.AddWithValue("@p_DateAdded", dateAdded);
                        cmd.Parameters.AddWithValue("@p_ReviewsText", reviews_text);
                        cmd.Parameters.AddWithValue("@p_ReviewsRating", reviews_rating);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Success");
                            getEmptyList();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update data");
                        }
                    }
                }
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://www.kaggle.com/datasets/uciml/restaurant-data-with-consumer-ratings";

            try
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., browser not found)
                MessageBox.Show("Error opening the URL: " + ex.Message);
            }
        }
    }
}