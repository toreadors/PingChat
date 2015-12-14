using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PingChat
{
    public class Converts
    {
        //Wandelt einen einen Hex String in ein byte Array um 
        public byte[] HexStringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        //Wandelt ein Byte Array in einen Hex Wert um.
        public string ByteArrayToHexString(byte[] ba, int length)
        {
            length++;
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
                length--;
                if (length == 0) break;
            }
            return hex.ToString();
        }


        // Wandelt einen String in einen Hex String 
        public string StringToHex(string hexstring)
        {
            var sb = new StringBuilder();
            foreach (char t in hexstring)
                sb.Append(Convert.ToInt32(t).ToString("x2"));
            return sb.ToString();
        }
    }
}
