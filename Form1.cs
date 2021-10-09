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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int vipkoltuksayi=0, ekonomikoltuksayi=0, businesskoltuksayi=0;
        int anlikvipkoltuksayisi=0, anlikekonomikoltuksayisi=0, anlikbusinesskoltuksayisi=0;
        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\koltuklar.accdb");

        OleDbCommand komut = new OleDbCommand();



        private void verileriGoster()
        {
            baglanti.Open();
            listView1.Items.Clear();
            OleDbCommand komut = new OleDbCommand();
            komut.Connection = baglanti;
            komut.CommandText = "Select * From koltuklar";
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {

                ListViewItem ekle = new ListViewItem();
                ekle.Text = oku["musteri"].ToString();
                ekle.SubItems.Add(oku["telefon"].ToString());
                ekle.SubItems.Add(oku["cinsiyet"].ToString());
                ekle.SubItems.Add(oku["nereden"].ToString());
                ekle.SubItems.Add(oku["nereye"].ToString());
                ekle.SubItems.Add(oku["koltukno"].ToString());
                ekle.SubItems.Add(oku["tarih"].ToString());
                ekle.SubItems.Add(oku["fiyat"].ToString());
                ekle.SubItems.Add(oku["vagon"].ToString());
                listView1.Items.Add(ekle);


            }
            baglanti.Close();

        }
        List<int> vipVagon = new List<int>();
        List<int> businessVagon = new List<int>();
        List<int> ekonomiVagon = new List<int>();
        List<string> NEREYE = new List<string>();
        List<string> NEREDEN = new List<string>();
        List<string> vagontipi = new List<string>();
        private Button tiklanan;

        private void kvgetir()
        {
            NEREDEN.Clear();
            NEREYE.Clear();
            baglanti.Open();
            OleDbCommand komut = new OleDbCommand();
            komut.Connection = baglanti;
            komut.CommandText = "Select vagon,koltukno,nereden,nereye From koltuklar";
            OleDbDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                NEREDEN.Add(oku[2].ToString());
                NEREYE.Add(oku[3].ToString());
                if (oku[0].ToString() == "VIP Vagon")
                {
                    vagontipi.Add("VIP Vagon");
                    vipVagon.Add(Convert.ToInt32(oku[1]));
                }
                else if (oku[0].ToString() == "Business Vagon")
                {
                    vagontipi.Add("Business Vagon");
                    businessVagon.Add(Convert.ToInt32(oku[1]));

                }
                else if (oku[0].ToString() == "Ekonomi Vagon")
                {
                    vagontipi.Add("Ekonomi Vagon");
                    ekonomiVagon.Add(Convert.ToInt32(oku[1]));
                }
                    

            }
            anlikvipkoltuksayisi = vipVagon.Count;
            anlikbusinesskoltuksayisi = businessVagon.Count;
            anlikekonomikoltuksayisi = ekonomiVagon.Count;
            baglanti.Close();

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbNereden.SelectedItem == cmbNereye.SelectedItem)
            {
                cmbNereden.Items.RemoveAt(cmbNereden.SelectedIndex);
                MessageBox.Show("Aynı şehire yolculuk edilemez.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }


        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbTren_SelectedIndexChanged(object sender, EventArgs e)
        {
            kvgetir();
            verileriGoster();
            kltkkntrl();
            switch (cmbTren.Text)
            {
                case "VIP Vagon":
                    KoltukDoldur(2, false);
                    vipkoltuksayi = 2 * 4;
                    break;
                case "Ekonomi Vagon":
                    KoltukDoldur(6, true);
                    ekonomikoltuksayi = (6 * 4) + 5;
                    break;
                case "Business Vagon":
                    KoltukDoldur(4, false);
                    businesskoltuksayi = 4 * 4;
                    break;

            }
            void KoltukDoldur(int sira, bool enarkaMi)
            {
               

            yavaslat:
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is Button)
                    {
                        Button btn = ctrl as Button;
                        if (btn.Text == "KAYDET")
                        {
                            continue;
                        }
                        else
                        {
                            this.Controls.Remove(ctrl);
                            goto yavaslat;
                        }
                    }
                }
                int koltukNo = 1;
                for (int i = 0; i < sira; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (enarkaMi == true)
                        {
                            if (i != sira - 1 && j == 2)
                            {
                                continue;
                            }

                        }
                        else
                        {
                            if (j == 2)
                                continue;
                        }

                        string nereden = cmbNereden.SelectedItem.ToString();
                        string nereye = cmbNereye.SelectedItem.ToString();
                        int indexNereden = NEREDEN.BinarySearch(nereden);
                        int indexNereye = NEREYE.BinarySearch(nereye);
                        List<int> koltukindexleri = new List<int>();
                        koltukindexleri.Clear();
                        bool varMi = false;
                        bool businessvarMi = false;
                        bool ekonomivarMi = false;
                        bool vipvarMi = false;
                        for (int k = 0; k < NEREDEN.Count; k++)
                        {
                            if (NEREDEN[k] == nereden && NEREYE[k] == nereye)
                            {
                                if(vagontipi[k]=="VIP Vagon")
                                {
                                    vipvarMi = true;
                                }
                                if (vagontipi[k] == "Business Vagon")
                                {
                                    businessvarMi = true;
                                }
                                if (vagontipi[k] == "Ekonomi Vagon")
                                {
                                    ekonomivarMi = true;
                                }
                                koltukindexleri.Add(k);
                                varMi = true;
                            }

                        }
                        if (varMi)
                        {

                            if (cmbTren.SelectedIndex == 0)
                            {
                                this.Width = Convert.ToInt16(1378);
                                this.Height = Convert.ToInt16(495);
                                Button koltuk = new Button();
                                koltuk.Name = "button" + koltukNo.ToString();
                                koltuk.Height = koltuk.Width = 40;
                                koltuk.Top = 30 + (i * 45);
                                koltuk.Left = 1100 + (j * 45);
                                koltuk.Text = koltukNo.ToString();
                                koltukNo++;
                                koltuk.ContextMenuStrip = contextMenuStrip1;
                                koltuk.MouseDown += Koltuk_MouseDown;
                                this.Controls.Add(koltuk);
                                if (vipvarMi)
                                {
                                    if (vipVagon.Count > 0)
                                    {
                                        for (int k = 0; k < koltukindexleri.Count; k++)
                                        {
                                            if (koltukNo - 1 == vipVagon[koltukindexleri[k]])
                                            {
                                                koltuk.Enabled = false;
                                                koltuk.BackColor = Color.Red;
                                            }

                                        }
                                    }
                                }
                               
      
                            }
                            else if (cmbTren.SelectedIndex == 1)
                            {
                                this.Width = Convert.ToInt16(1378);
                                this.Height = Convert.ToInt16(495);
                                Button koltuk = new Button();
                                koltuk.Name = "button" + koltukNo.ToString();
                                koltuk.Height = koltuk.Width = 40;
                                koltuk.Top = 30 + (i * 45);
                                koltuk.Left = 1100 + (j * 45);
                                koltuk.Text = koltukNo.ToString();
                                koltukNo++;
                                koltuk.ContextMenuStrip = contextMenuStrip1;
                                koltuk.MouseDown += Koltuk_MouseDown;
                                this.Controls.Add(koltuk);
                                if (businessvarMi)
                                {
                                    if (businessVagon.Count > 0)
                                    {
                                        for (int k = 0; k < koltukindexleri.Count; k++)
                                        {
                                            if (koltukNo - 1 == businessVagon[koltukindexleri[k]])
                                            {
                                                koltuk.Enabled = false;
                                                koltuk.BackColor = Color.Red;
                                            }
                                        }
                                    }
                                }
                                

                            }
                            else if (cmbTren.SelectedIndex == 2)
                            {
                                this.Width = Convert.ToInt16(1378);
                                this.Height = Convert.ToInt16(495);
                                Button koltuk = new Button();
                                koltuk.Name = "button" + koltukNo.ToString();
                                koltuk.Height = koltuk.Width = 40;
                                koltuk.Top = 30 + (i * 45);
                                koltuk.Left = 1100 + (j * 45);
                                koltuk.Text = koltukNo.ToString();
                                koltukNo++;
                                koltuk.ContextMenuStrip = contextMenuStrip1;
                                koltuk.MouseDown += Koltuk_MouseDown;
                                this.Controls.Add(koltuk);
                                if (ekonomivarMi)
                                {
                                    if (ekonomiVagon.Count > 0)
                                    {
                                        for (int k = 0; k < koltukindexleri.Count; k++)
                                        {
                                            if (koltukNo - 1 == ekonomiVagon[koltukindexleri[k]])
                                            {
                                                koltuk.Enabled = false;
                                                koltuk.BackColor = Color.Red;
                                            }
                                        }
                                    }

                                }


                            }

                        }
                        else
                        {
                            if (cmbTren.SelectedIndex == 0)
                            {
                                this.Width = Convert.ToInt16(1378);
                                this.Height = Convert.ToInt16(495);
                                Button koltuk = new Button();
                                koltuk.Name = "button" + koltukNo.ToString();
                                koltuk.Height = koltuk.Width = 40;
                                koltuk.Top = 30 + (i * 45);
                                koltuk.Left = 1100 + (j * 45);
                                koltuk.Text = koltukNo.ToString();
                                koltukNo++;
                                koltuk.ContextMenuStrip = contextMenuStrip1;
                                koltuk.MouseDown += Koltuk_MouseDown;
                                this.Controls.Add(koltuk);


                            }
                            else if (cmbTren.SelectedIndex == 1)
                            {
                                this.Width = Convert.ToInt16(1378);
                                this.Height = Convert.ToInt16(495);
                                Button koltuk = new Button();
                                koltuk.Name = "button" + koltukNo.ToString();
                                koltuk.Height = koltuk.Width = 40;
                                koltuk.Top = 30 + (i * 45);
                                koltuk.Left = 1100 + (j * 45);
                                koltuk.Text = koltukNo.ToString();
                                koltukNo++;
                                koltuk.ContextMenuStrip = contextMenuStrip1;
                                koltuk.MouseDown += Koltuk_MouseDown;
                                this.Controls.Add(koltuk);

                            }
                            else if (cmbTren.SelectedIndex == 2)
                            {
                                this.Width = Convert.ToInt16(1378);
                                this.Height = Convert.ToInt16(495);
                                Button koltuk = new Button();
                                koltuk.Name = "button" + koltukNo.ToString();
                                koltuk.Height = koltuk.Width = 40;
                                koltuk.Top = 30 + (i * 45);
                                koltuk.Left = 1100 + (j * 45);
                                koltuk.Text = koltukNo.ToString();
                                koltukNo++;
                                koltuk.ContextMenuStrip = contextMenuStrip1;
                                koltuk.MouseDown += Koltuk_MouseDown;
                                this.Controls.Add(koltuk);

                            }
                        }
                    }

                }

            }
            Button tiklanan;

        }


        public int MultiSimple { get; private set; }

        private void Koltuk_MouseDown(object sender, MouseEventArgs e)
        {
            tiklanan = sender as Button;

            {




            }



        }



        private void Form1_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            kvgetir();
            verileriGoster();
            kltkkntrl();
            OleDbConnection baglanti = new OleDbConnection();
            baglanti.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Bilgi.accdb";
            OleDbCommand komut = new OleDbCommand();
            komut.CommandText = "Select * from Bilgi";
            komut.Connection = baglanti;
            komut.CommandType = CommandType.Text;
            OleDbDataReader dr;
            baglanti.Open();
            dr = komut.ExecuteReader();

            while (dr.Read())
            {

                cmbNereden.DataSource = null;
                cmbNereden.Items.Add(dr["sehir"]);
                cmbNereye.Items.Add(dr["sehir"]);



            }
            baglanti.Close();

        }

        private void rezerveEtToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (cmbTren.SelectedIndex == 0)
            {
                anlikvipkoltuksayisi++;


            }
            else if (cmbTren.SelectedIndex == 1)
            {
                anlikbusinesskoltuksayisi++;


            }
            else if (cmbTren.SelectedIndex == 2)
            {
                anlikekonomikoltuksayisi++;



            }

            kltkkntrl();
            if (cmbTren.SelectedIndex == -1 || cmbNereden.SelectedIndex == -1 || cmbNereye.SelectedIndex == -1)
            {
                MessageBox.Show("Lütfen Önce Gerekli Alanları Doldurunuz..");
                return;
            }
            if (kltkkntrl())
            {
                MessageBox.Show("Bu Vagon Rezervasyona Kapanmıştır");
            }
            else
            {
                Form2 kf = new Form2();
                kf.nereden = cmbNereden.SelectedItem.ToString();
                kf.nereye = cmbNereye.SelectedItem.ToString();
                kf.vagon = cmbTren.SelectedItem.ToString();
                kf.tarih = dtpTarih.Text;
                kf.fiyat = nudFiyat.Value.ToString();
                kf.koltukNu = Convert.ToInt32(tiklanan.Text);
                DialogResult sonuc = kf.ShowDialog();
                if (sonuc == DialogResult.OK)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = string.Format("{0} {1}", kf.txtIsim.Text, kf.txtSoyisim.Text);
                    lvi.SubItems.Add(kf.mskdTelefon.Text);
                    if (kf.rdbBay.Checked)
                    {
                        lvi.SubItems.Add("Bay");
                        tiklanan.BackColor = Color.Blue;
                        tiklanan.Enabled = false;

                    }
                    if (kf.rdbBayan.Checked)
                    {
                        lvi.SubItems.Add("Bayan");
                        tiklanan.BackColor = Color.Pink;
                        tiklanan.Enabled = false;

                    }
                    lvi.SubItems.Add(cmbNereden.Text);
                    lvi.SubItems.Add(cmbNereye.Text);
                    lvi.SubItems.Add(tiklanan.Text);
                    lvi.SubItems.Add(dtpTarih.Text);
                    lvi.SubItems.Add(nudFiyat.Value.ToString());
                    lvi.SubItems.Add(cmbTren.Text);
                    listView1.Items.Add(lvi);

                }
            }


        }
        bool kltkkntrl()
        {
            if (cmbTren.SelectedIndex == 0)
            {
                if (anlikvipkoltuksayisi >= (vipkoltuksayi * 0.7))
                {
                    this.Width = Convert.ToInt16(1002);
                    this.Height = Convert.ToInt16(495);
                    return true;
                }


            }
            else if (cmbTren.SelectedIndex == 1)
            {
                if (anlikbusinesskoltuksayisi >= (businesskoltuksayi * 0.7))
                {
                    this.Width = Convert.ToInt16(1002);
                    this.Height = Convert.ToInt16(495);
                    return true;
                }


            }
            else if (cmbTren.SelectedIndex == 2)
            {
                if (anlikekonomikoltuksayisi >= (ekonomikoltuksayi * 0.7))
                {
                    this.Width = Convert.ToInt16(1002);
                    this.Height = Convert.ToInt16(495);
                    return true;


                }
            }
            return false;
        }
        private void cmbNereye_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbNereye.SelectedItem == cmbNereden.SelectedItem)
            {
                cmbNereye.Items.RemoveAt(cmbNereye.SelectedIndex);
                MessageBox.Show("Aynı şehire yolculuk edilemez.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
    
    
