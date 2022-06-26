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
    public partial class FNakitKart : Form
    {
        public FNakitKart()
        {
            InitializeComponent();
        }

        private void txtNakit_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtNakit.Text != "")
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Hesapla();

                }
            }
        }

        private void Hesapla()
        {
            FUrunSatis f = (FUrunSatis)Application.OpenForms["FUrunSatis"];
            double nakit = CIslemler.DoubleYap(txtNakit.Text);
            double toplam = CIslemler.DoubleYap(f.txtToplam.Text);
            double kart = toplam - nakit;
            f.lblNakit.Text = nakit.ToString("C2");
            f.lblkart.Text = kart.ToString("C2");
            f.SatisYap("Kart-Nakit");
            this.Hide();
        }
        private void Numarator_Click(object sender, EventArgs e)
        {
            // iki kez virgül tuşuna basılmaması için
            Button b = (Button)sender;
            if (b.Text == ",")
            {
                int virgul = txtNakit.Text.Count(x => x == ',');
                if (virgul < 1)
                {
                    txtNakit.Text += b.Text;
                }
            }
            // Yazılan en son rakamı silmek için 
            else if (b.Text == "←")
            {
                if (txtNakit.Text.Length > 0)
                {
                    txtNakit.Text = txtNakit.Text.Substring(0, txtNakit.Text.Length - 1);
                }
            }
            else
            {
                txtNakit.Text += b.Text;
            }
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            if (txtNakit.Text != "")
            {
                    Hesapla();   
            }
        }

        private void txtNakit_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Nakit kart bölümünde sadece rakam girilmesi için kodlar. Harf girişine izin verilmiyor.
            if (char.IsDigit(e.KeyChar)==false && e.KeyChar!=(char)08)
            {
                e.Handled = true;
            }
        }

        private void NakitKart_Load(object sender, EventArgs e)
        {

        }
    }
}
