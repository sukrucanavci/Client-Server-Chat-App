using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Drawing;

namespace Heyyo
{
    public partial class frmAna : Form
    {
        public frmAna()
        {
            InitializeComponent();
        }

        TabPage tp;
        RichTextBox rtx;
        public Socket soket;
        public string ip;
        public int port;
        public string kullaniciAdi;
        frmGiris fGiris = new frmGiris();
        Thread tMesajAlma;

        #region Enums

        enum komutlar
        {
            kullaniciadi,
            genelmesaj,
            ozelmesaj,
            odalistesi,
            odaekle,
            odasil,
            odayakullanicigirdi,
            odadankullanicicikti,
            kullanicilistesi,
            kullanicigirdi,
            kullanicicikti,
            kullaniciodayagirdi,
            kullaniciodadancikti,
            kullanicirenginidegistirdi,
            sunucumesaji,
            girisbasarli,
            girisbasarisiz
        }

        enum cevap
        {
            evet,
            hayır
        }

        #endregion

        private void frmAna_Load(object sender, EventArgs e)
        {
            #region Kullanıcı Adı Renkleri
            List<ToolStripMenuItem> renkler = new List<ToolStripMenuItem>();
            renkler.Add(new ToolStripMenuItem()); renkler[0].BackColor = Color.Blue;
            renkler.Add(new ToolStripMenuItem()); renkler[1].BackColor = Color.Red;
            renkler.Add(new ToolStripMenuItem()); renkler[2].BackColor = Color.Green;
            renkler.Add(new ToolStripMenuItem()); renkler[3].BackColor = Color.GreenYellow;
            renkler.Add(new ToolStripMenuItem()); renkler[4].BackColor = Color.YellowGreen;
            renkler.Add(new ToolStripMenuItem()); renkler[5].BackColor = Color.Cyan;
            renkler.Add(new ToolStripMenuItem()); renkler[6].BackColor = Color.DarkCyan;
            renkler.Add(new ToolStripMenuItem()); renkler[7].BackColor = Color.Pink;
            renkler.Add(new ToolStripMenuItem()); renkler[8].BackColor = Color.Purple;
            renkler.Add(new ToolStripMenuItem()); renkler[9].BackColor = Color.Orange;
            renkler.Add(new ToolStripMenuItem()); renkler[10].BackColor = Color.Brown;

            tsmiKullaniciAdiRengi.DropDownItems.AddRange(renkler.ToArray());
            #endregion

            tvwOdalar.ImageList = imgOdalar;
            tabAna.ImageList = imgSayilar;
        }

        private void frmAna_Shown(object sender, EventArgs e)
        {
            fGiris.ShowDialog();
        }

        public void Baglan()
        {
            if (soket.Connected) { BaglantiyiKes(); }
            soket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            soket.Connect(fGiris.lep);
            kullaniciAdi = fGiris.kullaniciAdi;

            List<byte> byteList = Encoding.UTF8.GetBytes(kullaniciAdi).ToList();
            byteList.Insert(0, (byte)((char)komutlar.kullaniciadi));
            MesajYolla(byteList.ToArray());

            tMesajAlma = new Thread(MesajAlma);
            tMesajAlma.Start();
        }

