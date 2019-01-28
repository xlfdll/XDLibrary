using System;

namespace Xlfdll
{
    public static class IntegerConversions
    {
        public static String ToPaddedHexString(this UInt64 n, Boolean includePrefix)
        {
            return (includePrefix ? "0x" : String.Empty) + n.ToString("X16");
        }

        public static String ToPaddedHexString(this UInt32 n, Boolean includePrefix)
        {
            return (includePrefix ? "0x" : String.Empty) + n.ToString("X8");
        }

        public static String ToPaddedHexString(this UInt16 n, Boolean includePrefix)
        {
            return (includePrefix ? "0x" : String.Empty) + n.ToString("X4");
        }

        public static String ToPaddedHexString(this Byte n, Boolean includePrefix)
        {
            return (includePrefix ? "0x" : String.Empty) + n.ToString("X2");
        }

        public static UInt64 ConvertHexStringToUInt64(String hexString)
        {
            return Convert.ToUInt64(hexString, 16);
        }

        public static UInt32 ConvertHexStringToUInt32(String hexString)
        {
            return Convert.ToUInt32(hexString, 16);
        }

        public static UInt16 ConvertHexStringToUInt16(String hexString)
        {
            return Convert.ToUInt16(hexString, 16);
        }

        public static Byte ConvertHexStringToByte(String hexString)
        {
            return Convert.ToByte(hexString, 16);
        }
    }
}