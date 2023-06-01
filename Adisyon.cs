using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace cafeManagement
{
    public partial class Adisyon : Form
    {
        private string masaAdi;
        //masa adını diğer fonk içinde sürekli kullanacağımız için global olarak tanımlıyoruz
        private string connectionString = ConfigurationManager.ConnectionStrings["cafeManagement.Properties.Settings.CafeDbConnectionString"].ConnectionString;

        public Adisyon(string masaAdi)
        {
            InitializeComponent();
            listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
            //lıstbox1 içerisinde gösterilen kategorilerin hangisinin seçili olduğunu belirtiyor
            this.masaAdi = masaAdi;
        }

        private void Adisyon_Load(object sender, EventArgs e)
        {
            try
            {
                FillKategoriList();
                if (listBox1.Items.Count > 0)
                {
                    listBox1.SelectedIndex = 0; // ilk kategori seçili hale getirilir.
                }
                FillMenuTable();
                //seçili olan kategorinin id si ürünler tablosunda bulunan category id
                //ile eşitlenir ve menü gelir
                FillMasaInfo();
                // Form1'den gelen Masa adi ile masa bilgilerini sql sorgusu ile
                // getirip gerekli olan yerleri dolduruyoruz
                FillAdisyonDetayTable();
                //adisyon tablosuna ürün eklemek için kullanılan fonksiyon
                if (textBox2.Text == "dolu")
                {
                    Tutarhesapla();//eğer masa dolu ise adisyon tablosundaki bütün
                                   //tutarlar toplanır ve hesap kısmına yazdırılır.
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi verilir ve loglanır.
                MessageBox.Show("Form yüklenirken hata: " + ex.Message);
            }
            //try catch kullandık try içinde hata çıkarsa bu hata catch edilir ve hata mesajı gösterilir
        }


        private void FillKategoriList()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Kategori";
                SqlCommand command = new SqlCommand(query, connection);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    listBox1.Items.Add(reader["kategoriAdi"].ToString());
                    //kategori adını listbox içine ekle 
                }
            }
            // bu fonksiyon veri tabanından kategorileri sorgu ile getirip listbox1 içerisine yazdırır   
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FillMenuTable();
                //kategori değiştiğinde menüyü yeniden çağır
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi verilir ve loglanır.
                MessageBox.Show("Menü yüklenirken hata: " + ex.Message);
            }
        }

        private void FillMenuTable()
        {
            try
            {
                string selectedCategory = listBox1.SelectedItem.ToString();
                //listbox içinde bir kategori seçiyoruz ve kategory id ile ürünleri sorguluyoruz

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT urunAdi, tutar FROM Urun WHERE categoryId = (SELECT id FROM Kategori WHERE kategoriAdi = @kategoriAdi)";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@kategoriAdi", selectedCategory);

                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridView1.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi verilir ve loglanır.
                MessageBox.Show("Menü yüklenirken hata: " + ex.Message);
            }
            //listbox1 de seçili olan kategori adını alıyoruz bu kategori adını ürünler tablosunda sorguluyoruz
            //ve kategori id leri ile eşleştirip ürünleri getiriyoruz
        }

        private void FillMasaInfo()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    //global olarak tanımlanan masa adi ile sorgumuzu yapıyoruz bu masa adı ilk başta form1 de ki masa butonundan gelmişti
                    connection.Open();
                    string query = "SELECT * FROM Masa WHERE masaAdi = @masaAdi";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@masaAdi", this.masaAdi);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        textBox1.Text = reader["masaAdi"].ToString();
                        textBox2.Text = reader["durum"].ToString();
                        //masa bilgileri gerekli textboxt.text için eşitler
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi verilir ve loglanır.
                MessageBox.Show("Menü yüklenirken hata: " + ex.Message);
            }
            //bu fonksiyon elimizde bulunan masa adı bilgisi ile veritabanında sorgu yapar ve
            //masa durumu masa adı bilgilerini adisyon formuna yazdırır.
        }
        
        private void FillAdisyonDetayTable()
        {
            string masaAdi = textBox1.Text; // masa adı textBox'tan alınır

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Önce masaId ve masa durumunu bulmalıyız
                string queryForMasaId = "SELECT id, durum FROM Masa WHERE masaAdi = @masaAdi";
                SqlCommand command = new SqlCommand(queryForMasaId, connection);
                command.Parameters.AddWithValue("@masaAdi", masaAdi);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int masaId = reader.GetInt32(0); // id değerini alıyoruz
                        string masaDurum = reader.GetString(1); // durum değerini alıyoruz

                        reader.Close(); // mevcut reader'ı kapatmamız gerek çünkü yeni bir sorgu çalıştıracağız

                        DataTable table = new DataTable();

                        string queryForAdisyonDetay = "SELECT urunAdi, urunAdet, Tutar, tarih FROM AdisyonDetay WHERE masaId = @masaId";
                        SqlDataAdapter adapter = new SqlDataAdapter(queryForAdisyonDetay, connection);
                        adapter.SelectCommand.Parameters.AddWithValue("@masaId", masaId);
                        adapter.Fill(table);


                        dataGridView2.DataSource = table;
                    }
                }
            }
            //adisyon tablosunu doldurur eğer dolu ise masa id sini kullanarak adisyon verilerini getirir
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string urunAdi = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            decimal tutar = Convert.ToDecimal(dataGridView1.Rows[e.RowIndex].Cells[1].Value);


            // Çift tıklanan ürünün detaylarını sadece dataGridView2'ye ekliyoruz.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Önce masaId'yi bulmalıyız
                string queryForMasaId = "SELECT id FROM Masa WHERE masaAdi = @masaAdi";
                SqlCommand command = new SqlCommand(queryForMasaId, connection);
                command.Parameters.AddWithValue("@masaAdi", masaAdi);

                int masaId = (int)command.ExecuteScalar(); // executeScalar kullanarak dönen değeri alıyoruz

                // yeni bir AdisyonDetay ekliyoruz
                string insertQuery = "INSERT INTO AdisyonDetay (masaId, urunAdi, urunAdet, tutar, tarih) VALUES (@masaId, @urunAdi, @urunAdet, @tutar, @tarih)";
                SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@masaId", masaId);
                insertCommand.Parameters.AddWithValue("@urunAdi", urunAdi);
                insertCommand.Parameters.AddWithValue("@urunAdet", 1);
                insertCommand.Parameters.AddWithValue("@tutar", tutar);
                insertCommand.Parameters.AddWithValue("@tarih", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                insertCommand.ExecuteNonQuery(); //verıler eklendıgınde masa dolu olur ve fonka gıdılır
            }
            FillAdisyonDetayTable();
            //menüdeki ürünlere 2 kere tıkladığımızda adisyon tablosuna ekleme yapan fonksiyondur

        }

        private void button2_Click(object sender, EventArgs e)
            //masa güncelle butonu
        {
            string masaAdi = textBox1.Text; // masa adı textBox'tan alınır

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Önce masaId'yi bulmalıyız
                string queryForMasaId = "SELECT id FROM Masa WHERE masaAdi = @masaAdi";
                SqlCommand command = new SqlCommand(queryForMasaId, connection);
                command.Parameters.AddWithValue("@masaAdi", masaAdi);
                int masaId = (int)command.ExecuteScalar(); // executeScalar kullanarak dönen değeri alıyoruz

                // masa durumunu dolu olarak güncelliyoruz
                string updateQuery = "UPDATE Masa SET durum = 'dolu' WHERE id = @masaId";
                SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                updateCommand.Parameters.AddWithValue("@masaId", masaId);
                updateCommand.ExecuteNonQuery();

                // masanın durumunu güncelledikten sonra, tekrar masa bilgilerini güncelliyoruz
                FillMasaInfo();

                decimal toplamTutar = 0;
                Tutarhesapla();
            }

            this.Close();
            Form1 form1 = new Form1();
            form1.Show();
        }
        private void Tutarhesapla()
        {
            decimal toplamTutar = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                toplamTutar += Convert.ToDecimal(row.Cells[2].Value); // 'tutar' sütununu topluyoruz
                label7.Text = "Toplam Tutar: " + toplamTutar.ToString("F2") + " TL";
                label8.Text = "Kişi başı: " + (toplamTutar / 2).ToString("F2") + " TL";
                label9.Text = "Kişi başı: " + (toplamTutar / 3).ToString("F2") + " TL";
                label10.Text = "Kişi başı: " + (toplamTutar / 4).ToString("F2") + " TL";
            }
        }

        private void button1_Click(object sender, EventArgs e)
            //adisyonu kapat
        {
            int masaId;
            // Masa durumunu bos olarak güncelle ve masaId'yi al
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Masa SET durum = 'bos' WHERE masaAdi = @masaAdi; SELECT id FROM Masa WHERE masaAdi = @masaAdi";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@masaAdi", masaAdi);
                command.ExecuteNonQuery();

                masaId = (int)command.ExecuteScalar();
            }

            decimal toplamTutar = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                toplamTutar += Convert.ToDecimal(row.Cells[2].Value); // 'tutar' sütunu
            }

            int adisyonDetayId = 0;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO AdisyonDetay (urunAdi, urunAdet, tutar, tarih, masaId) VALUES (@urunAdi, @urunAdet, @tutar, @tarih, @masaId)";
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                    var urunAdi = row.Cells["urunAdi"].Value;
                    var urunAdet = row.Cells["urunAdet"].Value;
                    var tutar = row.Cells["tutar"].Value;
                    var tarih = row.Cells["tarih"].Value;

                    if (urunAdi == null || urunAdet == null || tutar == null || tarih == null)
                        continue; // Null değerler varsa bu satırı atlayıp bir sonraki satıra geçiyoruz

                    insertCommand.Parameters.AddWithValue("@urunAdi", urunAdi.ToString());
                    insertCommand.Parameters.AddWithValue("@urunAdet", Convert.ToInt32(urunAdet));
                    insertCommand.Parameters.AddWithValue("@tutar", Convert.ToDecimal(tutar));
                    insertCommand.Parameters.AddWithValue("@tarih", Convert.ToDateTime(tarih));
                    insertCommand.Parameters.AddWithValue("@masaId", masaId);
                    insertCommand.ExecuteNonQuery();

                    SqlCommand commandForLastId = new SqlCommand("SELECT MAX(id) FROM AdisyonDetay", connection);
                    int lastId = (int)commandForLastId.ExecuteScalar();
                    adisyonDetayId = lastId; // lastId değişkeni şimdi son eklenen id'yi içerir
                }
            }// masa kapatılmadan önce ürün eklendiğinde tekrar masa güncelleme yaparak toplam price hesaplatmamak ıcın
            //adisyon kapat butonuna tıklayarak son eklenen verileri de adisyon tablosuna eklemiş oluruz

            // dataGridView2'yi(adisyon) temizle
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();

            // Siparişler tablosuna yeni bir sipariş ekleyin
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string insertQuery = "INSERT INTO Siparisler (toplamTutar, masaId) VALUES (@toplamTutar, @masaId)";
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@toplamTutar", toplamTutar);
                command.Parameters.AddWithValue("@masaId", masaId);
                command.ExecuteNonQuery();
            }//aldığımız verileri toplam tutar ve masaid ile birlikte ciro hesaplamak için siparişler tablosuna ekliyoruz

            // AdisyonDetay tablosundan o masaId nin eşit olduğu tüm verileri sil
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM AdisyonDetay WHERE masaId = @masaId";
                SqlCommand command = new SqlCommand(deleteQuery, connection);
                command.Parameters.AddWithValue("@masaId", masaId);
                command.ExecuteNonQuery();
            }//masa artık boş olduğu için adisyon tablosundaki masa ile ilgili verileri temizliyoruz

            // Son olarak, datagridview2'yi ve Masa durumunu güncelle
            FillAdisyonDetayTable();
            FillMasaInfo();
            this.Close();
            Form1 form1 = new Form1();
            form1.Show();
        }


        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }



    }

}
