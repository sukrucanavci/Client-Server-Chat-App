using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Heyyo_Sunucu
{
    class Istemci
    {
        private Thread tOdaEkle;
        private Oda oda;
        public Socket soket;
        public string kullaniciAdi;
        private byte[] kullaniciAdiRengi = { 0, 0, 255};
        public Oda bulunduguOda = null;
        byte sesDurumu = default;
        byte yetki = 1;

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

        public Istemci(Socket soket)
        {
            this.soket = soket;
            MesajAlma();
        }
        
        void MesajAlma()
        {

            try
            {
                while (true)
                {
                    byte[] byteMesajUzunlugu = new byte[2];
                    int a1 = soket.Receive(byteMesajUzunlugu, 0, 2, SocketFlags.None);

                    ushort gelecekMesajinUzunlugu = Sunucu.ByteToUshort(byteMesajUzunlugu);
                    if (gelecekMesajinUzunlugu == 0) { throw new SocketException(); }

                    byte[] alinanbytes = new byte[gelecekMesajinUzunlugu];

                    int offset = 0;
                    while (offset < alinanbytes.Length)
                    {
                        int received = soket.Receive(alinanbytes, offset, alinanbytes.Length - offset, 0);
                        offset += received;
                        if (received == 0) { throw new SocketException(); }
                    }

                    string alinanString = Sunucu.byteToString(alinanbytes);
                    Console.WriteLine("Alındı -> " + soket.RemoteEndPoint.ToString() + " -> " + alinanString);

                    string[] par = alinanString.Split(':');

                    switch ((byte)alinanString[0])
                    {
                        case (byte)komutlar.kullaniciadi:
                            if (!KullaniciAdiKontrol(alinanbytes)) { throw new SocketException(); }; break;
                        case (byte)komutlar.genelmesaj:
                            GenelMesajGonder(alinanbytes); break;
                        case (byte)komutlar.ozelmesaj:
                            OzelMesajGonder(alinanbytes); break;
                        default:
                            break;
                    }

                    if (alinanString.StartsWith("odaEkle"))
                    {
                        if (!Sunucu.odaListesi.ContainsKey(par[1]))
                        {
                            tOdaEkle = new Thread(() => oda = new Oda(alinanString));
                            tOdaEkle.Start();
                        }     
                    }
                    else if (alinanString.StartsWith("odayaGiris"))
                    {
                        string odaAdi = par[1];
                        string odaSifresi = par[2];

                        foreach (var item in Sunucu.odaListesi)
                        {
                            if (item.Key == odaAdi)
                            {
                                if (item.Value.odayaKullaniciEkle(this, odaSifresi))
                                {
                                    Sunucu.kullanicilaraMesajGonder("odayaGirdi:" + odaAdi + ":" + kullaniciAdi, this);

                                    //Bağlanmak isteyen kullanıcıya oda bağlantı bilgileri verilecek
                                    mesajGonder(soket, "odayaGirdi:" + odaAdi + ":" + kullaniciAdi);
                                }
                                else
                                {
                                    mesajGonder(soket, "odayaGiris:basarisiz:");
                                }
                                break;
                            }
                            
                        }
                    }
                    else if (alinanString.StartsWith("odadanCikis"))
                    {

                    }
                }
            }
            catch (SocketException)
            {
                if (Sunucu.istemciListesi.ContainsValue(this))
                {
                    Sunucu.kullanicilaraMesajGonder(("kcikis:" + kullaniciAdi), this);
                    if (bulunduguOda != null) { bulunduguOda.bagliIstemciler.Remove(this); }
                    Sunucu.istemciListesi.Remove(kullaniciAdi);
                    Console.WriteLine(kullaniciAdi + " çıkış yaptı.");
                }
                soket.Close();
                Thread.CurrentThread.Abort();
            }
        }

        private void GenelMesajGonder(byte[] mesajBytes)
        {
            byte[] kullaniciAdiBytes = Encoding.UTF8.GetBytes(kullaniciAdi);
            byte kullaniciAdiUzunlugu = (byte)((char)kullaniciAdiBytes.Length);
            List<byte> byteList = new List<byte>(mesajBytes);
            byteList.Insert(1, kullaniciAdiUzunlugu);
            byteList.InsertRange(2, kullaniciAdiBytes);
            mesajBytes = byteList.ToArray();
            byte[] bytes = Sunucu.UzunlukBE(mesajBytes);

            foreach (var istemci in Sunucu.istemciListesi.Values)
            {
                istemci.soket.Send(bytes, bytes.Length, SocketFlags.None);
            }
        }

        private void OzelMesajGonder(byte[] mesajBytes)
        {
            List<byte> byteList = new List<byte>(mesajBytes);

            byte[] kullaniciAdiBytes = Encoding.UTF8.GetBytes(kullaniciAdi);
            byte kullaniciAdiUzunlugu = (byte)((char)kullaniciAdiBytes.Length);
            byteList.Insert(1, kullaniciAdiUzunlugu);
            byteList.InsertRange(2, kullaniciAdiBytes);

            byte aliciAdiUzunlugu = (byte)((char)mesajBytes[1]);
            string alici = Encoding.UTF8.GetString(mesajBytes, 2, aliciAdiUzunlugu);

            Socket aliciSoket = default;
            foreach (var item in Sunucu.istemciListesi)
            {
                if (item.Value.kullaniciAdi == alici)
                    aliciSoket = item.Value.soket; break;
            }

            MesajGonder(aliciSoket, byteList.ToArray());
            MesajGonder(this.soket, byteList.ToArray());
        }

        bool KullaniciAdiKontrol(byte[] bytes)
        {
            kullaniciAdi = Encoding.UTF8.GetString(bytes, 1, bytes.Length - 1);

            if (Sunucu.istemciListesi.Keys.Contains(kullaniciAdi))
            {
                List<byte> byteList = new List<byte>();
                byteList.Add((byte)((char)komutlar.girisbasarisiz));
                byteList.AddRange(Sunucu.StringToByte("Bu kullanıcı adı zaten kullanımda!"));
                MesajGonder(this.soket, byteList.ToArray());
                return false;
            }
            else if (!kullaniciAdi.Substring(0, 3).Contains(" ") && kullaniciAdi.Length >= 3)
            {
                Sunucu.istemciListesi.Add(kullaniciAdi, this);
                Sunucu.kullanicilaraMesajGonder(("kgiris:" + kullaniciAdi), this);
                kullanicilariGonder();
                odalariGonder();
                return true;
            }
            else
            {
                List<byte> byteList = new List<byte>();
                byteList.Add((byte)((char)komutlar.girisbasarisiz));
                byteList.AddRange(Sunucu.StringToByte("Bilinmeyen bir hatadan ötürü sunucu giriş yapmanıza izin vermedi!"));
                MesajGonder(this.soket, byteList.ToArray());
                return false;
            }
        }

        private void kullanicilariGonder()
        {
            List<byte> byteListesi = new List<byte>();
            byteListesi.Add(12);
            byteListesi.AddRange(Sunucu.StringToByte("kullanicilar")); //komut uzunluğu ve komut
            byteListesi.Add((byte)Sunucu.istemciListesi.Count); //kullanıcı sayısı

            foreach (var istemci in Sunucu.istemciListesi.Values)
            {
                byteListesi.Add(istemci.yetki); //yetki derecesi
                byte[] byteAd = Sunucu.StringToByte(istemci.kullaniciAdi);
                byteListesi.Add((byte)byteAd.Length); //kullanıcı adı uzunluğu
                byteListesi.AddRange(byteAd); //kullanıcı adı
                byteListesi.AddRange(istemci.kullaniciAdiRengi); //kullanıcı adı rgb değerleri
            }

            byte[] byteDizisi = Sunucu.UzunlukBE(byteListesi.ToArray());

            soket.Send(byteDizisi, byteDizisi.Length, SocketFlags.None);

            Console.WriteLine(Sunucu.byteToString(byteDizisi));
        }

        /// <summary>
        /// Mesajın başına uzunluğunu ekleyip hedef sokete mesajı gönderir
        /// </summary>
        private void MesajGonder(Socket hedefSoket, byte[] byteMesaj)
        {
            byte[] bytes = Sunucu.UzunlukBE(byteMesaj);
            hedefSoket.Send(bytes, bytes.Length, SocketFlags.None);
        }

        private void mesajGonder(Socket soket, string mesaj)
        {
            byte[] byteMesaj = Sunucu.UzunlukBE(Sunucu.StringToByte(mesaj));
            soket.Send(byteMesaj, byteMesaj.Length, SocketFlags.None);
        }

        private void odalariGonder()
        {
            List<byte> byteListesi = new List<byte>();
            byteListesi.Add(6);
            byteListesi.AddRange(Sunucu.StringToByte("odalar")); //komut
            byteListesi.Add((byte)Sunucu.odaListesi.Count); //oda sayısı

            foreach (var oda in Sunucu.odaListesi.Values)
            {

                byteListesi.Add((byte)oda.odaAdi.Length); //oda adı uzunluğu
                byteListesi.AddRange(Sunucu.StringToByte(oda.odaAdi)); //oda adı
                int sifreDurumu = oda.sifreVar ? 1 : 0; //sifre varsa 1 yoksa 0
                byteListesi.Add((byte)sifreDurumu); //sifre var mı
                if (oda.sifreVar) { continue; }; //sifre varsa bağlı kullanıcıları yollama
                byteListesi.Add((byte)oda.bagliIstemciler.Count); //odadaki kullanıcı sayısı

                foreach (var istemci in oda.bagliIstemciler)
                {
                    byteListesi.Add((byte)istemci.kullaniciAdi.Length); //kullanıcı adı uzunluğu
                    byteListesi.AddRange(Sunucu.StringToByte(istemci.kullaniciAdi)); //kullanıcı adı
                    byteListesi.Add(istemci.sesDurumu); //kullanıcının durumu 0-aktif 1-mic muted 2-all muted
                }

            }

            byte[] byteDizisi = Sunucu.UzunlukBE(byteListesi.ToArray());
            soket.Send(byteDizisi, byteDizisi.Length, SocketFlags.None);
        }
    }
}
