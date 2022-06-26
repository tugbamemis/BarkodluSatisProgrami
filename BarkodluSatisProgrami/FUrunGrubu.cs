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
    public partial class FUrunGrubu : Form
    {
        public FUrunGrubu()
        {
            InitializeComponent();
        }
        SatisDBEntities db = new SatisDBEntities();

        //Form açıldığında listbox içine veriler otomatik olarak yüklenir
        private void FUrunGrubu_Load(object sender, EventArgs e)
        {
            UGrup();
        }

        //Ürün grubu ekleme işlemleri
        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (txtUrunGrupAd.Text!="")
            {
                UrunGrubuTbl u = new UrunGrubuTbl();
                u.UrunGrupAd = txtUrunGrupAd.Text;
                db.UrunGrubuTbl.Add(u);
                db.SaveChanges();
                UGrup();
                txtUrunGrupAd.Clear();
                MessageBox.Show("Ürün grubu eklenmiştir");
                FUrunGirisi f = (FUrunGirisi)Application.OpenForms["FUrunGirisi"];
                if (f!=null)
                {
                    f.UGrup();
                }
                
            }
            else
            {
                MessageBox.Show("Lütfen grup bilgisi ekleyiniz...");
            }
        }

        //listbox içine veriler otomatik olarak yüklenir
        private void UGrup()
        {
            listUrunGrup.DisplayMember = "UrunGrupAd";
            listUrunGrup.ValueMember = "Id";
            listUrunGrup.DataSource = db.UrunGrubuTbl.OrderBy(a => a.UrunGrupAd).ToList();
        }

        //Ürün grubu silme işlemi
        private void btnSil_Click(object sender, EventArgs e)
        {
            int grupid = Convert.ToInt32(listUrunGrup.SelectedValue.ToString());
            string grupad = listUrunGrup.Text;
            DialogResult onay = MessageBox.Show(grupad + " grubunu silmek istiyor musunuz?","Silme İşlemi",MessageBoxButtons.YesNo);
            if (onay == DialogResult.Yes)
            {
                var grup = db.UrunGrubuTbl.FirstOrDefault(x => x.Id == grupid);
                db.UrunGrubuTbl.Remove(grup);
                db.SaveChanges();
                UGrup();
                txtUrunGrupAd.Focus();
                MessageBox.Show(grupad + " ürün grubu silindi");
                FUrunGirisi f = (FUrunGirisi)Application.OpenForms["FUrunGirisi"];
                f.UGrup();
                
            }
            
        }
    }
}
