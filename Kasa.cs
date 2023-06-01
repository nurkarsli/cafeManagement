using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace cafeManagement
{
    public partial class Kasa : Form
    {
        public Kasa()
        {
            InitializeComponent();
        }
        private string connectionString = ConfigurationManager.ConnectionStrings["cafeManagement.Properties.Settings.CafeDbConnectionString"].ConnectionString;


        private void Kasa_Load(object sender, EventArgs e)
        {
            FillSiparisler();
            Hesaplamalar();
        }

        private void FillSiparisler()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Siparişler tablosunu çekiyoruz
                string query = "SELECT * FROM Siparisler";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                // Verileri DataTable'a dolduruyoruz
                DataTable table = new DataTable();
                adapter.Fill(table);

                // DataTable'ı DataGridView'a bağlıyoruz
                dataGridView1.DataSource = table;
            }

        }

        private void Hesaplamalar()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Siparişler tablosundaki toplamTutar'ın toplamını bul
                string totalSumQuery = "SELECT SUM(toplamTutar) FROM Siparisler";
                SqlCommand totalSumCommand = new SqlCommand(totalSumQuery, connection);
                decimal totalSum = (decimal)totalSumCommand.ExecuteScalar();

                // Siparişler tablosundaki toplam sipariş sayısını bul
                string totalCountQuery = "SELECT COUNT(*) FROM Siparisler";
                SqlCommand totalCountCommand = new SqlCommand(totalCountQuery, connection);
                int totalCount = (int)totalCountCommand.ExecuteScalar();

                // totalSum ve totalCount değerlerini bir label'a veya başka bir kontrole yazdırabilirsiniz.
                // Örneğin:
                textBox1.Text = totalSum.ToString();
                textBox2.Text = totalCount.ToString();
                textBox3.Text = (totalSum / totalCount).ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form = new Form1();
            form.Show();
        }
    }
}
