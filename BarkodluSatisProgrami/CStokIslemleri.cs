using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarkodluSatisProgrami
{
    static class CStokIslemleri
    {
        // Stok Azaltma işlemleri
        public static void StokAzalt(string barkod, double miktar)
        {
            if (barkod != "1111111111116")
            {
                using (var db = new SatisDBEntities())
                {
                    var urunbilgi = db.Urun.SingleOrDefault(x => x.Barkod == barkod);
                    urunbilgi.Miktar -= miktar; //varolan miktardan istenen miktar çıkarılıyor
                    db.SaveChanges();
                }
            }
           
        }

        //Stok arttırma işlemleri
        public static void StokArttir(string barkod, double miktar)
        {
            if (barkod != "1111111111116")
            {
                using (var db = new SatisDBEntities())
                {
                    var urunbilgi = db.Urun.SingleOrDefault(x => x.Barkod == barkod);
                    urunbilgi.Miktar += miktar; //varolan miktarı arttırma işlemi
                    db.SaveChanges();
                }
            }
           
        }


        public static void StokHareketleri(string barkod,string urunad, string birim,double miktar,string urungrup,string kullanici)
        {
            using(var db = new SatisDBEntities())
            {
                StokHareket s = new StokHareket();
                s.Barkod = barkod;
                s.UrunAd = urunad;
                s.Birim = birim;
                s.Miktar = miktar;
                s.UrunGrup = urungrup;
                s.Kullanici = kullanici;
                s.Tarih = DateTime.Now;
                db.StokHareket.Add(s);
                db.SaveChanges();
            }
        }

    }
}
