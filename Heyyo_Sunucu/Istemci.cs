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

                    short gelecekMesajinUzunlugu = byteToShort(byteMesajUzunlugu);
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

                    string alinanString = byteToString(alinanbytes);
                    Console.WriteLine("Alındı -> " + soket.RemoteEndPoint.ToString() + " -> " + alinanString);

                    string[] par = alinanString.Split(':');

                    if (alinanString.StartsWith("gMesaj"))
                    {
                        string gonderilecekMesaj = alinanString.Substring(7);
                        string gondereniEklenmisMesaj = par[0] + ":" + kullaniciAdi + ":" + gonderilecekMesaj;
                        tumKullanicilaraMesajGonder(gondereniEklenmisMesaj, false);
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
                            tumKullanicilaraMesajGonder(("kgiris:" + kullaniciAdi), true);
                            kullanicilariGonder();
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
                            tOdaEkle = new Thread(() => oda = new Oda(this, alinanString));
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
                                    tumKullanicilaraMesajGonder("odayaGirdi:" + odaAdi + ":" + kullaniciAdi, true);

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
                        Thread.Sleep(1000);
                    }
                }
            }
            catch (SocketException ex)
            {
                
                Sunucu.istemciListesi.Remove(kullaniciAdi);
                tumKullanicilaraMesajGonder(("kcikis:" + kullaniciAdi), false);
                Console.WriteLine(kullaniciAdi + " çıkış yaptı.");
                soket.Close();
                Console.WriteLine(ex.Message);
                //Thread.CurrentThread.Abort();
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
            byte[] byteKullanicilar = stringToByte(mesaj);

            soket.Send(byteKullanicilar, byteKullanicilar.Length, SocketFlags.None);
        }

        private byte[] stringToByte(string mesaj)
        {
            return Sunucu.stringToByte(mesaj);
        }

        private void mesajGonder(Socket soket, string mesaj)
        {
            byte[] byteMesaj = stringToByte(mesaj);
            soket.Send(byteMesaj, byteMesaj.Length, SocketFlags.None);
        }

        private short byteToShort(byte[] bytes)
        {
            byte[] uByte = { bytes[0], bytes[1] };
            return BitConverter.ToInt16(uByte, 0);
        }

        private string byteToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public void tumKullanicilaraMesajGonder(string mesaj, bool buIstemciHaricMi)
        {
            byte[] byteMesaj = stringToByte(mesaj);
            foreach (var item in Sunucu.istemciListesi.Values)
            {
                Socket istemciSoketi = item.soket;

                if (istemciSoketi == soket && buIstemciHaricMi){ continue; }
                istemciSoketi.Send(byteMesaj, byteMesaj.Length, SocketFlags.None);
            }
        }

    }
}
