using CRC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CRC;

namespace VietQR_QRCodeGeneration.Ultil
{
    public class Function
    {
        public enum ResultCode
        {
            Success = 200,
            Error = 400,
            Exception = 21,
            Timeout = 440,
            NotFound = 404,
            Unauthorized = 401
        }

        public static string MD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string prior to .NET 5
                StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static string CRC16Trans(string input)
        {
            byte[] inputByte = Encoding.UTF8.GetBytes(input);
            var crc = ComputeCRC16(inputByte, 0x11021, 0xFFFF, 0x0000, false);
            return crc.ToString("X4");
        }

        static ushort ComputeCRC16(byte[] data, int polynomial, int initialCrc, int xorOut, bool reverse)
        {
            int crc = initialCrc;

            foreach (byte b in data)
            {
                crc ^= (ushort)(b << 8);

                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0)
                    {
                        crc = (ushort)((crc << 1) ^ polynomial);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }

            if (reverse)
            {
                crc = ReverseBits(crc);
            }

            return (ushort)(crc ^ xorOut);
        }

        static int ReverseBits(int value)
        {
            ushort result = 0;

            for (int i = 0; i < 16; i++)
            {
                result = (ushort)((result << 1) | (value & 1));
                value >>= 1;
            }

            return result;
        }
    }
}