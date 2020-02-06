using System.Collections.Generic;

namespace Heyyo_Sunucu
{
    class Oda
    {
        List<Istemci> bagliIstemciler = new List<Istemci>();
        public string odaAdi;
        string odaSifresi;
        Istemci odaSahibiIstemci;

        public Oda(Istemci odaSahibiIstemci, string komut)
        {
            string[] par = komut.Split(':');

            this.odaAdi = par[1];
            this.odaSifresi = par[2];
            this.odaSahibiIstemci = odaSahibiIstemci;

            Sunucu.odaListesi.Add(odaAdi, this);

            string sifreDurumu = odaSifresi != "" ? "sifreli" : "sifresiz";
            odaSahibiIstemci.tumKullanicilaraMesajGonder(par[0] + ":" + par[1] + ":" + sifreDurumu, false);
        }

        public bool odayaKullaniciEkle(Istemci istemci, string girilenSifre)
        {
            if (this != istemci.bulunduguOda && odaSifresi == girilenSifre)
            {
                if (odaSahibiIstemci.bulunduguOda != null)
                {
                    Sunucu.tumKullanicilaraMesajGonder("odadanCikti:" + istemci.bulunduguOda.odaAdi + ":" + istemci.kullaniciAdi);
                    istemci.bulunduguOda.bagliIstemciler.Remove(istemci);
                }
                bagliIstemciler.Add(istemci);
                odaSahibiIstemci.bulunduguOda = this;
                return true;
            }
            else
            {
                return false;
            }

        }

        public void odadanKullaniciCikar(Istemci istemci)
        {
            bagliIstemciler.Remove(istemci);
        }
    }
}