        public void BaglantiyiKes()
        {
            if (soket.Connected)
            {
                rtxG.AppendText(soket.RemoteEndPoint + " bağlantısı kapatıldı.");
                soket.Close();
                tMesajAlma.Abort();
                tvwOdalar.Nodes[0].Nodes.Clear();
                tvwKullanicilar.Nodes[0].Nodes.Clear();
                tvwKullanicilar.Nodes[1].Nodes.Clear();
            }
            else
            {
                MessageBox.Show("Bir sunucuya bağlı değilsiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MesajAlma()
        {
            try
            {
                while (soket.Connected)
                {
                    byte[] byteMesajUzunlugu = new byte[2];
                    soket.Receive(byteMesajUzunlugu, 2, SocketFlags.None);

                    byte[] mesajBytes = new byte[byteToUShort(byteMesajUzunlugu)];
                    soket.Receive(mesajBytes, mesajBytes.Length, SocketFlags.None);

                    string mesaj = Encoding.UTF8.GetString(mesajBytes); MessageBox.Show(mesaj);
                    string[] par = mesaj.Split(':');

                    switch (mesajBytes[0])
                    {
                        case (byte)komutlar.genelmesaj:
                            GenelMesajAlindi(mesajBytes); break;
                        case (byte)komutlar.ozelmesaj:
                            OzelMesajAlindi(mesajBytes); break;
                        case (byte)komutlar.kullanicigirdi:

                        default:
                            break;
                    }


                    if (mesaj.StartsWith("kgiris"))
                    {
                        tvwKullanicilar.Invoke((MethodInvoker)delegate { tvwKullanicilar.Nodes[1].Nodes.Add(par[1], par[1]); });
                    }
                    else if (mesaj.StartsWith("kcikis"))
                    {
                        tvwKullanicilar.Invoke((MethodInvoker)delegate { tvwKullanicilar.Nodes[1].Nodes.RemoveByKey(par[1]); });
                    }
                    else if (mesaj.StartsWith("giris"))
                    {
                        if (par[1] == "basarisiz")
                        {
                            MessageBox.Show(mesaj);
                            break;
                        }
                    }
                    else if (mesaj.StartsWith("odayaGirdi"))
                    {
                        odayaKullaniciEkle(par[1], par[2], 0);
                    }
                    else if (mesaj.StartsWith("odadanCikti"))
                    {
                        string cikilanOdA = par[1];
                        string odadanCikan = par[2];

                        tvwOdalar.Invoke((MethodInvoker)delegate {
                            tvwOdalar.Nodes[0].Nodes[cikilanOdA].Nodes.RemoveByKey(odadanCikan);
                        });
                    }
                    else if (mesaj.StartsWith("odaEkle"))
                    {
                        bool sifreliMi = (par[2] == "sifreli") ? true : false;
                        odaOlustur(par[1], sifreliMi);
                    }
                    else if (mesaj.Substring(1).StartsWith("kullanicilar"))
                    {
                        List<byte> byteListesi = mesajBytes.ToList();
                        byteListesi.RemoveRange(0, 13);
                        byte[] byteDizisi = byteListesi.ToArray();

                        ushort index = 0;
                        byte kullaniciSayisi = byteDizisi[index++];

                        for (int i = 0; i < kullaniciSayisi; i++)
                        {
                            byte yetki = byteDizisi[index++];
                            byte adUzunlugu = byteDizisi[index++];
                            string ad = byteToString(byteDizisi, index, adUzunlugu);
                            index += adUzunlugu;
                            kullaniciEkle(yetki, ad, byteDizisi[index++], byteDizisi[index++], byteDizisi[index++]);
                        }
                    }
                    else if (mesaj.Substring(1).StartsWith("odalar"))
                    {
                        odalariGuncelle(mesajBytes);
                    }
                    else
                    {
                        //MessageBox.Show(mesaj);
                    }
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show("SocketException: " + ex.Message);
                BaglantiyiKes();
                tMesajAlma.Abort();
            }
        }

        private void odalariGuncelle(byte[] byteDizi)
        {
            List<byte> byteListesi = byteDizi.ToList();
            byteListesi.RemoveRange(0, 7); //komut ve komut uzunluğu çıkartıldı
            byteDizi = byteListesi.ToArray();
            ushort index = 0;
            byte odaSayisi = byteDizi[index++];

            for (int i = 0; i < odaSayisi; i++)
            {
                byte odaAdiUzunlugu = byteDizi[index++];
                string odaAdi = byteToString(byteDizi, index, odaAdiUzunlugu);
                index += odaAdiUzunlugu;
                bool odaSifresiVar = byteDizi[index++] == 1 ? true : false;
                odaOlustur(odaAdi, odaSifresiVar);
                if (odaSifresiVar) { continue; }
                byte odadakiKullaniciSayisi = byteDizi[index++];
                for (int j = 0; j < odadakiKullaniciSayisi; j++)
                {
                    byte kullaniciAdiUzunlugu = byteDizi[index++];
                    string odadakiKullanici = byteToString(byteDizi, index, kullaniciAdiUzunlugu);
                    index += kullaniciAdiUzunlugu;
                    byte kullanicininDurumu = byteDizi[index++];
                    odayaKullaniciEkle(odaAdi, odadakiKullanici, kullanicininDurumu);
                }
            }
        }

        private void kullaniciEkle(byte yetki, string ad, byte r, byte g, byte b)
        {
            tvwKullanicilar.Invoke((MethodInvoker)delegate {
                tvwKullanicilar.Nodes[yetki].Nodes.Add(ad, ad);
                tvwKullanicilar.Nodes[yetki].Nodes[ad].ForeColor = Color.FromArgb(r, g, b);
                tvwKullanicilar.Nodes[yetki].Expand();
            });
        }

        /// <summary>
        /// Uzunluk bytelarını ekleyip mesajı sunucuya gönderir
        /// </summary>
        public void MesajYolla(byte[] mesajBytes)
        {
            ushort uzunluk = (ushort)mesajBytes.Length;
            byte[] uBytes = BitConverter.GetBytes(uzunluk);

            List<byte> kopyaBytes = new List<byte>();
            kopyaBytes.Add(uBytes[0]);
            kopyaBytes.Add(uBytes[1]);
            kopyaBytes.AddRange(mesajBytes);
            mesajBytes = kopyaBytes.ToArray();

            if (soket.Connected)
                soket.Send(mesajBytes, mesajBytes.Length, SocketFlags.None);
            else
                MessageBox.Show("Sunucuya bağlantın yok kardeşim");  
        }

        private void GenelMesajAlindi(byte[] byteMesaj)
        {
            ushort index = 1;
            byte gonderenAdiUzunlugu = byteMesaj[index++];
            string mesajiGonderen = Encoding.UTF8.GetString(byteMesaj, index, gonderenAdiUzunlugu);
            index += gonderenAdiUzunlugu;
            string mesaj = Encoding.UTF8.GetString(byteMesaj, index, byteMesaj.Length - index);

            rtxG.Invoke((MethodInvoker)delegate {
                rtxG.SelectionStart = rtxG.TextLength;
                rtxG.SelectionLength = 0;
                rtxG.SelectionColor = tvwKullanicilar.Nodes["kullanicilar"].Nodes[mesajiGonderen].ForeColor;
                rtxG.AppendText(mesajiGonderen + ": ");
                rtxG.ScrollToCaret();
                rtxG.SelectionColor = Color.Black;
                rtxG.AppendText(mesaj + Environment.NewLine);
            });
        }

        private void OzelMesajAlindi(byte[] byteMesaj)
        {
            bool tabPageVar = false;
            ushort index = 1;

            byte gonderenAdiUzunlugu = byteMesaj[index++];
            string gonderenAdi = Encoding.UTF8.GetString(byteMesaj, index, gonderenAdiUzunlugu);
            index += gonderenAdiUzunlugu;

            byte aliciAdiUzunlugu = byteMesaj[index++];
            string aliciAdi = Encoding.UTF8.GetString(byteMesaj, index, gonderenAdiUzunlugu);
            index += aliciAdiUzunlugu;

            string mesaj = Encoding.UTF8.GetString(byteMesaj, index, byteMesaj.Length - index);


            if (gonderenAdi != kullaniciAdi)
            {

                if (!tabAna.Controls.ContainsKey(gonderenAdi))
                {
                    tp = new TabPage(gonderenAdi);
                    tp.Name = "tp" + gonderenAdi;

                    rtx = new RichTextBox();
                    rtx.Name = "rtx" + gonderenAdi;
                    rtx.Dock = DockStyle.Fill;
                    rtx.ReadOnly = true;

                    tabAna.Invoke((MethodInvoker)delegate { tabAna.TabPages.Add(tp);});
                    tp.Invoke((MethodInvoker)delegate { tp.Controls.Add(rtx); });
                }


                foreach (var ctl in tabAna.Controls.OfType<TabPage>())
                {
                    foreach (var ctlrtx in ctl.Controls.OfType<RichTextBox>())
                    {
                        if (ctlrtx.Name == "rtx" + gonderenAdi)
                        {
                            rtx.Invoke((MethodInvoker)delegate { 
                                ctlrtx.Text += gonderenAdi + " >> " + mesaj + Environment.NewLine;
                            });
                            break;
                        }
                    }
                }
            }
            else if (gonderenAdi == kullaniciAdi)
            {
                foreach (var ctl in tabAna.Controls.OfType<TabPage>())
                {
                    foreach (var ctlrtx in ctl.Controls.OfType<RichTextBox>())
                    {
                        if (ctlrtx.Name == "rtx" + aliciAdi)
                        {
                            rtx.Invoke((MethodInvoker)delegate { ctlrtx.Text += aliciAdi + " >> " + mesaj + Environment.NewLine;}); break;
                        }
                    }
                }
            }

        }

        private void frmAna_FormClosing(object sender, FormClosingEventArgs e)
        {
            BaglantiyiKes();
        }

        private void rtxMesajim_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter) && rtxMesajim.TextLength > 1)
            {
                List<byte> byteListesi = new List<byte>();

                if (tabAna.SelectedIndex == 0)
                {
                    string genelMesaj = (char)komutlar.genelmesaj + rtxMesajim.Text.Substring(0, rtxMesajim.TextLength - 1);
                    byteListesi.AddRange(Encoding.UTF8.GetBytes(genelMesaj));
                }
                else
                {
                    byte[] aliciBytes = Encoding.UTF8.GetBytes(tabAna.SelectedTab.Text);
                    byte aliciAdiUzunlugu = (byte)((char)aliciBytes.Length);
                    byte[] mesajByte = Encoding.UTF8.GetBytes(rtxMesajim.Text.Substring(0, rtxMesajim.TextLength - 1));
                    byteListesi.Add((byte)((char)komutlar.ozelmesaj));
                    byteListesi.Add(aliciAdiUzunlugu);
                    byteListesi.AddRange(aliciBytes);
                    byteListesi.AddRange(mesajByte);  
                }

                MesajYolla(byteListesi.ToArray());
                rtxMesajim.Clear();
            }
            else if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                rtxMesajim.Clear();
            }
        }

