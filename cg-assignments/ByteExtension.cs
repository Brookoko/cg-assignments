namespace ImageConverter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ByteExtension
    {
        public static string ToHexString(this IEnumerable<byte> bytes)
        {
            return BitConverter.ToString(bytes.ToArray());
        }
        
        public static string ExtractString(this byte[] bytes, int index, int lenght)
        {
            return Encoding.UTF8.GetString(bytes.Skip(index).Take(lenght).ToArray());
        }

        public static int ExtractInt(this byte[] bytes, int index)
        {
            return (bytes[index] << 24) | (bytes[index + 1] << 16) | (bytes[index + 2] << 8) | bytes[index + 3];
        }
        
        public static int ReadBits(this byte[] bytes, int offset, int bits)
        {
            var result = 0;
            for (var i = offset; i < bits + offset; i++)
            {
                var index = i / 8;
                var b = bytes[index];
                var bitOffset = 7 - (i - index * 8);
                var bit = (b >> bitOffset) & 1;
                result = (result << 1) + bit;
            }
            return result;
        }
        
        public static int ReadBits(this byte b, int offset, int bits)
        {
            var result = 0;
            for (var i = offset; i < bits + offset; i++)
            {
                var bitOffset = 7 - i;
                var bit = (b >> bitOffset) & 1;
                result = (result << 1) + bit;
            }
            return result;
        }
        
        public static int ReadBitsReverse(byte[] bytes, int offset, int bits)
        {
            var result = 0;
            for (var i = offset; i < bits + offset; i++)
            {
                var index = bytes.Length - 1 - i / 8;
                var b = bytes[index];
                var bitOffset = (i - index * 8);
                var bit = (b >> bitOffset) & 1;
                result = (result << 1) + bit;
            }
            return result;
        }
        
        public static int ReadBitsReverse(byte b, int offset, int bits)
        {
            var result = 0;
            for (var i = offset; i < bits + offset; i++)
            {
                var bit = (b >> i) & 1;
                result = (result << 1) + bit;
            }
            return result;
        }
    }
}