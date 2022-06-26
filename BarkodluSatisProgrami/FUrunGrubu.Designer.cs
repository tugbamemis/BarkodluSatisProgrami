namespace BarkodluSatisProgrami
{
    partial class FUrunGrubu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listUrunGrup = new System.Windows.Forms.ListBox();
            this.btnEkle = new BarkodluSatisProgrami.btn();
            this.txtUrunGrupAd = new BarkodluSatisProgrami.txt();
            this.lbl1 = new BarkodluSatisProgrami.lbl();
            this.btnSil = new BarkodluSatisProgrami.btn();
            this.SuspendLayout();
            // 
            // listUrunGrup
            // 
            this.listUrunGrup.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.listUrunGrup.FormattingEnabled = true;
            this.listUrunGrup.ItemHeight = 21;
            this.listUrunGrup.Location = new System.Drawing.Point(25, 78);
            this.listUrunGrup.Name = "listUrunGrup";
            this.listUrunGrup.Size = new System.Drawing.Size(262, 193);
            this.listUrunGrup.TabIndex = 2;
            // 
            // btnEkle
            // 
            this.btnEkle.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnEkle.FlatAppearance.BorderColor = System.Drawing.Color.DarkSeaGreen;
            this.btnEkle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEkle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnEkle.ForeColor = System.Drawing.Color.White;
            this.btnEkle.Image = global::BarkodluSatisProgrami.Properties.Resources.Ekle20;
            this.btnEkle.Location = new System.Drawing.Point(157, 275);
            this.btnEkle.Margin = new System.Windows.Forms.Padding(1);
            this.btnEkle.Name = "btnEkle";
            this.btnEkle.Size = new System.Drawing.Size(130, 69);
            this.btnEkle.TabIndex = 0;
            this.btnEkle.Text = "Ekle";
            this.btnEkle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnEkle.UseVisualStyleBackColor = false;
            this.btnEkle.Click += new System.EventHandler(this.btnEkle_Click);
            // 
            // txtUrunGrupAd
            // 
            this.txtUrunGrupAd.BackColor = System.Drawing.Color.White;
            this.txtUrunGrupAd.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtUrunGrupAd.Location = new System.Drawing.Point(25, 43);
            this.txtUrunGrupAd.Name = "txtUrunGrupAd";
            this.txtUrunGrupAd.Size = new System.Drawing.Size(262, 29);
            this.txtUrunGrupAd.TabIndex = 1;
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lbl1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lbl1.Location = new System.Drawing.Point(21, 19);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(123, 21);
            this.lbl1.TabIndex = 0;
            this.lbl1.Text = "Ürün Grubu Adı";
            // 
            // btnSil
            // 
            this.btnSil.BackColor = System.Drawing.Color.Salmon;
            this.btnSil.FlatAppearance.BorderColor = System.Drawing.Color.Salmon;
            this.btnSil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSil.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSil.ForeColor = System.Drawing.Color.White;
            this.btnSil.Image = global::BarkodluSatisProgrami.Properties.Resources.cancel24;
            this.btnSil.Location = new System.Drawing.Point(25, 275);
            this.btnSil.Margin = new System.Windows.Forms.Padding(1);
            this.btnSil.Name = "btnSil";
            this.btnSil.Size = new System.Drawing.Size(130, 69);
            this.btnSil.TabIndex = 0;
            this.btnSil.Text = "Sil";
            this.btnSil.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSil.UseVisualStyleBackColor = false;
            this.btnSil.Click += new System.EventHandler(this.btnSil_Click);
            // 
            // FUrunGrubu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.ClientSize = new System.Drawing.Size(308, 366);
            this.Controls.Add(this.btnSil);
            this.Controls.Add(this.btnEkle);
            this.Controls.Add(this.listUrunGrup);
            this.Controls.Add(this.txtUrunGrupAd);
            this.Controls.Add(this.lbl1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Name = "FUrunGrubu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ürün Grubu İşlemleri";
            this.Load += new System.EventHandler(this.FUrunGrubu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private lbl lbl1;
        private txt txtUrunGrupAd;
        private System.Windows.Forms.ListBox listUrunGrup;
        private btn btnEkle;
        private btn btnSil;
    }
}