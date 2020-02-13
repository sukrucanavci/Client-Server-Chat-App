using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Heyyo_Sunucu
{
    class Sunucu
    {
        private static Socket soket;
        public static SortedList<string, Istemci> istemciListesi = new SortedList<string, Istemci>();
        public static SortedList<string, Oda> odaListesi = new SortedList<string, Oda>();
        public static List<IPAddress> engelliIpListesi = new List<IPAddress>();

        static void Main(string[] args)
        {
            Thread threadSunucu = new Thread(ServerBaslat);
            threadSunucu.Start();

            while (true)
            {
                string komut = Console.ReadLine();
                string[] par = komut.Split(' ');

                switch (par[0])
                {
                    case "baslat":
                        threadSunucu.Start();
                        break;
                    case "ban":
                        kullaniciEngelle(par[1]);
                        istemciListesi[par[1]].soket.Close();
                        break;
                    case "mesaj":
                        KullanicilaraMesajGonder((char)Istemci.komutlar.sunucumesaji + komut);
                        break;
                    default:
                        Console.WriteLine("Geçersiz komut");
                        break;
                }

            }
        }

        static void ServerBaslat()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 5361);
            Socket dinleyiciSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            dinleyiciSoket.Bind(iep);
            dinleyiciSoket.Listen(256);
            Console.WriteLine("Server başlatıldı");
            Thread thread;
            while (true)
            {
                soket = dinleyiciSoket.Accept();
                if (ipKontrol(soket))
                {
                    Console.WriteLine(soket.RemoteEndPoint.ToString() + " bağlandı");
                    thread = new Thread(() => new Istemci(soket));
                    thread.Start();
                }
            }
        }

        public static bool ipKontrol(Socket soket)
        {
            if (engelliIpListesi.Contains(((IPEndPoint)soket.RemoteEndPoint).Address))
            {
                Console.WriteLine(soket.RemoteEndPoint + " engelli olduğu için bağlantısı engellendi!");
                return false;
            }
            return true;
        }

        public static void kullaniciEngelle(string kullaniciAdi)
        {
            IPAddress eip = IPAddress.Parse(((IPEndPoint)istemciListesi[kullaniciAdi].soket.RemoteEndPoint).Address.ToString());
            engelliIpListesi.Add(eip);
            Console.WriteLine(eip + " adresi banlandı!");
        }

        public static void KullanicilaraMesajGonder(string mesaj, params Istemci[] istisnaIstemciDizisi)
        {
            List<Istemci> istisnaIstemciListesi = new List<Istemci>(istisnaIstemciDizisi);
            byte[] byteMesaj = UzunlukBE(Encoding.UTF8.GetBytes(mesaj));
            foreach (var istemci in istemciListesi.Values)
            {
                if (istisnaIstemciListesi.Contains(istemci)) { continue; }
                istemci.soket.Send(byteMesaj, byteMesaj.Length, SocketFlags.None);
            }
        }

        public static void KullanicilaraMesajGonder(byte[] byteMesaj, params Istemci[] istisnaIstemciDizisi)
        {
            List<Istemci> istisnaIstemciListesi = new List<Istemci>(istisnaIstemciDizisi);
            byte[] bytes = UzunlukBE(byteMesaj);
            foreach (var istemci in istemciListesi.Values)
            {
                if (istisnaIstemciListesi.Contains(istemci)) { continue; }
                istemci.soket.Send(bytes, bytes.Length, SocketFlags.None);
            }
        }

        /// <summary>
        /// 2 byte'ı ushort'a çevirir
        /// </summary>
        public static ushort ByteToUshort(byte[] bytes)
        {
            byte[] uByte = { bytes[0], bytes[1] };
            return BitConverter.ToUInt16(uByte, 0);
        }

        /// <summary>
        /// Dizinin başına dizinin uzunluğunu 2 byte olarak ekler
        /// </summary>
        public static byte[] UzunlukBE(byte[] byteDizisi)
        {
            ushort uzunluk = (ushort)byteDizisi.Length;
            byte[] uBytes = BitConverter.GetBytes(uzunluk);
            List<byte> kopyaByteDizisi = new List<byte>();
            kopyaByteDizisi.Add(uBytes[0]);
            kopyaByteDizisi.Add(uBytes[1]);
            kopyaByteDizisi.AddRange(byteDizisi);
            byteDizisi = kopyaByteDizisi.ToArray();
            return byteDizisi;
        }

    }
}
