using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;


namespace cafeManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) 
        {
            if (MasaDurumlariniGuncelle())
            {
                MessageBox.Show("Masa durumları başarıyla eşitlendi.");
            }
            else
            {
                MessageBox.Show("Masa durumları eşitlenemedi.");
            }
        }//masa formu için veri tabanı bağlantısı load işleminde gerçekleştirilir bu işlem
         //MasaDurumlariniGuncelle fonksiyonu ile gerçekleştirilir.
         //veri tabanından masa bilgileri senkronize edilir



        private bool MasaDurumlariniGuncelle()
        {
            // Veritabanı bağlantınızın connection string'i
            string connectionString = ConfigurationManager.ConnectionStrings["cafeManagement.Properties.Settings.CafeDbConnectionString"].ConnectionString;
            //bu connection string ADO.NET için app.config dosyasında tanımdağımız connection stringi form içerisinde okumamızı sağlar
            string query = "SELECT masaAdi, durum FROM Masa";
            // SQL sorgumuz gelir
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Her bir row için, belirli bir butonun bulunup bulunamadığını kontrol ediyoruz
                bool allButtonsFound = true;

                while (reader.Read())
                {
                    string masaAdi = reader["masaAdi"].ToString();
                    string durum = reader["durum"].ToString();
                    //veri tabanında masa adı ile texti ve durum bilgisi ile dolu boş olması okunur ardından strınge çevrilir
                    // Butonları buluyoruz (tüm kontrol öğeleri içerisinde dolaşıyoruz)
                    Button btn = FindButtonByText(this, masaAdi);
                    //buton textleri if döngüsüne girer. Masanın durumuna göre buton arkaplan renk bilgisi belirlenir
                    if (btn != null)
                    {
                        // Butonun arka plan rengini duruma göre belirliyoruz.
                        if (durum == "dolu")
                        {
                            btn.BackColor = Color.Firebrick;
                        }
                        else if (durum == "bos")
                        {
                            btn.BackColor = Color.SeaGreen;
                        }
                    }
                    //buton içi null ise false atar ve renkler değişmez
                    else
                    {
                        allButtonsFound = false;
                    }
                }
                //masa durumları güncellenmiş şekilde butonlarımız gelir
                reader.Close();
                return allButtonsFound;
            }
        }

        // form içerisinde bulunan butonların textlerini almamızı sağlar.
        public static Button FindButtonByText(Control parent, string text)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button btn && btn.Text == text)
                    return btn;

                Button result = FindButtonByText(c, text);
                if (result != null)
                    return result;
            }

            return null;
        }

        //butonlara tıklandığında adisyon formu açılması için aşağıda her buton için gerekli kodları yazdık.
        private void button12_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            //buton Text MasaAdı olarak kullanılabilir. çünkü buton textlerini masa adı ile eşleştirdik. 
            Adisyon adisyon = new Adisyon(btn.Text);
            adisyon.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);
            adisyon.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);

            adisyon.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);

            adisyon.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);

            adisyon.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);

            adisyon.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);

            adisyon.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);

            adisyon.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);

            adisyon.Show();
            this.Hide();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);

            adisyon.Show();
            this.Hide();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);

            adisyon.Show();
            this.Hide();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);

            adisyon.Show();
            this.Hide();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);

            adisyon.Show();
            this.Hide();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);
            adisyon.Show();
            this.Hide();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);
            adisyon.Show();
            this.Hide();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Adisyon adisyon = new Adisyon(btn.Text);
            adisyon.Show();
            this.Hide();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.Close();
            Login login = new Login();
            login.Show();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            this.Close();
            Kasa kasa = new Kasa();
            kasa.Show();
        }




    }
}
