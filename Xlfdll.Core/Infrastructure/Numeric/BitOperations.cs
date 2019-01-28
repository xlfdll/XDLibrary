using System;

namespace Xlfdll
{
    public static class BitOperations
    {
        public static UInt16 GetBit(this UInt16 n, Int32 index)
        {
            return (UInt16)GetBit((UInt32)n, index);
        }

        public static UInt16 SetBit(this UInt16 n, Int32 index)
        {
            return (UInt16)SetBit((UInt32)n, index);
        }

        public static UInt16 ClearBit(this UInt16 n, Int32 index)
        {
            return (UInt16)ClearBit((UInt32)n, index);
        }

        public static UInt32 GetBit(this UInt32 n, Int32 index)
        {
            return (n >> index) & 1U;
        }

        public static UInt32 SetBit(this UInt32 n, Int32 index)
        {
            return n | (1U << index);
        }

        public static UInt32 ClearBit(this UInt32 n, Int32 index)
        {
            return n & ~(1U << index);
        }

        public static UInt64 GetBit(this UInt64 n, Int32 index)
        {
            return (n >> index) & 1UL;
        }

        public static UInt64 SetBit(this UInt64 n, Int32 index)
        {
            return n | (1UL << index);
        }

        public static UInt64 ClearBit(this UInt64 n, Int32 index)
        {
            return n & ~(1UL << index);
        }
    }
}