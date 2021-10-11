using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;

namespace VeriForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OleDbConnection baglanti = new OleDbConnection("Provider=Microsoft.ACE.Oledb.12.0; Data Source=data.accdb");
        OleDbDataAdapter adapter;
        OleDbDataReader reader;
        OleDbCommand komut;
        DataSet set;
        string personelId;


        private void Form1_Load(object sender, EventArgs e)
        {
            cmbCinsiyet.Items.Add("Erkek");
            cmbCinsiyet.Items.Add("Kadın");
            gridDoldur();
        }

        public void gridDoldur() 
        {
            try
            {
                baglanti.Close();
                adapter = new OleDbDataAdapter("SELECT * FROM Personel", baglanti);
                set = new DataSet();
                baglanti.Open();
                adapter.Fill(set, "Personel");
                gridPersonel.DataSource = set.Tables["Personel"];
                baglanti.Close();

                gridPersonel.Columns[0].Visible = false;
                gridPersonel.Columns[1].HeaderText = "Personel No";
                gridPersonel.Columns[2].HeaderText = "Ad";
                gridPersonel.Columns[3].HeaderText = "Soyad";
                gridPersonel.Columns[4].HeaderText = "Cinsiyet";
                gridPersonel.Columns[5].HeaderText = "Maaş";

                this.gridPersonel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception)
            {
            }
           
        }

        private void txtPersonelNo_TextChanged(object sender, EventArgs e)
        {

        }

        
        private void btnEkle_Click(object sender, EventArgs e) 
        {
            komut = new OleDbCommand();
            baglanti.Open();
            komut.Connection = baglanti;
            string Query = "INSERT INTO Personel (personelNo,personelAdi,personelSoyadi,personelCinsiyet,personelMaas) values (?,?,?,?,?);";
            komut = new OleDbCommand(Query, baglanti);
            komut.Parameters.Add("@personelNo", OleDbType.VarChar).Value = txtPersonelNo.Text;
            komut.Parameters.Add("@personelAd", OleDbType.VarChar).Value = txtPersonelAdi.Text;
            komut.Parameters.Add("@personelSoyadi", OleDbType.VarChar).Value = txtPersonelSoyadi.Text;
            komut.Parameters.Add("@personelCinsiyet", OleDbType.VarChar).Value = cmbCinsiyet.SelectedItem.ToString();
            komut.Parameters.Add("@personelMaas", OleDbType.VarChar).Value = txtMaas.Text;
            reader = komut.ExecuteReader();           
            baglanti.Close();
            gridDoldur();

            txtPersonelNo.Text = "";
            txtPersonelAdi.Text = "";
            txtPersonelSoyadi.Text = "";
            txtMaas.Text = "";
            cmbCinsiyet.Text = "";
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            komut = new OleDbCommand();
            baglanti.Open();
            komut.Connection = baglanti;
            string Query = "UPDATE PERSONEL SET personelNo=?,personelAdi=?,personelSoyadi=?,personelCinsiyet=?,personelMaas=? WHERE id=?";
            komut = new OleDbCommand(Query, baglanti);
            komut.Parameters.Add("@personelNo", OleDbType.VarChar).Value = txtPersonelNo.Text;
            komut.Parameters.Add("@personelAd", OleDbType.VarChar).Value = txtPersonelAdi.Text;
            komut.Parameters.Add("@personelSoyadi", OleDbType.VarChar).Value = txtPersonelSoyadi.Text;
            komut.Parameters.Add("@personelCinsiyet", OleDbType.VarChar).Value = cmbCinsiyet.SelectedItem.ToString();
            komut.Parameters.Add("@personelMaas", OleDbType.VarChar).Value = txtMaas.Text;
            komut.Parameters.Add("@id", OleDbType.Integer).Value = personelId;
            reader = komut.ExecuteReader();
            baglanti.Close();
            gridDoldur();

            personelId = "";
            txtPersonelNo.Text = "";
            txtPersonelAdi.Text = "";
            txtPersonelSoyadi.Text = "";
            txtMaas.Text = "";
            cmbCinsiyet.Text = "";
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            komut = new OleDbCommand();
            baglanti.Open();
            komut.Connection = baglanti;
            string Query = "DELETE FROM PERSONEL WHERE id=?";
            komut = new OleDbCommand(Query, baglanti);
            komut.Parameters.Add("@id", OleDbType.Integer).Value = personelId;
            komut.ExecuteNonQuery();
            baglanti.Close();
            gridDoldur();
            personelId = "";
        }

        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            try
            {
                gridPersonel.DataSource = "";
                set.Tables.Clear();
                baglanti.Close();
                adapter = new OleDbDataAdapter("SELECT * FROM Personel WHERE personelAdi LIKE '" + txtArama.Text + "%' ", baglanti);
                set = new DataSet();
                baglanti.Open();
                adapter.Fill(set, "Personel");
                gridPersonel.DataSource = set.Tables["Personel"];
                baglanti.Close();

                gridPersonel.Columns[0].Visible = false;
                gridPersonel.Columns[1].HeaderText = "Personel No";
                gridPersonel.Columns[2].HeaderText = "Ad";
                gridPersonel.Columns[3].HeaderText = "Soyad";
                gridPersonel.Columns[4].HeaderText = "Cinsiyet";
                gridPersonel.Columns[5].HeaderText = "Maaş";

                this.gridPersonel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception)
            {
            }
        }

        private void gridPersonel_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                cmbCinsiyet.Text = "";
                personelId = gridPersonel.SelectedRows[0].Cells[0].Value.ToString();
                txtPersonelNo.Text = gridPersonel.SelectedRows[0].Cells[1].Value.ToString();
                txtPersonelAdi.Text = gridPersonel.SelectedRows[0].Cells[2].Value.ToString();
                txtPersonelSoyadi.Text = gridPersonel.SelectedRows[0].Cells[3].Value.ToString();
                cmbCinsiyet.Text = gridPersonel.SelectedRows[0].Cells[4].Value.ToString();
                txtMaas.Text = gridPersonel.SelectedRows[0].Cells[5].Value.ToString();

            }
            catch
            {

            }
        }

        private void gridPersonel_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            gridPersonel.ClearSelection();
        }
    }
}
