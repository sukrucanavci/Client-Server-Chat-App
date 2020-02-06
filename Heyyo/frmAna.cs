using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;

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
        private bool baglantiVar = false;
        frmGiris fGiris = new frmGiris();
        Thread tMesajAlma;
        private void frmAna_Load(object sender, EventArgs e)
        {
            lstKullanicilar.Sorted = true;
            tvwOdalar.ImageList = imgOdalar;
            tabAna.ImageList = imgSayilar;

            
        }

        private void frmAna_Shown(object sender, EventArgs e)
        {
            fGiris.ShowDialog();
        }

        public void Baglan()
        {
            if (baglantiVar)
            {
                BaglantiyiKes();
            }
            soket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            soket.Connect(fGiris.lep);
            baglantiVar = true;
            kullaniciAdi = fGiris.kullaniciAdi;
            mesajYolla("nickname:" + kullaniciAdi);

            tMesajAlma = new Thread(MesajAlma);
            tMesajAlma.Start();
        }

        public void BaglantiyiKes()
        {
            if (baglantiVar)
            {
                rtxMesajlar.AppendText(soket.RemoteEndPoint + " bağlantısı kapatıldı.");
                baglantiVar = false;
                soket.Close();
                tMesajAlma.Abort();
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
                while (baglantiVar)
                {
                    byte[] byteMesajUzunlugu = new byte[2];
                    soket.Receive(byteMesajUzunlugu, 2, SocketFlags.None);

                    byte[] mesajBytes = new byte[byteToShort(byteMesajUzunlugu)];
                    soket.Receive(mesajBytes, mesajBytes.Length, SocketFlags.None);

                    string mesaj = Encoding.UTF8.GetString(mesajBytes);
                    string[] par = mesaj.Split(':');

                    if (mesaj.StartsWith("gMesaj"))
                    {
                        GenelMesajAlindi(mesaj.Substring(7));
                    }
                    else if (mesaj.StartsWith("oMesaj"))
                    {
                        OzelMesajAlindi(mesaj);
                    }
                    else if (mesaj.StartsWith("kgiris"))
                    {
                        lstKullanicilar.Invoke((MethodInvoker)delegate { lstKullanicilar.Items.Add(par[1]); });
                    }
                    else if (mesaj.StartsWith("kcikis"))
                    {
                        lstKullanicilar.Invoke((MethodInvoker)delegate { lstKullanicilar.Items.Remove(par[1]); });
                    }
                    else if (mesaj.StartsWith("giris"))
                    {
                        if (par[1] == "basarisiz")
                        {
                            MessageBox.Show(mesaj);
                            baglantiVar = false;
                            break;
                        }
                    }
                    else if (mesaj.StartsWith("odayaGirdi"))
                    {
                        string girilenOdA = par[1];
                        string odayaGiren = par[2];

                        tvwOdalar.Invoke((MethodInvoker)delegate {
                            tvwOdalar.Nodes[0].Nodes[girilenOdA].Nodes.Add(odayaGiren, odayaGiren, 0, 0);
                            tvwOdalar.Nodes[0].Nodes[girilenOdA].Expand();
                        });
                    }
                    else if (mesaj.StartsWith("odadanCikti"))
                    {
                        string cikilanOdA = par[1];
                        string odadanCikan = par[2];

                        tvwOdalar.Invoke((MethodInvoker)delegate {
                            tvwOdalar.Nodes[0].Nodes[cikilanOdA].Nodes.RemoveByKey(odadanCikan);
                            //tvwOdalar.Nodes[0].Nodes[cikilanOdA].Expand();
                        });
                    }
                    else if (mesaj.StartsWith("odaEkle"))
                    {
                        bool sifreliMi = (par[2] == "sifreli") ? true : false;
                        odaOlustur(par[1], sifreliMi);
                    }
                    else if (mesaj.StartsWith("kullanicilar"))
                    {
                        try
                        {
                            string[] kullanicilar = mesaj.Substring(13).Split(':');
                            lstKullanicilar.Invoke((MethodInvoker)delegate { lstKullanicilar.Items.AddRange(kullanicilar); });
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        GenelMesajAlindi(mesaj);
                    }
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show("SocketException: " + ex.Message);
                tMesajAlma.Abort();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex.Message);
            }
        }

        public void mesajYolla(string veri)
        {
            byte[] byteVeri = Encoding.UTF8.GetBytes(veri);
            short uzunluk = (short)byteVeri.Length;
            byte[] uBytes = BitConverter.GetBytes(uzunluk);

            List<byte> kopyaBytes = new List<byte>();
            kopyaBytes.Add(uBytes[0]);
            kopyaBytes.Add(uBytes[1]);
            kopyaBytes.AddRange(byteVeri);
            byteVeri = kopyaBytes.ToArray();

            if (soket.Connected)
            {
                soket.Send(byteVeri, byteVeri.Length, SocketFlags.None);
            }
            else
            {
                MessageBox.Show("Sunucuya bağlantın yok kardeşim");
            }
            
        }

        private void GenelMesajAlindi(string mesaj)
        {
            rtxMesajlar.Invoke((MethodInvoker)delegate{ rtxMesajlar.AppendText(mesaj + Environment.NewLine); });
        }
        
        private void OzelMesajAlindi(string hMesaj)
        {
            bool tabPageVar = false;

            string gonderici = hMesaj.Split(':')[1];
            string alici = hMesaj.Split(':')[2];

            int mBaslangic = gonderici.Length + alici.Length + 9;
            string mesaj = hMesaj.Substring(mBaslangic);
            
            if (gonderici != kullaniciAdi)
            {
                foreach (var ctl in tabAna.Controls.OfType<Control>())
                {
                    if (ctl.Text == gonderici)
                    {
                        tabPageVar = true;
                        break;
                    }
                }

                if (!tabPageVar)
                {
                    rtx = new RichTextBox();
                    rtx.Name = "rtx" + gonderici;
                    rtx.Dock = DockStyle.Fill;
                    //rtx.Multiline = true;

                    tp = new TabPage(gonderici);
                    tp.Name = "tp" + gonderici;
                    tp.ImageIndex = 0;

                    tabAna.Invoke((MethodInvoker)delegate { tabAna.TabPages.Add(tp);});
                    
                    tp.Invoke((MethodInvoker)delegate { tp.Controls.Add(rtx); });
                }

                foreach (var ctl in tabAna.Controls.OfType<Control>())
                {
                    foreach (var ctlrtx in ctl.Controls.OfType<Control>())
                    {
                        if (ctlrtx.Name == "rtx" + gonderici)
                        {
                            rtx.Invoke((MethodInvoker)delegate { ctlrtx.Text += gonderici + " >> " + mesaj + Environment.NewLine;});

                            break;
                        }
                    }
                }
            }
            else if (gonderici == kullaniciAdi)
            {
                foreach (var ctl in tabAna.Controls.OfType<Control>())
                {
                    foreach (var ctlrtx in ctl.Controls.OfType<Control>())
                    {
                        if (ctlrtx.Name == "rtx" + alici)
                        {
                            rtx.Invoke((MethodInvoker)delegate { ctlrtx.Text += gonderici + " >> " + mesaj + Environment.NewLine;});
                            
                            break;
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
                string mesaj = default;

                if (tabAna.SelectedIndex == 0)
                {
                    mesaj = "gMesaj:" + rtxMesajim.Text;
                }
                else
                {
                    string alici = tabAna.SelectedTab.Text;
                    mesaj = "oMesaj:" + alici + ":" + rtxMesajim.Text;
                }
                mesaj = mesaj.Substring(0, mesaj.Length - 1); //Son karakteri yani ENTER'ı siliyoruz
                mesajYolla(mesaj);
                rtxMesajim.Clear();
            }
            else if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                rtxMesajim.Clear();
            }
        }

        private void mesajGonderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstKullanicilar.SelectedIndex == -1)
            {
                return;
            }

            string alici = lstKullanicilar.SelectedItem.ToString();
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
            using (frmOdaOlustur foo = new frmOdaOlustur(kullaniciAdi))
            {
                foo.ShowDialog();
                if (foo.komut != null) { mesajYolla(foo.komut); }
            }
        }

        private void odaOlustur(string odaAdi, bool sifreliMi)
        {
            int imgIndex = sifreliMi ? 3 : 1; 
            tvwOdalar.Invoke((MethodInvoker)delegate { tvwOdalar.Nodes[0].Nodes.Add(odaAdi, odaAdi, imgIndex, imgIndex);
                tvwOdalar.Nodes[0].Expand();
                //tvwOdalar.Nodes[0].Nodes[odaAdi].Expand();
            });
        }

        private void tvwOdalar_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
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
                        mesajYolla("odayaGiris:" + secilenOda + ":" + odaSifresi);
                    }
                }
                else
                {
                    mesajYolla("odayaGiris:" + secilenOda + ":");
                }

            }

        }

        private short byteToShort(byte[] bytes)
        {
            byte[] uByte = { bytes[0], bytes[1] };
            return BitConverter.ToInt16(uByte, 0);
        }

        private void bağlanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fGiris.ShowDialog();
        }

        private void bağlantıyıKesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaglantiyiKes();
        }
    }
}
