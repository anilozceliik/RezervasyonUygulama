using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace RezervasyonUygulama
{
    public partial class Form2 : Form
    {
        private Form mstrblg;

        public Form2()
        {
            InitializeComponent();
        }
        public string vagon, nereden, nereye, tarih, cinsiyet, fiyat = string.Empty;
        public int koltukNu;
        void verilerikaydet()
        {
            string vtyolu = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\koltuklar.accdb";
            OleDbConnection baglanti = new OleDbConnection(vtyolu);
            baglanti.Open();
            string ekle = "insert into koltuklar(musteri,telefon,cinsiyet,nereden,nereye,koltukno,tarih,fiyat,vagon) values (@musteri,@telefon,@cinsiyet,@nereden,@nereye,@koltukno,@tarih,@fiyat,@vagon)";
            OleDbCommand komut = new OleDbCommand(ekle, baglanti);
            komut.Parameters.AddWithValue("@musteri", txtIsim.Text);
            komut.Parameters.AddWithValue("@telefon", mskdTelefon.Text);
            komut.Parameters.AddWithValue("@cinsiyet", cinsiyet);
            komut.Parameters.AddWithValue("@nereden", nereden);
            komut.Parameters.AddWithValue("@nereye", nereye);
            komut.Parameters.AddWithValue("@koltukno", koltukNu);
            komut.Parameters.AddWithValue("@tarih", tarih);
            komut.Parameters.AddWithValue("@fiyat", fiyat);
            komut.Parameters.AddWithValue("@vagon", vagon);
            komut.ExecuteNonQuery();
            baglanti.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }

        private void btnTamam_Click(object sender, EventArgs e)
        {
            if (rdbBay.Checked == true)
            {
                cinsiyet = "Bay";
            }
            else
                cinsiyet = "Bayan";

            verilerikaydet();
           
            Form_musteribilgiler mstrblg = new Form_musteribilgiler();
            mstrblg.Show();
            this.Hide();
            if (txtIsim.Text == "" && txtSoyisim.Text == "" && mskdTelefon.Text == "")
            {
                MessageBox.Show("Verilen Bilgileri Doldurunuz..");
            }
            else if (txtIsim.Text == "" && txtSoyisim.Text == "")
            {
                MessageBox.Show("Verilen Bilgileri Doldurunuz..");
            }
            else if (txtIsim.Text == "" && mskdTelefon.Text == "")
            {
                MessageBox.Show("Verilen Bilgileri Doldurunuz..");
            }
            else if (txtSoyisim.Text == "" && mskdTelefon.Text == "")
            {
                MessageBox.Show("Verilen Bilgileri Doldurunuz..");
            }
            else if (txtIsim.Text == "")
            {
                MessageBox.Show("İsminizi Giriniz..");
            }
            else if (txtSoyisim.Text== "")
            {
                MessageBox.Show("Soyisminizi Giriniz..");
            }
            else if (mskdTelefon.Text=="" )
            {
                MessageBox.Show("Telefon Numaranızı Giriniz..");
            
           
            }
            else if (txtIsim.Text != null && txtSoyisim.Text != null && mskdTelefon.Text != null )
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
                
                
            }




        }

        private Form_musteribilgiler Form_musteribilgiler()
        {
            throw new NotImplementedException();
        }
        public string trenadi = string.Empty;
        public int vipvagon,businessvagon,ekonomivagon;
        


        private void Form2_Load(object sender, EventArgs e)
        {
            
        }
    }
}
