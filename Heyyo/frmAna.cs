using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Collections;
using System.Drawing.Imaging;

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
        frmOdaOlustur foo = new frmOdaOlustur();
        Thread tMesajAlma;
        SortedList<string, string> emojiler = new SortedList<string, string>();

        #region Enums

        enum komutlar
        {
            kullaniciadi,
            genelmesaj,
            ozelmesaj,
            odalistesi,
            odaekle,
            odasil,
            odayaGir,
            odadancik,
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
            hayir
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

            #region Emojiler

            emojiler.Add(":)", Application.StartupPath + @"\emojiler\mutlu.png");

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
            try { if (soket.Connected) { BaglantiyiKes(); } } catch (Exception) { } 
            soket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            soket.Connect(fGiris.lep);
            kullaniciAdi = fGiris.kullaniciAdi;

            List<byte> byteList = Encoding.UTF8.GetBytes(kullaniciAdi).ToList();
            byteList.Insert(0, (byte)((char)komutlar.kullaniciadi));
            MesajGonder(byteList.ToArray());

            tMesajAlma = new Thread(MesajAlma);
            tMesajAlma.Start();
        }

        public void BaglantiyiKes()
        {
            if (soket.Connected)
            {
                rtxG.AppendText(Environment.NewLine + soket.RemoteEndPoint + " sunucu ile bağlantı kesildi.");
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

                    byte[] alinanBytes = new byte[byteToUShort(byteMesajUzunlugu)];
                    soket.Receive(alinanBytes, alinanBytes.Length, SocketFlags.None);

                    ushort index = 1;

                    switch (alinanBytes[0])
                    {
                        case (byte)komutlar.girisbasarisiz:
                            MessageBox.Show(Encoding.UTF8.GetString(alinanBytes, 1, alinanBytes.Length - 1)); BaglantiyiKes(); break;

                        case (byte)komutlar.genelmesaj:
                            GenelMesajAlindi(alinanBytes); break;

                        case (byte)komutlar.ozelmesaj:
                            OzelMesajAlindi(alinanBytes); break;

                        case (byte)komutlar.kullanicigirdi:
                            kullaniciEkle(alinanBytes, index); break;

                        case (byte)komutlar.kullanicicikti:
                            tvwKullanicilar.Invoke((MethodInvoker)delegate {
                                tvwKullanicilar.Nodes[alinanBytes[1]].Nodes.RemoveByKey(Encoding.UTF8.GetString(alinanBytes, 2, alinanBytes.Length - 2));
                            }); break;

                        case (byte)komutlar.kullanicilistesi:
                            byte kullaniciSayisi = alinanBytes[index++];
                            for (int i = 0; i < kullaniciSayisi; i++) { index = kullaniciEkle(alinanBytes, index); }
                            break;

                        case (byte)komutlar.odalistesi:
                            odalariGuncelle(alinanBytes); break;

                        case (byte)komutlar.odaekle:
                            OdaOlustur(alinanBytes); break;

                        case (byte)komutlar.odasil:
                            break;

                        case (byte)komutlar.kullaniciodayagirdi:
                            byte gireninAdUzunluk = alinanBytes[index++];
                            string odayaGiren = Encoding.UTF8.GetString(alinanBytes, index, gireninAdUzunluk);
                            index += gireninAdUzunluk;
                            byte girilenOdaUzunluk = alinanBytes[index++];
                            string girilenOda = Encoding.UTF8.GetString(alinanBytes, index, girilenOdaUzunluk);
                            odayaKullaniciEkle(girilenOda, odayaGiren, 0); break;

                        case (byte)komutlar.kullaniciodadancikti:
                            byte cikaninAdUzunluk = alinanBytes[index++];
                            string odadanCikan = Encoding.UTF8.GetString(alinanBytes, index, cikaninAdUzunluk);
                            index += cikaninAdUzunluk;
                            byte cikilanOdaUzunluk = alinanBytes[index++];
                            string cikilanOda = Encoding.UTF8.GetString(alinanBytes, index, cikilanOdaUzunluk);
                            tvwOdalar.Invoke((MethodInvoker)delegate {
                                tvwOdalar.Nodes[0].Nodes[cikilanOda].Nodes.RemoveByKey(odadanCikan);
                            }); break;

                        case (byte)komutlar.sunucumesaji:
                            MessageBox.Show(Encoding.UTF8.GetString(alinanBytes, 1, alinanBytes.Length - 1)); break;

                        default:
                            break;
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

                foreach (string emote in emojiler.Keys)
                {
                    while (rtxG.Text.Contains(emote))
                    {
                        int i = rtxG.Text.IndexOf(emote);
                        rtxG.Select(i, emote.Length);
                        
                        Image img = Image.FromFile(emojiler[emote]);

                        //Bitmap bmp = new Bitmap(24, 24);
                        //Graphics g = Graphics.FromImage(bmp);

                        //g.DrawImage(img, new Rectangle(0, 0, 24, 24), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, new ImageAttributes());

                        Clipboard.SetImage(Image.FromFile(emojiler[emote]));
                        rtxG.Paste();
                        Clipboard.Clear();
                    }
                }
            });

            
        }

        private void OzelMesajAlindi(byte[] byteMesaj)
        {
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

                    tabAna.Invoke((MethodInvoker)delegate { tabAna.TabPages.Add(tp); });
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
                            rtx.Invoke((MethodInvoker)delegate { ctlrtx.Text += aliciAdi + " >> " + mesaj + Environment.NewLine; }); break;
                        }
                    }
                }
            }

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

                MesajGonder(byteListesi.ToArray());
                rtxMesajim.Clear();
            }
            else if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                rtxMesajim.Clear();
            }
        }

        private void odalariGuncelle(byte[] byteDizi)
        {
            ushort index = 1;
            byte odaSayisi = byteDizi[index++];

            for (int i = 0; i < odaSayisi; i++)
            {
                byte odaAdiUzunlugu = byteDizi[index++];
                string odaAdi = Encoding.UTF8.GetString(byteDizi, index, odaAdiUzunlugu);
                index += odaAdiUzunlugu;
                bool odaSifresiVar = (byte)((char)byteDizi[index++]) == 1 ? true : false;

                int imgIndex = byteDizi[index++] == (byte)((char)cevap.evet) ? 3 : 1;
                tvwOdalar.Invoke((MethodInvoker)delegate {
                    tvwOdalar.Nodes[0].Nodes.Add(odaAdi, odaAdi, imgIndex, imgIndex);
                    tvwOdalar.Nodes[0].Expand();
                });

                if (odaSifresiVar) { continue; }
                byte odadakiKullaniciSayisi = byteDizi[index++];
                for (int j = 0; j < odadakiKullaniciSayisi; j++)
                {
                    byte kullaniciAdiUzunlugu = byteDizi[index++];
                    string odadakiKullanici = Encoding.UTF8.GetString(byteDizi, index, kullaniciAdiUzunlugu);
                    index += kullaniciAdiUzunlugu;
                    byte kullanicininDurumu = byteDizi[index++];
                    odayaKullaniciEkle(odaAdi, odadakiKullanici, kullanicininDurumu);
                }
            }
        }

        private ushort kullaniciEkle(byte[] bytes, ushort index)
        {
            byte yetki = bytes[index++];
            byte adUzunlugu = bytes[index++];
            string ad = Encoding.UTF8.GetString(bytes, index, adUzunlugu);
            index += adUzunlugu;

            tvwKullanicilar.Invoke((MethodInvoker)delegate {
                tvwKullanicilar.Nodes[yetki].Nodes.Add(ad, ad);
                tvwKullanicilar.Nodes[yetki].Nodes[ad].ForeColor = Color.FromArgb(bytes[index++], bytes[index++], bytes[index++]);
                tvwKullanicilar.Nodes[yetki].Expand();
            });

            return index;
        }

        public void MesajGonder(byte[] mesajBytes)
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

        private void OdaOlustur(byte[] byteDizi)
        {
            ushort index = 1;
            byte odaAdiUzunlugu = byteDizi[index++];
            string odaAdi = Encoding.UTF8.GetString(byteDizi, index, odaAdiUzunlugu);
            index += odaAdiUzunlugu;
            int imgIndex = byteDizi[index] == (byte)((char)cevap.evet) ? 3 : 1;

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

        private void tvwOdalar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            List<byte> byteList = new List<byte>();
   
            string secilenOda = tvwOdalar.SelectedNode.Text;
            if (!tvwOdalar.Nodes[0].Nodes.ContainsKey(secilenOda)) { return; }

            byte[] odaAdiByte = Encoding.UTF8.GetBytes(secilenOda);
            byteList.Add((byte)((char)komutlar.odayaGir));
            byteList.Add((byte)odaAdiByte.Length);
            byteList.AddRange(odaAdiByte);

            if(tvwOdalar.Nodes[0].Nodes[secilenOda].ImageIndex == 3)
            {
                using (frmOdaGiris fog = new frmOdaGiris(secilenOda))
                {
                    fog.ShowDialog();
                    if(fog.odaSifresi == null) { return; }
                    string odaSifresi = fog.odaSifresi;
                    byte[] odaSifresiByte = Encoding.UTF8.GetBytes(odaSifresi);
                    byteList.Add((byte)odaSifresiByte.Length);
                    byteList.AddRange(odaSifresiByte);
                }
            }
            else { byteList.Add(0); }

            MesajGonder(byteList.ToArray());

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

        private void tsbKanalOlustur_Click(object sender, EventArgs e)
        {
            foo.ShowDialog();

            if(foo.odaAdi == null) { return; }

            List<byte> byteList = new List<byte>();
  
            byte[] odaAdiByte = Encoding.UTF8.GetBytes(foo.odaAdi);
            byte[] sifreByte = Encoding.UTF8.GetBytes(foo.odaSifresi);

            byteList.Add((byte)((char)komutlar.odaekle));
            byteList.Add((byte)((char)odaAdiByte.Length));
            byteList.AddRange(odaAdiByte);
            byteList.Add((byte)((char)sifreByte.Length));
            byteList.AddRange(sifreByte);

            MesajGonder(byteList.ToArray());
        }

        private ushort byteToUShort(byte[] bytes)
        {
            byte[] uByte = { bytes[0], bytes[1] };
            return BitConverter.ToUInt16(uByte, 0);
        }

        private void tvwKullanicilar_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void tvwOdalar_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void tsmiBaglan_Click(object sender, EventArgs e)
        {
            fGiris.ShowDialog();
        }

        private void tsmiBaglantiyiKes_Click(object sender, EventArgs e)
        {
            BaglantiyiKes();
        }

        private void frmAna_FormClosing(object sender, FormClosingEventArgs e)
        {
            BaglantiyiKes();
        }
    }
}
