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
    public partial class FUrunGirisi : Form
    {
        public FUrunGirisi()
        {
            InitializeComponent();
        }

        private void UrunGirisi_Load(object sender, EventArgs e)
        {
            txtUrunSayisi.Text = db.Urun.Count().ToString();
            gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(20).ToList();
            CIslemler.GridDuzenle(gridUrunler);
            UGrup();
        }
        SatisDBEntities db= new SatisDBEntities();
        //Barkod numarası girilen ürün bilgileri ekrana gelir

        private void txtBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barkod = txtBarkod.Text.Trim();
                if (db.Urun.Any(a => a.Barkod == barkod))
                {
                    var urun=db.Urun.Where(a=>a.Barkod==barkod).SingleOrDefault();
                    txtUrunAdi.Text = urun.UrunAd;
                    txtAciklama.Text= urun.Aciklama; 
                    cmbUrunGrubu.Text = urun.UrunGrup;
                    txtAlisFiyati.Text = urun.AlisFiyat.ToString();
                    txtSatisFiyati.Text = urun.SatisFiyat.ToString();
                    txtMiktar.Text=urun.Miktar.ToString();
                    txtKdvOrani.Text = urun.KdvOrani.ToString();
                }
                else
                {
                    MessageBox.Show("Ürün kaydı bulunamadı,kaydedebilirsiniz...");
                }
            }
        }
        // Ürün kayıt(Urun tablosuna)
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            // girilen barkod numarası varsa entere basıldığında barkod numarası ile ilgili bilgiler ekrana gelerek güncelleme işlemi yapılmaktadır.
            if (txtBarkod.Text!="" && txtUrunAdi.Text!="" && cmbUrunGrubu.Text != "" && txtAlisFiyati.Text != "" && txtSatisFiyati.Text != "" && txtKdvOrani.Text != "" && txtMiktar.Text != "" )
            {
                if (db.Urun.Any(a=>a.Barkod==txtBarkod.Text))
                {
                    var guncelle=db.Urun.Where(a => a.Barkod == txtBarkod.Text).SingleOrDefault();
                    guncelle.UrunAd = txtUrunAdi.Text;
                    guncelle.Aciklama = txtAciklama.Text;
                    guncelle.UrunGrup = cmbUrunGrubu.Text;
                    guncelle.AlisFiyat = Convert.ToDouble(txtAlisFiyati.Text);
                    guncelle.SatisFiyat = Convert.ToDouble(txtSatisFiyati.Text);
                    guncelle.Miktar += Convert.ToDouble(txtMiktar.Text);
                    guncelle.KdvOrani = Convert.ToInt32(txtKdvOrani.Text);
                    guncelle.KdvTutari = Math.Round(CIslemler.DoubleYap(txtSatisFiyati.Text) * Convert.ToInt32(txtKdvOrani.Text) / 100, 2);
                    guncelle.Birim = "Adet";
                    guncelle.Tarih = DateTime.Now;
                    guncelle.Kullanici = lblKullanici.Text;
                    db.SaveChanges();
                    MessageBox.Show("Ürün Güncelleme Başarılı");
                    gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(20).ToList();
                }
                else
                {
                    Urun urun = new Urun();
                    urun.Barkod = txtBarkod.Text;
                    urun.UrunAd = txtUrunAdi.Text;
                    urun.Aciklama = txtAciklama.Text;
                    urun.UrunGrup = cmbUrunGrubu.Text;
                    urun.AlisFiyat = Convert.ToDouble(txtAlisFiyati.Text);
                    urun.SatisFiyat = Convert.ToDouble(txtSatisFiyati.Text);
                    urun.Miktar = Convert.ToDouble(txtMiktar.Text);
                    urun.KdvOrani = Convert.ToInt32(txtKdvOrani.Text);
                    urun.KdvTutari = Math.Round(CIslemler.DoubleYap(txtSatisFiyati.Text) * Convert.ToInt32(txtKdvOrani.Text) / 100, 2);
                    urun.Birim = "Adet";
                    urun.Tarih = DateTime.Now;
                    urun.Kullanici = lblKullanici.Text;
                    db.Urun.Add(urun);
                    db.SaveChanges();

                    if (txtBarkod.Text.Length == 8)
                    {
                        var ozelbarkod = db.Barkod.First();
                        ozelbarkod.BarkodNo += 1;
                        db.SaveChanges();
                    }
                   
                    
                    gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(20).ToList();
                    CIslemler.GridDuzenle(gridUrunler);
                }
                CStokIslemleri.StokHareketleri(txtBarkod.Text,txtUrunAdi.Text,"Adet",Convert.ToDouble(txtMiktar.Text),cmbUrunGrubu.Text,lblKullanici.Text);
                Temizle();
            }
            else
            {
                MessageBox.Show("Lütfen Boşlukları Doldurunuz...");
                txtBarkod.Focus();
            }
        }

        //Ürün arama işlemi
        private void txtUrunAra_TextChanged(object sender, EventArgs e)
        {
            if (txtUrunAra.Text.Length>=2)
            {
                string urunad = txtUrunAra.Text;
                gridUrunler.DataSource = db.Urun.Where(a => a.UrunAd.Contains(urunad)).ToList();
                CIslemler.GridDuzenle(gridUrunler);
            }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        private void Temizle()
        {
            txtBarkod.Clear();
            txtUrunAdi.Clear();
            txtAciklama.Clear();
            txtAlisFiyati.Text = "0";
            txtSatisFiyati.Text = "0";
            txtMiktar.Text = "0";
            txtKdvOrani.Text = "8";
            txtBarkod.Focus();
        }
        public void UGrup()
        {
            cmbUrunGrubu.DisplayMember = "UrunGrupAd";
            cmbUrunGrubu.ValueMember = "Id";
            cmbUrunGrubu.DataSource = db.UrunGrubuTbl.OrderBy(a => a.UrunGrupAd).ToList();
        }
        private void btnUrunGrubu_Click(object sender, EventArgs e)
        {
            FUrunGrubu u = new FUrunGrubu();
            u.ShowDialog();
        }

        //Benzersiz bir barkod numarası oluşturulması için kodlar.
        private void btnBarkod_Click(object sender, EventArgs e)
        {
            var barkodno = db.Barkod.First();
            int karakterler = barkodno.BarkodNo.ToString().Length;
            string sifirlar = string.Empty;
            for (int i = 0; i < 8- karakterler; i++)
            {
                sifirlar = sifirlar + "0";
            }
            string olusanbarkod = sifirlar + barkodno.BarkodNo.ToString();
            txtBarkod.Text = olusanbarkod;
            txtUrunAdi.Focus();
        }
       
        //Virgüllü sayı yazmak için kod    
        private void txtSatisFiyati_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08 && e.KeyChar != (char)44 && e.KeyChar != (char)45)
            {
                e.Handled = true;
            }
        }
        // DATAGRİDVİEW üzerinde ilgili satırın üzerine geldindiğinde sağ tık ile silme işlemi yapılmaktadır.
        //Ayrıca silinen ürün hızlı ürün ekleme butonlarında var ise ordan da silme işlemi gerçekleştirilmektedir.
        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gridUrunler.Rows.Count>0)
            {
                int urunid = Convert.ToInt32(gridUrunler.CurrentRow.Cells["UrunId"].Value.ToString());
                string urunad = gridUrunler.CurrentRow.Cells["UrunAd"].Value.ToString();
                string barkod = gridUrunler.CurrentRow.Cells["Barkod"].Value.ToString();
                
                DialogResult onay = MessageBox.Show(urunad + " ürününü silmek istiyor musunuz?", "Ürün Silme İşlemi", MessageBoxButtons.YesNo);
                if (onay == DialogResult.Yes)
                {
                    var urun = db.Urun.Find(urunid);
                    db.Urun.Remove(urun);
                    db.SaveChanges();
                    var hizliurun=db.HizliUrun.Where(x=>x.Barkod==barkod).SingleOrDefault();
                    hizliurun.Barkod = "-";
                    hizliurun.UrunAd = "-";
                    hizliurun.Fiyat = 0;
                    db.SaveChanges();
                    MessageBox.Show("Ürün silindi");
                    gridUrunler.DataSource = db.Urun.OrderByDescending(a => a.UrunId).Take(20).ToList();
                    CIslemler.GridDuzenle(gridUrunler);
                    txtBarkod.Focus();
                }
            }
            
        }
    }
}
