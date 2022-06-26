using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarkodluSatisProgrami
{
    public partial class FStok : Form
    {
        public FStok()
        {
            InitializeComponent();
        }
        // Filtreleme işlemleri
        private void btnAra_Click(object sender, EventArgs e)
        {
            gridListe.DataSource = null;
            using (var db=new SatisDBEntities())
            {
                if (cmbIslemTuru.Text!="")
                {
                    string urungrubu = cmbUrunGrubu.Text;
                    if (cmbIslemTuru.SelectedIndex==0)
                    {
                        if (rdTumu.Checked)
                        {
                            db.Urun.OrderBy(x => x.Miktar).Load();
                            gridListe.DataSource=db.Urun.Local.ToBindingList();
                        }
                        else if (rdUrunGrubu.Checked)
                        {
                            db.Urun.Where(x => x.UrunGrup == urungrubu).OrderBy(x => x.Miktar).Load();
                            gridListe.DataSource = db.Urun.Local.ToBindingList();
                        }
                        else
                        {
                            MessageBox.Show("Lütfen Filtreleme Türü Seçiniz");
                        }
                    }
                    // Tarih aralığında filtreleme işlemleri sadece 2. durum seçili iken (stok giriş izleme) çalışmaktadır.
                    else if (cmbIslemTuru.SelectedIndex==1)
                    {
                        DateTime baslangic = DateTime.Parse(dateBaslangic.Value.ToShortDateString());
                        DateTime bitis = DateTime.Parse(dateBitis.Value.ToShortDateString());
                        bitis = bitis.AddDays(1);

                        if (rdTumu.Checked)
                        {
                            db.StokHareket.OrderByDescending(x => x.Tarih).Where(x => x.Tarih >= baslangic && x.Tarih <= bitis).Load();
                            gridListe.DataSource=db.StokHareket.Local.ToBindingList();
                        }
                        else if (rdUrunGrubu.Checked)
                        {
                            db.StokHareket.OrderByDescending(x => x.Tarih).Where(x => x.Tarih >= baslangic && x.Tarih <= bitis && x.UrunGrup.Contains(urungrubu)).Load();
                            gridListe.DataSource = db.StokHareket.Local.ToBindingList();
                        }
                        else
                        {
                            MessageBox.Show("Lütfen Filtreleme Türü Seçiniz");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen İşlem Türü Seçiniz");
                }
                CIslemler.GridDuzenle(gridListe);
            }
        }

        SatisDBEntities dba = new SatisDBEntities();
        //Form açıldığında ürün grubu comboxına veriler urungrubutbl tablosundan yüklenir
        private void FStok_Load(object sender, EventArgs e)
        {
            cmbUrunGrubu.DisplayMember = "UrunGrupAd";
            cmbUrunGrubu.ValueMember = "Id";
            cmbUrunGrubu.DataSource = dba.UrunGrubuTbl.ToList();
        }

        //Ürün arama işlemleri
        private void txtUrunAra_TextChanged(object sender, EventArgs e)
        {
            if (txtUrunAra.Text.Length>=3)
            {
                string urunad = txtUrunAra.Text;
                using (var db = new SatisDBEntities())
                {
                    if (cmbIslemTuru.SelectedIndex == 0)
                    {                    
                            db.Urun.Where(x => x.UrunAd.Contains(urunad)).Load();
                            gridListe.DataSource = db.Urun.Local.ToBindingList();                      
                    }
                    else if (cmbIslemTuru.SelectedIndex == 1)
                    {
                        db.StokHareket.Where(x => x.UrunAd.Contains(urunad)).Load();
                        gridListe.DataSource = db.StokHareket.Local.ToBindingList();
                    }
                }
                CIslemler.GridDuzenle(gridListe);
            }
        }

        private void cmbIslemTuru_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
