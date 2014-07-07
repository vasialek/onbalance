using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.Core
{

    public class Common
    {

        public static byte[] CalculateMd5(string p)
        {
            if (string.IsNullOrEmpty(p))
	        {
		        throw new ArgumentNullException("Could not compute MD5 of empty string");
	        }
            byte[] ba = Encoding.UTF8.GetBytes(p);
            return System.Security.Cryptography.MD5CryptoServiceProvider.Create().ComputeHash(ba, 0, ba.Length);
        }

        #region Hex Encode/Decode
        private static char[] _hex = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        /// <summary>
        /// Encode hex
        /// </summary>
        /// <param name="ba">ByteArray to encode</param>
        /// <returns>Hex encoded data</returns>
        public static string EncodeHex(byte[] ba)
        {
            if(ba == null) return null;
            char[] ca = new char[ba.Length << 1];
            for(int ck = 0; ck < ba.Length; ck++)
            {
                byte b = ba[ck];
                ca[(ck << 1)] = _hex[b >> 4];
                ca[(ck << 1) + 1] = _hex[b & 0x0F];
            }
            return new string(ca);
        }

        public static string EncodeHex(byte[] ba, int offset, int length)
        {
            if(ba == null) return null;
            char[] ca = new char[length << 1];
            //char[] ca = new char[ba.Length << 1];
            //for (int ck = offset; ck < offset + length; ck++)
            for(int ck = 0; ck < length; ck++)
            {
                byte b = ba[ck + offset];
                ca[(ck << 1)] = _hex[b >> 4];
                ca[(ck << 1) + 1] = _hex[b & 0x0F];
            }
            return new string(ca);
        }
        /// <summary>
        /// Decode hex
        /// </summary>
        /// <param name="s">Hex encoded data</param>
        /// <returns>Decoded ByteArray</returns>
        public static byte[] DecodeHex(string s)
        {
            if(s == null) return null;
            byte[] ba = new byte[s.Length >> 1];
            int ck = 0;
            for(int i = 0; i < ba.Length; i++)
            {
                switch(s[ck++])
                {
                    case '0': break;
                    case '1': ba[i] = 0x10; break;
                    case '2': ba[i] = 0x20; break;
                    case '3': ba[i] = 0x30; break;
                    case '4': ba[i] = 0x40; break;
                    case '5': ba[i] = 0x50; break;
                    case '6': ba[i] = 0x60; break;
                    case '7': ba[i] = 0x70; break;
                    case '8': ba[i] = 0x80; break;
                    case '9': ba[i] = 0x90; break;
                    case 'A':
                    case 'a': ba[i] = 0xA0; break;
                    case 'B':
                    case 'b': ba[i] = 0xB0; break;
                    case 'C':
                    case 'c': ba[i] = 0xC0; break;
                    case 'D':
                    case 'd': ba[i] = 0xD0; break;
                    case 'E':
                    case 'e': ba[i] = 0xE0; break;
                    case 'F':
                    case 'f': ba[i] = 0xF0; break;
                    default: throw new ArgumentException("String is not hex encoded data.");
                }
                switch(s[ck++])
                {
                    case '0': break;
                    case '1': ba[i] |= 0x01; break;
                    case '2': ba[i] |= 0x02; break;
                    case '3': ba[i] |= 0x03; break;
                    case '4': ba[i] |= 0x04; break;
                    case '5': ba[i] |= 0x05; break;
                    case '6': ba[i] |= 0x06; break;
                    case '7': ba[i] |= 0x07; break;
                    case '8': ba[i] |= 0x08; break;
                    case '9': ba[i] |= 0x09; break;
                    case 'A':
                    case 'a': ba[i] |= 0x0A; break;
                    case 'B':
                    case 'b': ba[i] |= 0x0B; break;
                    case 'C':
                    case 'c': ba[i] |= 0x0C; break;
                    case 'D':
                    case 'd': ba[i] |= 0x0D; break;
                    case 'E':
                    case 'e': ba[i] |= 0x0E; break;
                    case 'F':
                    case 'f': ba[i] |= 0x0F; break;
                    default: throw new ArgumentException("String is not hex encoded data.");
                }
            }
            return ba;
        }
        #endregion
    }

}
