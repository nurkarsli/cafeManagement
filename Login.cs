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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }


        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Lütfen kullanıcı adı ve parola giriniz.");
                // kullanıcı adı , parola veya herhangi biri boş ise hata döndürür
            }
            else
            {
                string connectionString = ConfigurationManager.ConnectionStrings["cafeManagement.Properties.Settings.CafeDbConnectionString"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Admin WHERE kullaniciAdi = @username AND parola = @password";
                    // kullanıcı adı @username 'e parola da @password değişkeni olarak atandı
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", textBox1.Text.Trim());
                        // @username textbox1.Text ile karşılaştırılıyor
                        cmd.Parameters.AddWithValue("@password", textBox2.Text.Trim());
                        // @password textbox2.Text ile karşılaştırılıyor
                        SqlDataAdapter sda = new SqlDataAdapter(cmd); //veri alımı
                        DataTable dtbl = new DataTable();
                        sda.Fill(dtbl);
                        if (dtbl.Rows.Count == 1) // eğer sonuç 1 e eşit ise 
                        {
                            Form1 objForm1 = new Form1();
                            //form1 oluşturuyor
                            this.Hide();
                            // login formu hide oluyor
                            objForm1.Show();
                            //form1 show ediliyor
                        }
                        else
                        {
                            MessageBox.Show("Kullanıcı adı veya parola hatalı.");
                            //eğer girilen kullanıcı bilgileri veri tabanındaki bilgiler ile eşitlenmezse hata döndürür
                        }
                    }
                }
            }
        }
    }
}
