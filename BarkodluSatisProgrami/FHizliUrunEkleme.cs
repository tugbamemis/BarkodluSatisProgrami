using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarkodluSatisProgrami
{
    public partial class FHizliUrunEkleme : Form
    {
        public FHizliUrunEkleme()
        {
            InitializeComponent();
        }
        SatisDBEntities db = new SatisDBEntities();

        // ürün adına göre arama işlemi yapılmaktadır
        private void txtUrunAra_TextChanged(object sender, EventArgs e)
        {
            if (txtUrunAra.Text!="")
            {
                string urunad = txtUrunAra.Text;
                var urunler = db.Urun.Where(a=> a.UrunAd.Contains(urunad)).ToList();
                gridUrunler.DataSource = urunler;
                CIslemler.GridDuzenle(gridUrunler);
            }
        }

        //Hızlı ürün ekleme butonlarına ürün eklemek için yazılmış kodlar
        private void gridUrunler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gridUrunler.Rows.Count>0)
            {
                string barkod = gridUrunler.CurrentRow.Cells["Barkod"].Value.ToString();
                string urunad = gridUrunler.CurrentRow.Cells["UrunAd"].Value.ToString();
                double fiyat = Convert.ToDouble(gridUrunler.CurrentRow.Cells["SatisFiyat"].Value.ToString());
                int id = Convert.ToInt16(lblButonId.Text);
                var guncellenecek = db.HizliUrun.Find(id);
                guncellenecek.Barkod = barkod;
                guncellenecek.UrunAd = urunad;
                guncellenecek.Fiyat = fiyat;
                db.SaveChanges();
                MessageBox.Show("Hızlı Ürün Eklendi");
                FUrunSatis f=(FUrunSatis)Application.OpenForms["Form1"];

                /* Eklenen ürünün ana ekrana gelmesi için programın kapatıp açılması gerekiyordu.
                 * Aşağıdaki kod ile programın tekrar başlatılmasına gerek kalmadı */
                if (f!=null)
                {
                    Button b = f.Controls.Find("btnHizliUrun"+ id,true).FirstOrDefault() as Button;
                    b.Text = urunad + "\n" + fiyat.ToString("C2");
                }
            }
        }

        private void chTumu_CheckedChanged(object sender, EventArgs e)
        {
            if (chTumu.Checked)
            {
                gridUrunler.DataSource = db.Urun.ToList();
                gridUrunler.Columns["AlisFiyat"].Visible = false;
                gridUrunler.Columns["SatisFiyat"].Visible = false;
                gridUrunler.Columns["KdvOrani"].Visible = false;
                gridUrunler.Columns["KdvTutari"].Visible = false;
                gridUrunler.Columns["Miktar"].Visible = false;
                CIslemler.GridDuzenle(gridUrunler);
            }
            else
            {
                gridUrunler.DataSource = null;
            }
           
        }

        private void HizliUrunEkleme_Load(object sender, EventArgs e)
        {

        }
    }
}
