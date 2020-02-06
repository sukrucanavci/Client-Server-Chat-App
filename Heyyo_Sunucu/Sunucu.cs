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
        static Socket soket;
        public static SortedList<string, Istemci> istemciListesi = new SortedList<string, Istemci>();
        public static SortedList<string, Oda> odaListesi = new SortedList<string, Oda>();
        public static List<IPAddress> engelliIpListesi = new List<IPAddress>();

        static void Main(string[] args)
        {
            Thread threadSunucu = new Thread(() => ServerBaslat());
            threadSunucu.Start();

            string komut = Console.ReadLine();
            try
            {
                string[] par = komut.Split(' ');

                switch (par[0])
                {
                    case "ban":
                        kullaniciEngelle(par[1]);
                        istemciListesi[par[1]].soket.Close();
                        break;
                    case "istemci":

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ServerBaslat()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 5361);
            Socket dinleyiciSoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            dinleyiciSoket.Bind(iep);
            dinleyiciSoket.Listen(5361);
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

        static void kullaniciEngelle(string kullaniciAdi)
        {
            IPAddress eip = IPAddress.Parse(((IPEndPoint)istemciListesi[kullaniciAdi].soket.RemoteEndPoint).Address.ToString());
            engelliIpListesi.Add(eip);
            Console.WriteLine(eip + " adresi banlandı!");
        }

        public static void tumKullanicilaraMesajGonder(string mesaj)
        {
            byte[] byteMesaj = stringToByte(mesaj);
            foreach (var item in Sunucu.istemciListesi.Values)
            {
                Socket istemciSoketi = (Socket)item.soket;
                soket.Send(byteMesaj, byteMesaj.Length, SocketFlags.None);
            }
        }

        public static byte[] stringToByte(string veri)
        {
            byte[] byteKomut = Encoding.UTF8.GetBytes(veri);
            short uzunluk = (short)byteKomut.Length;
            byte[] uBytes = BitConverter.GetBytes(uzunluk);

            List<byte> kopyaByteKomut = new List<byte>();
            kopyaByteKomut.Add(uBytes[0]);
            kopyaByteKomut.Add(uBytes[1]);
            kopyaByteKomut.AddRange(byteKomut);
            byteKomut = kopyaByteKomut.ToArray();
            return byteKomut;
        }


    }
}
