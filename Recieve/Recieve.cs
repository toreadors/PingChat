using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;


namespace PingChat
{
    class Recieve
    {
        static void Main(string[] args)
        {

            Converts Konv = new Converts();

            // ID Key in Hex umwandeln (eigentlich unnoetig, da der String spaeter verglichen wird
            byte[] hexkey = Konv.HexStringToByteArray("06524c06");
            string idkey = Konv.ByteArrayToHexString(hexkey, hexkey.Length);
            Console.WriteLine("Identify HEX-Key: " + idkey);
            // Lokale IP Adresse ermitteln
            IPHostEntry myifaces = Dns.GetHostByName(Dns.GetHostName());
            IPAddress myIp = myifaces.AddressList[0];
            Console.WriteLine("Local IP Address: " + myIp);
            while (true)
            {
                // Raw Socket aufmachen, ICMP Protokoll, alles Empfangen an myIp
                Socket icmp_recieve = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
                icmp_recieve.Bind(new IPEndPoint(myIp, 0));
                icmp_recieve.IOControl(IOControlCode.ReceiveAll, new byte[] { 1, 0, 0, 0 }, null);

                byte[] buffer = new byte[1024];
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // 32 Bytes wieder entfernen, da wir erst ab Byte 32 anfangen zu lesen.
                int bytesRead = icmp_recieve.ReceiveFrom(buffer, ref remoteEndPoint) - 32;

                // Dest und Source IP ermitteln.
                Byte[] ipsource = new Byte[4];
                Array.Copy(buffer,12 ,ipsource, 0, 4);
                IPAddress ips = new IPAddress(ipsource);

                Byte[] ipdest = new Byte[4];
                Array.Copy(buffer, 12, ipdest, 0, 4);
                IPAddress ipd = new IPAddress(ipdest);
                
                string icmptyp = Encoding.UTF8.GetString(buffer, 20, 1);
                string icmpcode = Encoding.UTF8.GetString(buffer, 21, 1);
                string empfangeneridkey = Encoding.UTF8.GetString(buffer, 28, 4);
                string receivedMsg = Encoding.UTF8.GetString(buffer, 32, bytesRead);

                empfangeneridkey = Konv.StringToHex(empfangeneridkey);
                // Methode fuellt nicht mit Nullen auf, daher ID Key ohne Nullen
                if (empfangeneridkey == "06524c06")
                {
                    Console.WriteLine(DateTime.Now.ToString() + ": Sender " + remoteEndPoint + ": " + receivedMsg);
                    Console.WriteLine("Länge Payload: " + receivedMsg.Length);
                    Console.WriteLine("Hex-String ID-Key: " + empfangeneridkey);
                    Console.WriteLine("IP Source: " + ips);
                    Console.WriteLine("IP Destination: " + ipd);
                    Console.WriteLine("ICMP Typ: " + Konv.StringToHex(icmptyp));
                    Console.WriteLine("ICMP Code: " + Konv.StringToHex(icmpcode));
                    Console.WriteLine("----------------");
                }
            }
        }
    }
}