        private void tsbKanalOlustur_Click(object sender, EventArgs e)
        {
            using (frmOdaOlustur foo = new frmOdaOlustur(kullaniciAdi))
            {
                foo.ShowDialog();
                if (foo.komut != null) { MesajYolla(Encoding.UTF8.GetBytes(foo.komut)); }
            }
        }

        private ushort byteToUShort(byte[] bytes)
        {
            byte[] uByte = { bytes[0], bytes[1] };
            return BitConverter.ToUInt16(uByte, 0);
        }

        private void tsmiBaglan_Click(object sender, EventArgs e)
        {
            fGiris.ShowDialog();
        }

        private void tsmiBaglantiyiKes_Click(object sender, EventArgs e)
        {
            BaglantiyiKes();
        }

        private void odaOlustur(string odaAdi, bool sifreliMi)
        {
            int imgIndex = sifreliMi ? 3 : 1;
            tvwOdalar.Invoke((MethodInvoker)delegate {
                tvwOdalar.Nodes[0].Nodes.Add(odaAdi, odaAdi, imgIndex, imgIndex);
                tvwOdalar.Nodes[0].Expand();
            });
        }

        private void odayaKullaniciEkle(string girilenOdA, string odayaGiren, byte durum)
        {
            tvwOdalar.Invoke((MethodInvoker)delegate {
                tvwOdalar.Nodes[0].Nodes[girilenOdA].Nodes.Add(odayaGiren, odayaGiren, 0, 0);
                tvwOdalar.Nodes[0].Nodes[girilenOdA].Expand();
            });
        }

