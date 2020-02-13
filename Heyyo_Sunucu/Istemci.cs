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
        private byte[] renk = { 0, 0, 255};
        public Oda bulunduguOda = null;
        byte sesDurumu = default;
        byte yetki = 1;

        #region Enums

        public enum komutlar
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

                    Console.WriteLine("Alındı -> " + soket.RemoteEndPoint.ToString() + " -> " + Encoding.UTF8.GetString(alinanbytes));

                    switch (alinanbytes[0])
                    {
                        case (byte)komutlar.kullaniciadi:
                            if (!KullaniciAdiKontrol(alinanbytes)) { throw new SocketException(); }; break;

                        case (byte)komutlar.genelmesaj:
                            GenelMesajGonder(alinanbytes); break;

                        case (byte)komutlar.ozelmesaj:
                            OzelMesajGonder(alinanbytes); break;

                        case (byte)komutlar.odaekle:
                                tOdaEkle = new Thread(() => oda = new Oda(alinanbytes)); tOdaEkle.Start();
                                break;

                        case (byte)komutlar.odayaGir:
                            OdayaGirmeIstegi(alinanbytes);
                            break;

                        case (byte)komutlar.odadancik:
                            break;

                        default:
                            break;
                    }

                }
            }
            catch (SocketException)
            {
                if (Sunucu.istemciListesi.ContainsValue(this))
                {
                    Sunucu.KullanicilaraMesajGonder((yetki + (char)komutlar.kullanicicikti + kullaniciAdi), this);
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
                MesajGonder(this.soket, (char)komutlar.girisbasarisiz + "Bu kullanıcı adı zaten kullanımda!");
                return false;
            }
            else if(kullaniciAdi.Substring(0, 3).Contains(" "))
            {
                MesajGonder(this.soket, (char)komutlar.girisbasarisiz + "İsminizin ilk 3 harfinde boşluk karakteri bulunamaz!");
                return false;
            }
            else if (!kullaniciAdi.Substring(0, 3).Contains(" ") && kullaniciAdi.Length >= 3)
            {
                Sunucu.istemciListesi.Add(kullaniciAdi, this);

                List<byte> byteList = new List<byte>();
                byte[] kullaniciAdiByte = Encoding.UTF8.GetBytes(kullaniciAdi);

                byteList.Add((byte)komutlar.kullanicigirdi);
                byteList.Add(yetki);
                byteList.Add((byte)kullaniciAdiByte.Length);
                byteList.AddRange(kullaniciAdiByte);
                byteList.AddRange(renk);

                Sunucu.KullanicilaraMesajGonder(byteList.ToArray(), this);

                kullanicilariGonder();
                odalariGonder();

                return true;
            }
            else
            {
                MesajGonder(this.soket, (char)komutlar.girisbasarisiz + "Bilinmeyen bir hatadan ötürü sunucu giriş yapmanıza izin vermedi!");
                return false;
            }
        }

        private void kullanicilariGonder()
        {
            List<byte> byteListesi = new List<byte>();
            byteListesi.Add((byte)komutlar.kullanicilistesi);
            byteListesi.Add((byte)Sunucu.istemciListesi.Count);

            foreach (var istemci in Sunucu.istemciListesi.Values)
            {
                byteListesi.Add(yetki);
                byte[] byteAd = Encoding.UTF8.GetBytes(istemci.kullaniciAdi);
                byteListesi.Add((byte)byteAd.Length);
                byteListesi.AddRange(byteAd);
                byteListesi.AddRange(istemci.renk);
            }

            byte[] byteDizisi = Sunucu.UzunlukBE(byteListesi.ToArray());
            soket.Send(byteDizisi, byteDizisi.Length, SocketFlags.None);
        }

        /// <summary>
        /// Mesajın başına uzunluğunu ekleyip hedef sokete mesajı gönderir
        /// </summary>
        private void MesajGonder(Socket hedefSoket, byte[] byteMesaj)
        {
            byte[] bytes = Sunucu.UzunlukBE(byteMesaj);
            hedefSoket.Send(bytes, bytes.Length, SocketFlags.None);
        }

        private void MesajGonder(Socket hedefSoket, string mesaj)
        {
            byte[] byteMesaj = Sunucu.UzunlukBE(Encoding.UTF8.GetBytes(mesaj));
            hedefSoket.Send(byteMesaj, byteMesaj.Length, SocketFlags.None);
        }

        private void odalariGonder()
        {
            List<byte> byteListesi = new List<byte>();
            byteListesi.Add((byte)((char)komutlar.odalistesi));
            byteListesi.Add((byte)Sunucu.odaListesi.Count);

            foreach (var oda in Sunucu.odaListesi.Values)
            {
                byteListesi.Add((byte)oda.odaAdi.Length);
                byteListesi.AddRange(Encoding.UTF8.GetBytes(oda.odaAdi));
                byte sifreDurumu = oda.sifreVar ? (byte)((char)cevap.evet) : (byte)((char)cevap.hayir);
                byteListesi.Add(sifreDurumu);
                if (oda.sifreVar) { continue; };
                byteListesi.Add((byte)oda.bagliIstemciler.Count);

                foreach (var istemci in oda.bagliIstemciler)
                {
                    byteListesi.Add((byte)istemci.kullaniciAdi.Length);
                    byteListesi.AddRange(Encoding.UTF8.GetBytes(istemci.kullaniciAdi));
                    byteListesi.Add(istemci.sesDurumu); //kullanıcının durumu 0-aktif 1-mic muted 2-all muted
                }

            }

            byte[] byteDizisi = Sunucu.UzunlukBE(byteListesi.ToArray());
            soket.Send(byteDizisi, byteDizisi.Length, SocketFlags.None);
        }

        private void OdayaGirmeIstegi(byte[] bytes)
        {
            ushort index = 1;

            byte odaAdiUzunlugu = bytes[index++];
            string odaAdi = Encoding.UTF8.GetString(bytes, index, odaAdiUzunlugu);
            index += odaAdiUzunlugu;

            byte odaSifresiUzunlugu = bytes[index];
            string odaSifresi = Encoding.UTF8.GetString(bytes, index, odaSifresiUzunlugu);

            foreach (var item in Sunucu.odaListesi)
                if (item.Key == odaAdi) { item.Value.odayaKullaniciEkle(this, odaSifresi); break; }
        }
    }
}
