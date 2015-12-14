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
    class Send
    {
        static void Main(string[] args)
        {
            Converts Konv = new Converts();
            byte[] hexkey = Konv.HexStringToByteArray("06524C06");
            string idkey = Konv.ByteArrayToHexString(hexkey, hexkey.Length);
            Console.WriteLine("Identify HEX-Key: " + idkey);

            Console.Write("Ziel IP angeben: ");
            string empfang_ip = Console.ReadLine();
            //string empfang_ip = "192.168.192.104";
            Console.WriteLine("Ziel IP: " + empfang_ip);

            while (true)
            {
                Console.Write("Nachricht: ");
                string nachricht = Console.ReadLine();
                Ping icmp_send = new Ping();
                PingOptions options = new PingOptions();
                options.DontFragment = true;

                Encoder meinEncoder = Encoding.UTF8.GetEncoder();
                Char[] nacharray = nachricht.ToCharArray();
                int msgcount = meinEncoder.GetByteCount(nacharray, 0, nacharray.Length, true);
                Byte[] msg = new Byte[msgcount];
                meinEncoder.GetBytes(nacharray, 0, nacharray.Length, msg, 0, true);

                byte[] outg = hexkey.Concat(msg).ToArray();
                PingReply reply = icmp_send.Send(empfang_ip, 60 * 1000, outg, options);
                string responseReceived = Encoding.UTF8.GetString(reply.Buffer);
            }
        }
    }
}
