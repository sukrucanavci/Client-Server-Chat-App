using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Heyyo_Sunucu
{
    class Oda
    {
        public List<Istemci> bagliIstemciler = new List<Istemci>();
        public string odaAdi;
        private string odaSifresi;
        public bool sifreVar = true;

        enum cevap
        {
            evet,
            hayir
        }

        public Oda(byte[] bytes)
        {
            ushort index = 1;

            byte odaAdiUzunluk = bytes[index++];
            odaAdi = Encoding.UTF8.GetString(bytes, index, odaAdiUzunluk);

            if(Sunucu.odaListesi.ContainsKey(odaAdi)) { Thread.CurrentThread.Abort(); }

            index += odaAdiUzunluk;

            byte odaSifreUzunluk = bytes[index++];
            odaSifresi = Encoding.UTF8.GetString(bytes, index, odaSifreUzunluk);

            System.Console.WriteLine(odaAdi + " oda oluşturuldu");
            Sunucu.odaListesi.Add(odaAdi, this);

            if (odaSifreUzunluk == 0) { sifreVar = false; }
            byte sifreDurumu = sifreVar ? (byte)cevap.evet : (byte)cevap.hayir;

            List<byte> byteList = new List<byte>(bytes);
            byteList.RemoveRange(--index, odaSifreUzunluk + 1);
            byteList.Add(sifreDurumu);

            Sunucu.KullanicilaraMesajGonder(byteList.ToArray());
        }

        public void odayaKullaniciEkle(Istemci odayaGirmekIsteyenIstemci, string girilenSifre)
        {
            if (this == odayaGirmekIsteyenIstemci.bulunduguOda || odaSifresi != girilenSifre) { return; }

            List<byte> byteList = new List<byte>();

            byte[] kullaniciAdiByte = Encoding.UTF8.GetBytes(odayaGirmekIsteyenIstemci.kullaniciAdi);
            byteList.Add((byte)kullaniciAdiByte.Length);
            byteList.AddRange(kullaniciAdiByte);

            byte[] odaAdiByte = Encoding.UTF8.GetBytes(odaAdi);
            byteList.Add((byte)odaAdiByte.Length);
            byteList.AddRange(odaAdiByte);

            if (odayaGirmekIsteyenIstemci.bulunduguOda != null)
            {
                byteList.Insert(0, (byte)((char)Istemci.komutlar.kullaniciodadancikti));

                if (sifreVar)
                    foreach (var bagliIstemci in odayaGirmekIsteyenIstemci.bulunduguOda.bagliIstemciler)
                        bagliIstemci.soket.Send(Sunucu.UzunlukBE(byteList.ToArray()));
                else
                    Sunucu.KullanicilaraMesajGonder(byteList.ToArray());

                odayaGirmekIsteyenIstemci.bulunduguOda.odadanKullaniciCikar(odayaGirmekIsteyenIstemci);
                byteList.RemoveAt(0);
            }

            byteList.Insert(0, (byte)Istemci.komutlar.kullaniciodayagirdi);

            if (sifreVar)
            {
                foreach (var bagliIstemci in bagliIstemciler)
                    bagliIstemci.soket.Send(Sunucu.UzunlukBE(byteList.ToArray()));

                odayaGirmekIsteyenIstemci.soket.Send(Sunucu.UzunlukBE(byteList.ToArray())); 
            }
            else
                Sunucu.KullanicilaraMesajGonder(byteList.ToArray());

        }

        public void odadanKullaniciCikar(Istemci istemci)
        {
            istemci.bulunduguOda = null;
            bagliIstemciler.Remove(istemci);
        }
    }
}
