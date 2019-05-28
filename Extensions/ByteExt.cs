using System;
using System.Collections.Generic;
using System.Text;

namespace UnitSense.Extensions.Extensions
{
    public static class ByteExt
    {
        public static string ToHexString(this byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);

            for (int i = 0; i < ba.Length; i++) // <-- use for loop is faster than foreach   
                hex.Append(ba[i].ToString("X2")); // <-- ToString is faster than AppendFormat   

            return hex.ToString();
        }

        public static byte[] ToByteArray(this string s)
        {
            byte[] bytes = new byte[s.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int hi = s[i * 2] - 65;
                hi = hi + 10 + ((hi >> 31) & 7);

                int lo = s[i * 2 + 1] - 65;
                lo = lo + 10 + ((lo >> 31) & 7) & 0x0f;

                bytes[i] = (byte) (lo | hi << 4);
            }

            return bytes;
        }

        public static byte[] Randomize(int size)
        {
            List<byte> builder = new List<byte>();
            Random random = new Random(Environment.TickCount);
            byte b;

            for (int i = 0; i < size; i++)
            {
                b = Convert.ToByte(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Add(b);
            }

            return builder.ToArray();
        }
    }
}