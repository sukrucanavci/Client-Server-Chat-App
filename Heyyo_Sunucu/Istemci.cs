using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Heyyo_Sunucu
{
    class Istemci
    {
        public Socket soket;
        private Oda oda;
        public string kullaniciAdi;
        Thread tOdaEkle;
        public Oda bulunduguOda = null;
        byte sesDurumu; 

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

                    ushort gelecekMesajinUzunlugu = Sunucu.byteToUShort(byteMesajUzunlugu);
                    if (gelecekMesajinUzunlugu == 0) { throw new SocketException(); }


                    byte[] alinanbytes = new byte[gelecekMesajinUzunlugu];
                    int offset = 0;

                    while (offset < alinanbytes.Length)
                    {
                        int received = soket.Receive(alinanbytes, offset, alinanbytes.Length - offset, 0);
                        offset += received;
                        if (received == 0)
                        {
                            throw new SocketException();
                        }
                    }

                    string alinanString = Sunucu.byteToString(alinanbytes);
                    Console.WriteLine("Alındı -> " + soket.RemoteEndPoint.ToString() + " -> " + alinanString);

                    string[] par = alinanString.Split(':');

                    if (alinanString.StartsWith("gMesaj"))
                    {
                        string gonderilecekMesaj = alinanString.Substring(7);
                        string gondereniEklenmisMesaj = par[0] + ":" + kullaniciAdi + ":" + gonderilecekMesaj;
                        Sunucu.tumKullanicilaraMesajGonder(gondereniEklenmisMesaj);
                    }
                    else if (alinanString.StartsWith("oMesaj"))
                    {
                        string alici = par[1];

                        Socket aliciSoket = default;
                        foreach (var item in Sunucu.istemciListesi)
                        {
                            if (item.Value.kullaniciAdi == alici)
                            {
                                aliciSoket = item.Value.soket;
                                break;
                            }
                        }

                        int mesajBaslangicIndexi = par[0].Length + par[1].Length + 2;
                        string gonderilecekMesaj = alinanString.Substring(mesajBaslangicIndexi);
                        string gondereniEklenmisMesaj = par[0] + ":" + kullaniciAdi + ":" + par[1] + ":" + gonderilecekMesaj;

                        mesajGonder(aliciSoket, gondereniEklenmisMesaj);
                        mesajGonder(soket, gondereniEklenmisMesaj);
                    }
                    else if (alinanString.StartsWith("nickname"))
                    {
                        kullaniciAdi = par[1];
                        if (Sunucu.istemciListesi.Keys.Contains(kullaniciAdi))
                        {
                            mesajGonder(soket, "giris:basarisiz:Bu kullanıcı adı zaten kullanımda!");
                            throw new Exception();
                        }
                        else if (!kullaniciAdi.Substring(0,3).Contains(" ") && kullaniciAdi.Length >= 3)
                        {
                            Sunucu.istemciListesi.Add(kullaniciAdi, this);
                            Sunucu.tumKullanicilaraMesajGonder(("kgiris:" + kullaniciAdi), this.soket);
                            kullanicilariGonder();
                            odalariGonder();
                        }
                        else
                        {
                            mesajGonder(soket, "giris:basarisiz:Kullanıcı adınız en az 3 harften oluşmalı ve ilk 3 karakter içerisinde boşluk bulunmamalı!");
                            throw new Exception();
                        }

                    }
                    else if (alinanString.StartsWith("odaEkle"))
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
                                    Sunucu.tumKullanicilaraMesajGonder("odayaGirdi:" + odaAdi + ":" + kullaniciAdi, this.soket);

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
                    else
                    {
                        Console.WriteLine("HATALI MESAJ: " + alinanString);
                        Thread.Sleep(100);
                    }
                }
            }
            catch (SocketException ex)
            {
                if (Sunucu.istemciListesi.ContainsValue(this))
                {
                    Sunucu.istemciListesi.Remove(kullaniciAdi);
                    if (bulunduguOda != null) { bulunduguOda.bagliIstemciler.Remove(this); }
                    Sunucu.tumKullanicilaraMesajGonder(("kcikis:" + kullaniciAdi));
                    Console.WriteLine(kullaniciAdi + " çıkış yaptı.");
                }

                soket.Close();
                Thread.CurrentThread.Abort();
            }
        }

        private void kullanicilariGonder()
        {
            string mesaj = "kullanicilar";
            foreach (var item in Sunucu.istemciListesi)
            {
                mesaj += ":" + item.Value.kullaniciAdi;
            }
            Console.WriteLine(mesaj);
            byte[] byteKullanicilar = Sunucu.uzunlukBaytlariniEkle(Sunucu.stringToByte(mesaj));

            soket.Send(byteKullanicilar, byteKullanicilar.Length, SocketFlags.None);
        }

        private void mesajGonder(Socket soket, string mesaj)
        {
            byte[] byteMesaj = Sunucu.uzunlukBaytlariniEkle(Sunucu.stringToByte(mesaj));
            soket.Send(byteMesaj, byteMesaj.Length, SocketFlags.None);
        }

        private void odalariGonder()
        {
            List<byte> byteDizisi = new List<byte>();
            byteDizisi.Add(6); //komutuzunlugu
            byteDizisi.AddRange(Sunucu.stringToByte("odalar")); //komut
            byteDizisi.Add((byte)Sunucu.odaListesi.Count); //oda sayısı

            foreach (var oda in Sunucu.odaListesi.Values)
            {

                byteDizisi.Add((byte)oda.odaAdi.Length); //oda adı uzunluğu
                byteDizisi.AddRange(Sunucu.stringToByte(oda.odaAdi)); //oda adı
                int sifreDurumu = oda.sifreVar ? 1 : 0; //sifre varsa 1 yoksa 0
                byteDizisi.Add((byte)sifreDurumu); //sifre var mı
                if (oda.sifreVar) { continue; }; //sifre varsa bağlı kullanıcıları yollama
                byteDizisi.Add((byte)oda.bagliIstemciler.Count); //odadaki kullanıcı sayısı

                foreach (var istemci in oda.bagliIstemciler)
                {
                    byteDizisi.Add((byte)istemci.kullaniciAdi.Length); //kullanıcı adı uzunluğu
                    byteDizisi.AddRange(Sunucu.stringToByte(istemci.kullaniciAdi)); //kullanıcı adı
                    byteDizisi.Add(istemci.sesDurumu); //kullanıcının durumu 0-aktif 1-mic muted 2-all muted
                }

            }

            byte[] byteOdaListesi = Sunucu.uzunlukBaytlariniEkle(byteDizisi.ToArray());
            soket.Send(byteOdaListesi, byteOdaListesi.Length, SocketFlags.None);
        }
    }
}
