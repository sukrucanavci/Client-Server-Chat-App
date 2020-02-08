using System.Collections.Generic;

namespace Heyyo_Sunucu
{
    class Oda
    {
        public List<Istemci> bagliIstemciler = new List<Istemci>();
        public string odaAdi;
        string odaSifresi;
        public bool sifreVar;

        public Oda(string komut)
        {
            string[] par = komut.Split(':');

            this.odaAdi = par[1];
            this.odaSifresi = par[2];
            System.Console.WriteLine(odaAdi + " oda oluşturuldu");
            Sunucu.odaListesi.Add(odaAdi, this);

            sifreVar = odaSifresi != "" ? true : false;
            string sifreDurumu = odaSifresi != "" ? "sifreli" : "sifresiz";
            Sunucu.kullanicilaraMesajGonder(par[0] + ":" + par[1] + ":" + sifreDurumu);
        }

        public bool odayaKullaniciEkle(Istemci istemci, string girilenSifre)
        {
            if (this != istemci.bulunduguOda && odaSifresi == girilenSifre)
            {
                if (istemci.bulunduguOda != null)
                {
                    Sunucu.kullanicilaraMesajGonder("odadanCikti:" + istemci.bulunduguOda.odaAdi + ":" + istemci.kullaniciAdi);
                    istemci.bulunduguOda.bagliIstemciler.Remove(istemci);
                }
                bagliIstemciler.Add(istemci);
                istemci.bulunduguOda = this;
                return true;
            }
            else
            {
                return false;
            }

        }

        public void odadanKullaniciCikar(Istemci istemci)
        {
            istemci.bulunduguOda = null;
            bagliIstemciler.Remove(istemci);
        }
    }
}
