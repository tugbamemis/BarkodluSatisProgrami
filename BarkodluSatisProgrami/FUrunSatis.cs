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
    public partial class FUrunSatis : Form
    {
        public FUrunSatis()
        {
            InitializeComponent();
        }
        // Program çalıştığında 5-10-20-50-100 ve 200 tl'lik tuşların çalışması
        private void Form1_Load(object sender, EventArgs e)
        {
            HizliUrunButon();
            btn5.Text = 5.ToString("C2");
            btn10.Text = 10.ToString("C2");
            btn20.Text = 20.ToString("C2");
            btn50.Text = 50.ToString("C2");
            btn100.Text = 100.ToString("C2");
            btn200.Text = 200.ToString("C2");
        }
        private void HizliUrunButon()
        {
            var hizliurun = db.HizliUrun.ToList();
            foreach (var item in hizliurun)
            {
                Button bh = this.Controls.Find("btnHizliUrun" + item.Id, true).FirstOrDefault() as Button;
                if (bh != null)
                {
                    double fiyat = CIslemler.DoubleYap(item.Fiyat.ToString());
                    bh.Text = item.UrunAd + "\n" + fiyat.ToString("C2");
                }
            }
        }


        private void HizliUrunClick(object sender, EventArgs e)
        {    // "-" işaretli Hızlı ürün ekleme tuşuna basıldığında ürün ekleme sayfası açılmaktadır.
            Button b = (Button)sender;
            //butonların adı "btnHizliUrun" dür. Toplam 12 karakterdir. 12 karakterden sonra gelen karakterler butonun numarasını verir.
            int butonid = Convert.ToInt16(b.Name.ToString().Substring(12, b.Name.Length - 12));
            if (b.Text.ToString().StartsWith("-"))
            {
                FHizliUrunEkleme h = new FHizliUrunEkleme();
                h.lblButonId.Text = butonid.ToString();
                h.ShowDialog();
            }
            else
            {
                //Hızlı ürün tuşuna basıldığında ürünün adı barkod numarsı ve miktarı listeye eklneir

                var urunbarkod = db.HizliUrun.Where(a => a.Id == butonid).Select(a => a.Barkod).FirstOrDefault();
                var urun = db.Urun.Where(a => a.Barkod == urunbarkod).FirstOrDefault();
                UrunGetirListeye(urun, urunbarkod, Convert.ToDouble(txtMiktar.Text));
                GenelToplam();
            }

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        SatisDBEntities db = new SatisDBEntities();

        /*  Satın alınmak istenen ürünün miktarı ve barkod numarası ilgili textboxlara girildikten 
            sonra enter tuşuna basılarak listeye ürün ekleme işlemi  yapılmaktadır. */
        private void txtBarkod_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barkod = txtBarkod.Text.Trim();
                if (barkod.Length <= 2)
                {
                    txtMiktar.Text = barkod;
                    txtBarkod.Clear();
                    txtBarkod.Focus();
                }
                /* 
                 * Aşağıdaki kodlarda eklenmek istenen ürün kilogram, gram vb cinsinden olan ürünler 
                 * için oluşturulmuştur. 
                 * Barkod numarası "2700001015006" örneğinde olduğu gibi 13 numaradan oluşmaktadır.
                 
                 Örnek barkod numarası "2700001015006"
                    * "27" terazi öne ek numarasını (genellikle 27, 28 , 29 numaralarından oluşmaktadır.)   
                    * "00001" ürün kodu
                    * "01500" gramaj kodu 1500 gr
                    * "6" kontrol kodudur. Herhangi bir rakam olabilir. Bir önemi bulunmamaktadır.
                 */
                else
                {
                    if (db.Urun.Any(a => a.Barkod == barkod))
                    {
                        var urun = db.Urun.Where(a => a.Barkod == barkod).FirstOrDefault();
                        UrunGetirListeye(urun, barkod, Convert.ToDouble(txtMiktar.Text));

                    }
                    else
                    {
                        int onek = Convert.ToInt32(barkod.Substring(0, 2));
                        if (db.Terazi.Any(a => a.TeraziOnEk == onek))
                        {
                            string teraziurunno = barkod.Substring(2, 5);
                            if (db.Urun.Any(a => a.Barkod == teraziurunno))
                            {
                                var urunterazi = db.Urun.Where(a => a.Barkod == teraziurunno).FirstOrDefault();
                                double miktarkg = Convert.ToDouble(barkod.Substring(7, 5)) / 1000;
                                UrunGetirListeye(urunterazi, teraziurunno, miktarkg);
                            }
                            // Girilen barkod numarasına dair ürün yoksa  bip sesi çıkması sağlanarak ürün ekleme sayfası açılır
                            else
                            {
                                Console.Beep(900, 2000);
                                MessageBox.Show("Kg Ürün Ekleme Sayfası");
                            }
                        }
                        else
                        {
                            Console.Beep(900, 2000);
                            FUrunGirisi a = new FUrunGirisi();
                            a.txtBarkod.Text = barkod;
                            a.ShowDialog();
                        }
                    }
                }
                gridSatisListesi.ClearSelection();
                GenelToplam();



            }
        }

        private void UrunGetirListeye(Urun urun, string barkod, double miktar)
        {
            int satirsayisi = gridSatisListesi.Rows.Count;
            // double miktar = Convert.ToDouble(txtMiktar.Text);
            bool eklenmismi = false;
            // Eğer aynı ürün bir kez daha okutulacaksa daha önce okutulmuş ürünün üzerine ekleme işlemi yapılmaktadır.
            if (satirsayisi > 0)
            {
                for (int i = 0; i < satirsayisi; i++)
                {
                    if (gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString() == barkod)
                    {
                        gridSatisListesi.Rows[i].Cells["Miktar"].Value = miktar + Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Miktar"].Value);
                        gridSatisListesi.Rows[i].Cells["Toplam"].Value = Math.Round(Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Miktar"].Value) * Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Fiyat"].Value), 2);
                        eklenmismi = true;

                    }
                }
            }
            // Barkodu girilen ÜRÜN okutulmamışsa yeni satır eklenerek ürünü listeye ekleme işlemi yapılmaktadır.
            if (!eklenmismi)
            {
                gridSatisListesi.Rows.Add();
                gridSatisListesi.Rows[satirsayisi].Cells["Barkod"].Value = barkod;
                gridSatisListesi.Rows[satirsayisi].Cells["UrunAdi"].Value = urun.UrunAd;
                gridSatisListesi.Rows[satirsayisi].Cells["UrunGrup"].Value = urun.UrunGrup;
                gridSatisListesi.Rows[satirsayisi].Cells["Birim"].Value = urun.Birim;
                gridSatisListesi.Rows[satirsayisi].Cells["Fiyat"].Value = urun.SatisFiyat;
                gridSatisListesi.Rows[satirsayisi].Cells["Miktar"].Value = miktar;
                gridSatisListesi.Rows[satirsayisi].Cells["Toplam"].Value = Math.Round(miktar * (double)urun.SatisFiyat, 2);
                gridSatisListesi.Rows[satirsayisi].Cells["AlisFiyat"].Value = urun.AlisFiyat;
                gridSatisListesi.Rows[satirsayisi].Cells["KdvTutari"].Value = urun.KdvTutari;

            }
        }

        // ******** Satın alınacak ürünlerin toplamı için oluşturulmuş metod *********
        private void GenelToplam()
        {

            double toplam = 0;
            for (int i = 0; i < gridSatisListesi.Rows.Count; i++)
            {
                toplam += Convert.ToDouble(gridSatisListesi.Rows[i].Cells["Toplam"].Value);

            }
            txtToplam.Text = toplam.ToString("C2");
            txtMiktar.Text = "1";
            txtBarkod.Clear();
            txtBarkod.Focus();


        }
        // Datagridview üzerinde bulunan çarpı işaretine baılınca silme işlemi ve toplam değerinde değişiklik için kodlar 
        private void gridSatisListesi_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 9)
            {
                gridSatisListesi.Rows.Remove(gridSatisListesi.CurrentRow);
                gridSatisListesi.ClearSelection();
                GenelToplam();
                txtBarkod.Focus();
            }
        }

        //Mouse sağ tıklama ile ürün silme işlemi
        private void btnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Button b = (Button)sender;
                if (!b.Text.StartsWith("-"))
                {
                    int butonid = Convert.ToInt16(b.Name.ToString().Substring(12, b.Name.Length - 12));
                    ContextMenuStrip s = new ContextMenuStrip();
                    ToolStripMenuItem sil = new ToolStripMenuItem();
                    sil.Text = "Temizle- Buton No:" + butonid.ToString();
                    sil.Click += Sil_Click;
                    s.Items.Add(sil);
                    this.ContextMenuStrip = s;
                }
            }
        }
        // Hızlı ürün ekleme butonlarından silinen ürün yerine butona "-" ve "0" yazması için kodlar
        private void Sil_Click(object sender, EventArgs e)
        {
            int butonid = Convert.ToInt16(sender.ToString().Substring(18, sender.ToString().Length - 18));
            var guncelle = db.HizliUrun.Find(butonid);
            guncelle.Barkod = "-";
            guncelle.UrunAd = "-";
            guncelle.Fiyat = 0;
            db.SaveChanges();
            double fiyat = 0;
            Button b = this.Controls.Find("btnHizliUrun" + butonid, true).FirstOrDefault() as Button;
            b.Text = "-" + "\n" + fiyat.ToString("C2");
        }

        //Numaratör Kullanımı
        private void Numarator_Click(object sender, EventArgs e)
        {
            // iki kez virgül tuşuna basılmaması için
            Button b = (Button)sender;
            if (b.Text == ",")
            {
                int virgul = txtNum.Text.Count(x => x == ',');
                if (virgul < 1)
                {
                    txtNum.Text += b.Text;
                }
            }
            // Yazılan en son rakamı silmek için 
            else if (b.Text == "←")
            {
                if (txtNum.Text.Length > 0)
                {
                    txtNum.Text = txtNum.Text.Substring(0, txtNum.Text.Length - 1);
                }
            }
            else
            {
                txtNum.Text += b.Text;
            }
        }

        // Numaratöre yazılan sayı adet tuşuna basıldığında miktar textboxına yazılmaktadır.
        private void btnAdet_Click(object sender, EventArgs e)
        {
            if (txtNum.Text != "")
            {
                txtMiktar.Text = txtNum.Text;
                txtNum.Clear();
                txtBarkod.Clear();
                txtBarkod.Focus();

            }
        }
        // Ödeme yapıldığında genel toplamdan düşme ve para üstü gösterme işlemleri
        private void btnOdeme_Click(object sender, EventArgs e)
        {
            if (txtNum.Text != "")
            {
                double sonuc = CIslemler.DoubleYap(txtNum.Text) - CIslemler.DoubleYap(txtToplam.Text);
                txtParaUstu.Text = sonuc.ToString("C2");
                txtOdenen.Text = CIslemler.DoubleYap(txtNum.Text).ToString("C2");
                txtNum.Clear();
                txtBarkod.Focus();

            }
        }
        // numaratörden barkod numarası girilerek ürün ekleme işlemi yapılması
        private void btnBarkod_Click(object sender, EventArgs e)
        {
            //if (txtNum.Text != "")
            //{

            //    if (db.Urun.Any(a => a.Barkod == txtNum.Text))
            //    {
            //        var urun = db.Urun.Where(a => a.Barkod == txtNum.Text).FirstOrDefault();
            //        UrunGetirListeye(urun, txtNum.Text, Convert.ToDouble(txtMiktar.Text));
            //        txtNum.Clear();
            //        GenelToplam();
            //        txtBarkod.Focus();
            //    }
            //    else
            //    {

            //        MessageBox.Show("Ürün Ekleme Sayfasını Aç");
            //    }
            //}

        }

        // Para üstü hesaplama fonksiyonu
        private void ParaUstuHesapla_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            double sonuc = CIslemler.DoubleYap(b.Text) - CIslemler.DoubleYap(txtToplam.Text);
            txtOdenen.Text = CIslemler.DoubleYap(b.Text).ToString("C2");
            txtParaUstu.Text = sonuc.ToString("C2");
        }

        // Barkodsuz olan ürünler için numaratörden fiyat girilerek barkodsuz ürün tuşuna basılır.
        private void btnDigerUrun_Click(object sender, EventArgs e)
        {
            if (txtNum.Text != "")
            {
                int satirsayisi = gridSatisListesi.Rows.Count;
                gridSatisListesi.Rows.Add();
                gridSatisListesi.Rows[satirsayisi].Cells["Barkod"].Value = "1111111111116";
                gridSatisListesi.Rows[satirsayisi].Cells["UrunAdi"].Value = "Barkodsuz Ürün";
                gridSatisListesi.Rows[satirsayisi].Cells["UrunGrup"].Value = "Barkodsuz Ürün";
                gridSatisListesi.Rows[satirsayisi].Cells["Birim"].Value = "Adet";
                gridSatisListesi.Rows[satirsayisi].Cells["Miktar"].Value = 1;
                gridSatisListesi.Rows[satirsayisi].Cells["AlisFiyat"].Value = 0;
                gridSatisListesi.Rows[satirsayisi].Cells["Fiyat"].Value = Convert.ToDouble(txtNum.Text);
                gridSatisListesi.Rows[satirsayisi].Cells["KdvTutari"].Value = 0;
                gridSatisListesi.Rows[satirsayisi].Cells["Toplam"].Value = Convert.ToDouble(txtNum.Text);
                txtNum.Text = "";
                GenelToplam();
                txtBarkod.Focus();

            }
        }

        private void btnIade_Click(object sender, EventArgs e)
        {
            if (chSatisİade.Checked)
            {
                chSatisİade.Checked = false;
                chSatisİade.Text = "Satış İşlemi Gerçekleştiriliyor";
            }
            else
            {
                chSatisİade.Checked = true;
                chSatisİade.Text = "İade İşlemi Gerçekleştiriliyor";
            }
        }
        //textbox checkbox gibi kutuları temizleme metodu
        private void Temizle()
        {
            txtMiktar.Text = "1";
            txtBarkod.Clear();
            txtOdenen.Clear();
            txtParaUstu.Clear();
            txtToplam.Text = 0.ToString("C2");
            txtNum.Clear();
            chSatisİade.Checked = false;
            gridSatisListesi.Rows.Clear();
            txtBarkod.Clear();
            txtBarkod.Focus();
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }
        /* Satışı yapılan ürünleri satış tablosuna ekleme işlemi  */
        public void SatisYap(string odemeyontemi)
        {
            int satirsayisi = gridSatisListesi.Rows.Count;
            bool satisiade = chSatisİade.Checked;
            double alisfiyattoplam = 0;
            if (satirsayisi > 0)
            {
                int? islemno = db.Islem.First().IslemNo;
                Satis satis = new Satis();
                for (int i = 0; i < satirsayisi; i++)
                {
                    satis.IslemNo = islemno; // Her yapılan işlem için bir işlem numarası verilir ve bu işlemler işlem tablosunda bulunur.
                    satis.UrunAd = gridSatisListesi.Rows[i].Cells["UrunAdi"].Value.ToString();
                    satis.UrunGrup = gridSatisListesi.Rows[i].Cells["UrunGrup"].Value.ToString();
                    satis.Barkod = gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString();
                    satis.Birim = gridSatisListesi.Rows[i].Cells["Birim"].Value.ToString();
                    satis.AlisFiyat = CIslemler.DoubleYap(gridSatisListesi.Rows[i].Cells["AlisFiyat"].Value.ToString());
                    satis.SatisFiyat = CIslemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Fiyat"].Value.ToString());
                    satis.Miktar = CIslemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString());
                    satis.Toplam = CIslemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Toplam"].Value.ToString());
                    satis.KdvTutari = CIslemler.DoubleYap(gridSatisListesi.Rows[i].Cells["KdvTutari"].Value.ToString()) * CIslemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString());
                    satis.OdemeSekli = odemeyontemi;
                    satis.Iade = satisiade;
                    satis.Tarih = DateTime.Now;
                    satis.Kullanici = lblKullanici.Text;
                    db.Satis.Add(satis);
                    db.SaveChanges();


                    // Eğer satışı yapılan ürün iade ise stok artımı, normal satış ise stok azaltımı yapılır
                    if (!satisiade)
                    {
                        CStokIslemleri.StokAzalt(gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString(), CIslemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString()));
                    }
                    else
                    {
                        CStokIslemleri.StokArttir(gridSatisListesi.Rows[i].Cells["Barkod"].Value.ToString(), CIslemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString()));
                    }
                    alisfiyattoplam += CIslemler.DoubleYap(gridSatisListesi.Rows[i].Cells["AlisFiyat"].Value.ToString())*CIslemler.DoubleYap(gridSatisListesi.Rows[i].Cells["Miktar"].Value.ToString());

                }
                // Satış tablosunda açıklama kısmına yapılan işlemin satış ya da işlemi olup olmadığı ile ilgili kodlar
                
                IslemOzet iozet = new IslemOzet();
                iozet.IslemNo = islemno;
                iozet.Iade = satisiade;
                iozet.AlisFiyatToplam = alisfiyattoplam;
                iozet.Gelir = false;
                iozet.Gider = false;
                if (!satisiade)
                {
                    iozet.Aciklama = odemeyontemi + "Satış";
                }
                else
                {
                    iozet.Aciklama = "İade İşlemi(" + odemeyontemi + ")";
                }
                iozet.OdemeSekli = odemeyontemi;
                iozet.Kullanici = lblKullanici.Text;
                iozet.Tarih = DateTime.Now;
                // Yapılan satışın satış tablosunda hangi ödeme yöntemiyle yapıldığı ile ilgili bilgi veren kod bloğu
                switch (odemeyontemi)
                {
                    case "Nakit":
                        iozet.Nakit = CIslemler.DoubleYap(txtToplam.Text);
                        iozet.Kart = 0; break;

                    case "Kart":
                        iozet.Nakit = 0;
                        iozet.Kart = CIslemler.DoubleYap(txtToplam.Text); break;
                    case "Kart-Nakit":
                        iozet.Nakit = CIslemler.DoubleYap(lblNakit.Text);
                        iozet.Kart = CIslemler.DoubleYap(lblkart.Text); break;
                }
                db.IslemOzet.Add(iozet);
                db.SaveChanges();
                var islemnoartir = db.Islem.First();
                islemnoartir.IslemNo += 1;
                db.SaveChanges();
                MessageBox.Show("Yazdırma İşlemi Yap");
                Temizle();
            }
        }

        private void btnNakit_Click(object sender, EventArgs e)
        {
                SatisYap("Nakit");
        }

        private void btnKart_Click(object sender, EventArgs e)
        {
            SatisYap("Kart");
        }

        private void btnNakitKart_Click(object sender, EventArgs e)
        {
            FNakitKart f = new FNakitKart();
            f.ShowDialog();
        }

        private void txtBarkod_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nakit kart bölümünde sadece rakam girilmesi için kodlar. Harf girişine izin verilmiyor.
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08)
            {
                e.Handled = true;
            }
        }

        private void txtMiktar_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMiktar_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nakit kart bölümünde sadece rakam girilmesi için kodlar. Harf girişine izin verilmiyor.
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08)
            {
                e.Handled = true;
            }
        }

        private void txtNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nakit kart bölümünde sadece rakam girilmesi için kodlar. Harf girişine izin verilmiyor.
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != (char)08)
            {
                e.Handled = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        { 
            // Kısa yol tuşları
            if (e.KeyCode==Keys.F1)
            {
                SatisYap("Nakit");
            }

            if (e.KeyCode == Keys.F2)
            {
                SatisYap("Kart");
            }

            if (e.KeyCode == Keys.F3)
            {
                FNakitKart f = new FNakitKart();
                f.ShowDialog();
            }

          
        }

        // Data gridview içindeki bilgiler gizli olarak bulunan diğer datagridview içine aktarılır.
        private void IslemBeklet()
        {
            int satir = gridSatisListesi.RowCount;
            int sutun = gridSatisListesi.Columns.Count;
            if (satir > 0)
            {
                for (int i = 0; i < satir; i++)
                {
                    gridIslemBeklet.Rows.Add();
                    for (int j = 0; j < sutun - 1; j++) // sil sütunun olmaması için -1 dedik
                    {
                        gridIslemBeklet.Rows[i].Cells[j].Value=gridSatisListesi.Rows[i].Cells[j].Value;
                    }
                }
            }
        }
        // Gizli datagridview içindeki bilgiler anasayfada bulunan datagridview içine aktarılır
        private void IslemBeklemedenCik()
        {
            int satir = gridIslemBeklet.RowCount;
            int sutun = gridIslemBeklet.Columns.Count;
            if (satir > 0)
            {
                for (int i = 0; i < satir; i++)
                {
                    gridSatisListesi.Rows.Add();
                    for (int j = 0; j < sutun - 1; j++) // sil sütunun olmaması için -1 dedik
                    {
                        gridSatisListesi.Rows[i].Cells[j].Value = gridIslemBeklet.Rows[i].Cells[j].Value;
                    }
                }
            }
        }

        /*İşlem bekletme tuşu: Müşteri almak istediği ürünleri okumtmuştur ama henüz satış gerçekleşmemiştir. 
         * Daha sonra aklına gelen bir ürün olduğu zaman işlem bekletme butonuna basılarak 
         * okutmuş olduğu ürünler gizli bir datagridview içerisinde bekletilir. Müşteri aklına gelen ürünü 
         * almak için tekrar market içine yöneldiğinde hemen ardından gelen müşterilerin satış işlemleri gerçekleştirilir.
         Daha sonra istediği ürünleri alan müşteri geldiğinde tekrar işlem beklet butonuna basılarak daha önce okutmuş olduğu ürünler listede gözükür.*/
        private void btnİslemBeklet_Click(object sender, EventArgs e)
        {
            if (btnIslemBeklet.Text=="İşlem Beklet")
            {
                IslemBeklet();
                btnIslemBeklet.BackColor = System.Drawing.Color.OrangeRed;
                btnIslemBeklet.Text = "İşlem Bekliyor";
                gridSatisListesi.Rows.Clear();
            }
            else
            {
                IslemBeklemedenCik();
                btnIslemBeklet.BackColor = System.Drawing.Color.LightSteelBlue;
                btnIslemBeklet.Text = "İşlem Beklet";
                gridIslemBeklet.Rows.Clear();
            }
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
