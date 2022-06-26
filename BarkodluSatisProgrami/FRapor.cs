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
    public partial class FRapor : Form
    {
        public FRapor()
        {
            InitializeComponent();
        }

        private void FRapor_Load(object sender, EventArgs e)
        {

        }

        private void btnGoster_Click(object sender, EventArgs e)
        {
            Cursor.Current=Cursors.WaitCursor;
            DateTime baslangic = DateTime.Parse(dateBaslangic.Value.ToShortDateString());     
            DateTime bitis = DateTime.Parse(dateBitis.Value.ToShortDateString());
            bitis = bitis.AddDays(1);
            using (var db =new SatisDBEntities())
            {
                if (listFiltrelemeTuru.SelectedIndex==0)
                {
                    db.IslemOzet.Where(x => x.Tarih >= baslangic && x.Tarih <= bitis).OrderByDescending(x => x.Tarih).Load();
                    var islemozet = db.IslemOzet.Local.ToBindingList();
                    gridListe.DataSource = islemozet;
                }
            }


            Cursor.Current= Cursors.Default;
        }
    }
}
