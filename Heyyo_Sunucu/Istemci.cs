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
            MesajAlma(soket);
        }
        
        void MesajAlma(Socket soket)
        {

            try
            {
                while (true)
                {
                    byte[] byteMesajUzunlugu = new byte[2];
                    soket.Receive(byteMesajUzunlugu, 2, SocketFlags.None);

                    byte[] alinanbytes = new byte[byteToShort(byteMesajUzunlugu)];
                    soket.Receive(alinanbytes, alinanbytes.Length, SocketFlags.None);

                    string mesaj = byteToString(alinanbytes);
                    Console.WriteLine(soket.RemoteEndPoint.ToString() + " -> " + mesaj);

                    string[] par = mesaj.Split(':');

                    if (mesaj.StartsWith("gMesaj"))
                    {
                        tumKullanicilaraMesajGonder(mesaj, false);
                    }
                    else if (mesaj.StartsWith("oMesaj"))
                    {
                        string alici = mesaj.Split(':')[2];

                        Socket aliciSoket = default;
                        foreach (var item in Sunucu.istemciListesi)
                        {
                            if (item.Value.kullaniciAdi == alici)
                            {
                                aliciSoket = item.Value.soket;
                                break;
                            }
                        }

                        mesajGonder(aliciSoket, mesaj);
                        mesajGonder(soket, mesaj);
                    }
                    else if (mesaj.StartsWith("nickname"))
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
                    else if (mesaj.StartsWith("odaEkle"))
                    {
                        if (!Sunucu.odaListesi.ContainsKey(mesaj.Split(':')[1]))
                        {
                            tOdaEkle = new Thread(() => oda = new Oda(this, mesaj));
                            tOdaEkle.Start();
                        }     
                    }
                    else if (mesaj.StartsWith("odayaGiris"))
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
                    else if (mesaj.StartsWith("odadanCikis"))
                    {

                    }
                    else
                    {
                        Console.WriteLine("HATALI MESAJ istemci.cs " + alinanbytes.Length + " " + byteToShort(byteMesajUzunlugu));

                        soket.Send(stringToByte("sg"), 4, SocketFlags.None);
                        //throw new Exception("123");
                    }

                }
            }
            catch (Exception ex)
            {
                
                Sunucu.istemciListesi.Remove(kullaniciAdi);
                tumKullanicilaraMesajGonder(("kcikis:" + kullaniciAdi), false);
                Console.WriteLine(kullaniciAdi + " çıkış yaptı.");
                soket.Close();
                Console.WriteLine(ex.Message);
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
