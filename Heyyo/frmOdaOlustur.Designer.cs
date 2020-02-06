namespace Heyyo
{
    partial class frmOdaOlustur
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtOdaAdi = new System.Windows.Forms.TextBox();
            this.txtOdaSifresi = new System.Windows.Forms.TextBox();
            this.btnOdaOlustur = new System.Windows.Forms.Button();
            this.chkSifre = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Oda Adı";
            // 
            // txtOdaAdi
            // 
            this.txtOdaAdi.Location = new System.Drawing.Point(97, 12);
            this.txtOdaAdi.Name = "txtOdaAdi";
            this.txtOdaAdi.Size = new System.Drawing.Size(233, 22);
            this.txtOdaAdi.TabIndex = 2;
            // 
            // txtOdaSifresi
            // 
            this.txtOdaSifresi.Enabled = false;
            this.txtOdaSifresi.Location = new System.Drawing.Point(97, 41);
            this.txtOdaSifresi.Name = "txtOdaSifresi";
            this.txtOdaSifresi.Size = new System.Drawing.Size(233, 22);
            this.txtOdaSifresi.TabIndex = 3;
            // 
            // btnOdaOlustur
            // 
            this.btnOdaOlustur.Location = new System.Drawing.Point(208, 69);
            this.btnOdaOlustur.Name = "btnOdaOlustur";
            this.btnOdaOlustur.Size = new System.Drawing.Size(122, 35);
            this.btnOdaOlustur.TabIndex = 4;
            this.btnOdaOlustur.Text = "Oluştur";
            this.btnOdaOlustur.UseVisualStyleBackColor = true;
            this.btnOdaOlustur.Click += new System.EventHandler(this.btnOdaOlustur_Click);
            // 
            // chkSifre
            // 
            this.chkSifre.AutoSize = true;
            this.chkSifre.Location = new System.Drawing.Point(15, 43);
            this.chkSifre.Name = "chkSifre";
            this.chkSifre.Size = new System.Drawing.Size(59, 21);
            this.chkSifre.TabIndex = 5;
            this.chkSifre.Text = "Şifre";
            this.chkSifre.UseVisualStyleBackColor = true;
            this.chkSifre.CheckedChanged += new System.EventHandler(this.chkSifre_CheckedChanged);
            // 
            // frmOdaOlustur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 120);
            this.Controls.Add(this.chkSifre);
            this.Controls.Add(this.btnOdaOlustur);
            this.Controls.Add(this.txtOdaSifresi);
            this.Controls.Add(this.txtOdaAdi);
            this.Controls.Add(this.label1);
            this.MinimizeBox = false;
            this.Name = "frmOdaOlustur";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Oda Oluştur";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOdaAdi;
        private System.Windows.Forms.TextBox txtOdaSifresi;
        private System.Windows.Forms.Button btnOdaOlustur;
        private System.Windows.Forms.CheckBox chkSifre;
    }
}