        public static string byteToString(byte[] byteDizisi, ushort baslangic = 0, int uzunluk = default)
        {
            return Encoding.UTF8.GetString(byteDizisi, baslangic, uzunluk);
        }

        private void tvwOdalar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string secilenOda = tvwOdalar.SelectedNode.Text;
            if (tvwOdalar.Nodes[0].Nodes.ContainsKey(secilenOda))
            {
                if (tvwOdalar.SelectedNode.ImageIndex == 3)
                {
                    using (frmOdaGiris fog = new frmOdaGiris(secilenOda))
                    {
                        fog.ShowDialog();
                        string odaSifresi = fog.odaSifresi;
                        MesajYolla(Encoding.UTF8.GetBytes("odayaGiris:" + secilenOda + ":" + odaSifresi));
                    }
                }
                else
                {
                    MesajYolla(Encoding.UTF8.GetBytes("odayaGiris:" + secilenOda + ":"));
                }

            }

        }

        private void tvwKullanicilar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!tvwKullanicilar.Nodes[1].Nodes.ContainsKey(tvwKullanicilar.SelectedNode.Text))
            {
                return;
            }

            string alici = tvwKullanicilar.SelectedNode.Text;
            bool tabPageVar = false;
            foreach (var ctl in tabAna.Controls.OfType<Control>())
            {
                if (ctl.Text == alici)
                {
                    tabPageVar = true;
                    break;
                }
            }

            if (!tabPageVar)
            {
                rtx = new RichTextBox();
                rtx.Name = "rtx" + alici;
                rtx.Dock = DockStyle.Fill;
                rtx.Multiline = true;

                tp = new TabPage(alici);
                tp.Name = "tp" + alici;

                tabAna.Invoke((MethodInvoker)delegate { tabAna.TabPages.Add(tp); });

                tp.Invoke((MethodInvoker)delegate { tp.Controls.Add(rtx); });
            }
        }

        private void tvwKullanicilar_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void tvwOdalar_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
