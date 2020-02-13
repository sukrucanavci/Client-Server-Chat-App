namespace Heyyo
{
    partial class frmAna
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Ses Kanalları");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAna));
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Yöneticiler");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Kullanıcılar");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.bağlantıToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBaglan = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBaglantiyiKes = new System.Windows.Forms.ToolStripMenuItem();
            this.ayarlarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tümAyarlarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fontAyarlarıToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRenkAyarları = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiKullaniciAdiRengi = new System.Windows.Forms.ToolStripMenuItem();
            this.yardımToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.güncellemeleriDenetleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hakkındaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ssrBilgi = new System.Windows.Forms.StatusStrip();
            this.tsslSunucu = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslKullaniciAdi = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslPing = new System.Windows.Forms.ToolStripStatusLabel();
            this.tvwOdalar = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.tsbKullaniciListesi = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbKanalOlustur = new System.Windows.Forms.ToolStripButton();
            this.cmsKullanicilar = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mesajGönderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imgSayilar = new System.Windows.Forms.ImageList(this.components);
            this.imgOdalar = new System.Windows.Forms.ImageList(this.components);
            this.tvwKullanicilar = new System.Windows.Forms.TreeView();
            this.rtxMesajim = new System.Windows.Forms.RichTextBox();
            this.tabAna = new System.Windows.Forms.TabControl();
            this.tpG = new System.Windows.Forms.TabPage();
            this.rtxG = new System.Windows.Forms.RichTextBox();
            this.menuStrip1.SuspendLayout();
            this.ssrBilgi.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.cmsKullanicilar.SuspendLayout();
            this.tabAna.SuspendLayout();
            this.tpG.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bağlantıToolStripMenuItem,
            this.ayarlarToolStripMenuItem,
            this.yardımToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.menuStrip1.Size = new System.Drawing.Size(1262, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // bağlantıToolStripMenuItem
            // 
            this.bağlantıToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiBaglan,
            this.tsmiBaglantiyiKes});
            this.bağlantıToolStripMenuItem.Name = "bağlantıToolStripMenuItem";
            this.bağlantıToolStripMenuItem.Size = new System.Drawing.Size(78, 24);
            this.bağlantıToolStripMenuItem.Text = "Bağlantı";
            // 
            // tsmiBaglan
            // 
            this.tsmiBaglan.Name = "tsmiBaglan";
            this.tsmiBaglan.Size = new System.Drawing.Size(185, 26);
            this.tsmiBaglan.Text = "Bağlan";
            this.tsmiBaglan.Click += new System.EventHandler(this.tsmiBaglan_Click);
            // 
            // tsmiBaglantiyiKes
            // 
            this.tsmiBaglantiyiKes.Name = "tsmiBaglantiyiKes";
            this.tsmiBaglantiyiKes.Size = new System.Drawing.Size(185, 26);
            this.tsmiBaglantiyiKes.Text = "Bağlantıyı Kes";
            this.tsmiBaglantiyiKes.Click += new System.EventHandler(this.tsmiBaglantiyiKes_Click);
            // 
            // ayarlarToolStripMenuItem
            // 
            this.ayarlarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tümAyarlarToolStripMenuItem,
            this.fontAyarlarıToolStripMenuItem,
            this.tsmiRenkAyarları});
            this.ayarlarToolStripMenuItem.Name = "ayarlarToolStripMenuItem";
            this.ayarlarToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.ayarlarToolStripMenuItem.Text = "Ayarlar";
            // 
            // tümAyarlarToolStripMenuItem
            // 
            this.tümAyarlarToolStripMenuItem.Name = "tümAyarlarToolStripMenuItem";
            this.tümAyarlarToolStripMenuItem.Size = new System.Drawing.Size(179, 26);
            this.tümAyarlarToolStripMenuItem.Text = "Ses Ayarları";
            // 
            // fontAyarlarıToolStripMenuItem
            // 
            this.fontAyarlarıToolStripMenuItem.Name = "fontAyarlarıToolStripMenuItem";
            this.fontAyarlarıToolStripMenuItem.Size = new System.Drawing.Size(179, 26);
            this.fontAyarlarıToolStripMenuItem.Text = "Font Ayarları";
            // 
            // tsmiRenkAyarları
            // 
            this.tsmiRenkAyarları.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiKullaniciAdiRengi});
            this.tsmiRenkAyarları.Name = "tsmiRenkAyarları";
            this.tsmiRenkAyarları.Size = new System.Drawing.Size(179, 26);
            this.tsmiRenkAyarları.Text = "Renk Ayarları";
            // 
            // tsmiKullaniciAdiRengi
            // 
            this.tsmiKullaniciAdiRengi.Name = "tsmiKullaniciAdiRengi";
            this.tsmiKullaniciAdiRengi.Size = new System.Drawing.Size(217, 26);
            this.tsmiKullaniciAdiRengi.Text = "Kullanıcı Adı Rengi";
            // 
            // yardımToolStripMenuItem
            // 
            this.yardımToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.güncellemeleriDenetleToolStripMenuItem,
            this.hakkındaToolStripMenuItem});
            this.yardımToolStripMenuItem.Name = "yardımToolStripMenuItem";
            this.yardımToolStripMenuItem.Size = new System.Drawing.Size(69, 24);
            this.yardımToolStripMenuItem.Text = "Yardım";
            // 
            // güncellemeleriDenetleToolStripMenuItem
            // 
            this.güncellemeleriDenetleToolStripMenuItem.Name = "güncellemeleriDenetleToolStripMenuItem";
            this.güncellemeleriDenetleToolStripMenuItem.Size = new System.Drawing.Size(247, 26);
            this.güncellemeleriDenetleToolStripMenuItem.Text = "Güncellemeleri Denetle";
            // 
            // hakkındaToolStripMenuItem
            // 
            this.hakkındaToolStripMenuItem.Name = "hakkındaToolStripMenuItem";
            this.hakkındaToolStripMenuItem.Size = new System.Drawing.Size(247, 26);
            this.hakkındaToolStripMenuItem.Text = "Hakkında";
            // 
            // ssrBilgi
            // 
            this.ssrBilgi.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ssrBilgi.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslSunucu,
            this.tsslKullaniciAdi,
            this.tsslPing});
            this.ssrBilgi.Location = new System.Drawing.Point(0, 647);
            this.ssrBilgi.Name = "ssrBilgi";
            this.ssrBilgi.Size = new System.Drawing.Size(1262, 26);
            this.ssrBilgi.TabIndex = 1;
            this.ssrBilgi.Text = "statusStrip1";
            // 
            // tsslSunucu
            // 
            this.tsslSunucu.Name = "tsslSunucu";
            this.tsslSunucu.Size = new System.Drawing.Size(50, 20);
            this.tsslSunucu.Text = "0.0.0.0";
            // 
            // tsslKullaniciAdi
            // 
            this.tsslKullaniciAdi.Name = "tsslKullaniciAdi";
            this.tsslKullaniciAdi.Size = new System.Drawing.Size(179, 20);
            this.tsslKullaniciAdi.Text = "(İsimsiz) olarak bağlanıldı";
            // 
            // tsslPing
            // 
            this.tsslPing.Name = "tsslPing";
            this.tsslPing.Size = new System.Drawing.Size(73, 20);
            this.tsslPing.Text = "Ping 0 ms";
            // 
            // tvwOdalar
            // 
            this.tvwOdalar.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvwOdalar.FullRowSelect = true;
            this.tvwOdalar.ItemHeight = 30;
            this.tvwOdalar.Location = new System.Drawing.Point(0, 55);
            this.tvwOdalar.Name = "tvwOdalar";
            treeNode1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            treeNode1.Name = "sesKanallari";
            treeNode1.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            treeNode1.Text = "Ses Kanalları";
            this.tvwOdalar.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tvwOdalar.ShowLines = false;
            this.tvwOdalar.ShowPlusMinus = false;
            this.tvwOdalar.Size = new System.Drawing.Size(252, 592);
            this.tvwOdalar.TabIndex = 1111;
            this.tvwOdalar.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvwOdalar_BeforeCollapse);
            this.tvwOdalar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tvwOdalar_MouseDoubleClick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.tsbKullaniciListesi,
            this.toolStripSeparator1,
            this.tsbKanalOlustur});
            this.toolStrip1.Location = new System.Drawing.Point(0, 28);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(1262, 27);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::Heyyo.Properties.Resources.iconfinder_volume_up_1608454;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(29, 24);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "Hoparlör";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::Heyyo.Properties.Resources.iconfinder_microphone_1608550;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(29, 24);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.ToolTipText = "Mikrofon";
            // 
            // tsbKullaniciListesi
            // 
            this.tsbKullaniciListesi.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbKullaniciListesi.Image = ((System.Drawing.Image)(resources.GetObject("tsbKullaniciListesi.Image")));
            this.tsbKullaniciListesi.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbKullaniciListesi.Name = "tsbKullaniciListesi";
            this.tsbKullaniciListesi.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tsbKullaniciListesi.Size = new System.Drawing.Size(133, 24);
            this.tsbKullaniciListesi.Text = "Kullanıcı Listesi";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // tsbKanalOlustur
            // 
            this.tsbKanalOlustur.Image = ((System.Drawing.Image)(resources.GetObject("tsbKanalOlustur.Image")));
            this.tsbKanalOlustur.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbKanalOlustur.Name = "tsbKanalOlustur";
            this.tsbKanalOlustur.Size = new System.Drawing.Size(121, 24);
            this.tsbKanalOlustur.Text = "Kanal Oluştur";
            this.tsbKanalOlustur.Click += new System.EventHandler(this.tsbKanalOlustur_Click);
            // 
            // cmsKullanicilar
            // 
            this.cmsKullanicilar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsKullanicilar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mesajGönderToolStripMenuItem});
            this.cmsKullanicilar.Name = "cmsKullanicilar";
            this.cmsKullanicilar.Size = new System.Drawing.Size(171, 28);
            // 
            // mesajGönderToolStripMenuItem
            // 
            this.mesajGönderToolStripMenuItem.Name = "mesajGönderToolStripMenuItem";
            this.mesajGönderToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.mesajGönderToolStripMenuItem.Text = "Mesaj Gönder";
            // 
            // imgSayilar
            // 
            this.imgSayilar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgSayilar.ImageStream")));
            this.imgSayilar.TransparentColor = System.Drawing.Color.Transparent;
            this.imgSayilar.Images.SetKeyName(0, "1.png");
            // 
            // imgOdalar
            // 
            this.imgOdalar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgOdalar.ImageStream")));
            this.imgOdalar.TransparentColor = System.Drawing.Color.Transparent;
            this.imgOdalar.Images.SetKeyName(0, "varsayilan.png");
            this.imgOdalar.Images.SetKeyName(1, "oda24.png");
            this.imgOdalar.Images.SetKeyName(2, "kullanici.png");
            this.imgOdalar.Images.SetKeyName(3, "kilit.png");
            // 
            // tvwKullanicilar
            // 
            this.tvwKullanicilar.ContextMenuStrip = this.cmsKullanicilar;
            this.tvwKullanicilar.Dock = System.Windows.Forms.DockStyle.Right;
            this.tvwKullanicilar.FullRowSelect = true;
            this.tvwKullanicilar.ItemHeight = 30;
            this.tvwKullanicilar.Location = new System.Drawing.Point(1011, 55);
            this.tvwKullanicilar.Name = "tvwKullanicilar";
            treeNode2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            treeNode2.ForeColor = System.Drawing.Color.Black;
            treeNode2.Name = "yoneticiler";
            treeNode2.Text = "Yöneticiler";
            treeNode3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            treeNode3.ForeColor = System.Drawing.Color.Black;
            treeNode3.Name = "kullanicilar";
            treeNode3.Text = "Kullanıcılar";
            this.tvwKullanicilar.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3});
            this.tvwKullanicilar.ShowLines = false;
            this.tvwKullanicilar.ShowPlusMinus = false;
            this.tvwKullanicilar.Size = new System.Drawing.Size(251, 592);
            this.tvwKullanicilar.TabIndex = 91;
            this.tvwKullanicilar.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvwKullanicilar_BeforeCollapse);
            this.tvwKullanicilar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tvwKullanicilar_MouseDoubleClick);
            // 
            // rtxMesajim
            // 
            this.rtxMesajim.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtxMesajim.Location = new System.Drawing.Point(252, 572);
            this.rtxMesajim.Name = "rtxMesajim";
            this.rtxMesajim.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtxMesajim.Size = new System.Drawing.Size(759, 75);
            this.rtxMesajim.TabIndex = 89;
            this.rtxMesajim.Text = "";
            this.rtxMesajim.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.rtxMesajim_KeyPress);
            // 
            // tabAna
            // 
            this.tabAna.Controls.Add(this.tpG);
            this.tabAna.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAna.Location = new System.Drawing.Point(252, 55);
            this.tabAna.Name = "tabAna";
            this.tabAna.SelectedIndex = 0;
            this.tabAna.Size = new System.Drawing.Size(759, 517);
            this.tabAna.TabIndex = 90;
            // 
            // tpG
            // 
            this.tpG.Controls.Add(this.rtxG);
            this.tpG.Location = new System.Drawing.Point(4, 25);
            this.tpG.Name = "tpG";
            this.tpG.Padding = new System.Windows.Forms.Padding(3);
            this.tpG.Size = new System.Drawing.Size(751, 488);
            this.tpG.TabIndex = 0;
            this.tpG.Text = "Genel Sohbet";
            // 
            // rtxG
            // 
            this.rtxG.BackColor = System.Drawing.Color.White;
            this.rtxG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxG.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.rtxG.Location = new System.Drawing.Point(3, 3);
            this.rtxG.Name = "rtxG";
            this.rtxG.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rtxG.Size = new System.Drawing.Size(745, 482);
            this.rtxG.TabIndex = 4;
            this.rtxG.Text = "";
            // 
            // frmAna
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.tabAna);
            this.Controls.Add(this.rtxMesajim);
            this.Controls.Add(this.tvwKullanicilar);
            this.Controls.Add(this.tvwOdalar);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.ssrBilgi);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(640, 360);
            this.Name = "frmAna";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Heyyo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAna_FormClosing);
            this.Load += new System.EventHandler(this.frmAna_Load);
            this.Shown += new System.EventHandler(this.frmAna_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ssrBilgi.ResumeLayout(false);
            this.ssrBilgi.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cmsKullanicilar.ResumeLayout(false);
            this.tabAna.ResumeLayout(false);
            this.tpG.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem bağlantıToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiBaglan;
        private System.Windows.Forms.ToolStripMenuItem tsmiBaglantiyiKes;
        private System.Windows.Forms.ToolStripMenuItem ayarlarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yardımToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hakkındaToolStripMenuItem;
        private System.Windows.Forms.StatusStrip ssrBilgi;
        private System.Windows.Forms.ToolStripMenuItem tümAyarlarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem güncellemeleriDenetleToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel tsslPing;
        private System.Windows.Forms.ToolStripStatusLabel tsslKullaniciAdi;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.TreeView tvwOdalar;
        private System.Windows.Forms.ToolStripButton tsbKullaniciListesi;
        private System.Windows.Forms.ContextMenuStrip cmsKullanicilar;
        private System.Windows.Forms.ToolStripMenuItem mesajGönderToolStripMenuItem;
        private System.Windows.Forms.ImageList imgSayilar;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbKanalOlustur;
        private System.Windows.Forms.ImageList imgOdalar;
        private System.Windows.Forms.ToolStripStatusLabel tsslSunucu;
        private System.Windows.Forms.ToolStripMenuItem fontAyarlarıToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiRenkAyarları;
        private System.Windows.Forms.TreeView tvwKullanicilar;
        private System.Windows.Forms.RichTextBox rtxMesajim;
        private System.Windows.Forms.TabControl tabAna;
        private System.Windows.Forms.TabPage tpG;
        private System.Windows.Forms.RichTextBox rtxG;
        private System.Windows.Forms.ToolStripMenuItem tsmiKullaniciAdiRengi;
    }
}