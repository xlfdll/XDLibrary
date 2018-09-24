using System;
using System.Collections.Generic;
using System.Text;

namespace Xlfdll.Collections
{
	public static class ByteArrayOperations
    {
        public static String ToHexString(this IEnumerable<Byte> bytes, Boolean upperCase)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Byte b in bytes)
            {
                sb.Append(b.ToString(upperCase ? "X2" : "x2"));
            }

            return sb.ToString();
        }

        public static Byte[] ToByteArray(String hexString)
        {
            Byte[] bytes = new Byte[hexString.Length / 2];

            for (Int32 i = 0; i < hexString.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}