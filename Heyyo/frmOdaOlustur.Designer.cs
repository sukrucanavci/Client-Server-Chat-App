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
            this.label2 = new System.Windows.Forms.Label();
            this.btnVazgec = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Oda Adı: ";
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
            this.txtOdaSifresi.Location = new System.Drawing.Point(97, 41);
            this.txtOdaSifresi.Name = "txtOdaSifresi";
            this.txtOdaSifresi.Size = new System.Drawing.Size(233, 22);
            this.txtOdaSifresi.TabIndex = 3;
            // 
            // btnOdaOlustur
            // 
            this.btnOdaOlustur.Location = new System.Drawing.Point(97, 69);
            this.btnOdaOlustur.Name = "btnOdaOlustur";
            this.btnOdaOlustur.Size = new System.Drawing.Size(109, 35);
            this.btnOdaOlustur.TabIndex = 4;
            this.btnOdaOlustur.Text = "Oluştur";
            this.btnOdaOlustur.UseVisualStyleBackColor = true;
            this.btnOdaOlustur.Click += new System.EventHandler(this.btnOdaOlustur_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Şifre: ";
            // 
            // btnVazgec
            // 
            this.btnVazgec.Location = new System.Drawing.Point(221, 69);
            this.btnVazgec.Name = "btnVazgec";
            this.btnVazgec.Size = new System.Drawing.Size(109, 35);
            this.btnVazgec.TabIndex = 6;
            this.btnVazgec.Text = "Vazgeç";
            this.btnVazgec.UseVisualStyleBackColor = true;
            this.btnVazgec.Click += new System.EventHandler(this.btnVazgec_Click);
            // 
            // frmOdaOlustur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(352, 120);
            this.Controls.Add(this.btnVazgec);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOdaOlustur);
            this.Controls.Add(this.txtOdaSifresi);
            this.Controls.Add(this.txtOdaAdi);
            this.Controls.Add(this.label1);
            this.Name = "frmOdaOlustur";
            this.Text = "Oda Oluştur";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOdaAdi;
        private System.Windows.Forms.TextBox txtOdaSifresi;
        private System.Windows.Forms.Button btnOdaOlustur;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnVazgec;
    }
}