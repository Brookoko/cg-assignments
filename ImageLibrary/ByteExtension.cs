namespace ImageConverter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ByteExtension
    {
        public static byte[] ToBytes(this string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        public static byte[] FromHexString(this string s)
        {
            return Enumerable.Range(0, s.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(s.Substring(x, 2), 16))
                .ToArray();
        }

        public static int HexStringLength(this string s)
        {
            return s.Length / 2;
        }

        public static string ToHexString(this IEnumerable<byte> bytes)
        {
            return BitConverter.ToString(bytes.ToArray()).Replace("-", "");
        }

        public static string ExtractString(this byte[] bytes, int index, int lenght)
        {
            var sub = bytes.Skip(index).Take(lenght).ToArray();
            return Encoding.UTF8.GetString(sub);
        }

        public static int ExtractInt(this byte[] bytes, int index)
        {
            return (bytes[index] << 24) | (bytes[index + 1] << 16) | (bytes[index + 2] << 8) | bytes[index + 3];
        }

        public static byte[] ToBytes(this int i)
        {
            var bytes = BitConverter.GetBytes(i);
            return BitConverter.IsLittleEndian ? bytes.Reverse().ToArray() : bytes;
        }

        public static byte[] ToBytes(this int i, int resultLength)
        {
            var source = i.ToBytes();
            var resultBytes = new byte[resultLength];

            for (int resultIndex = resultLength - 1, sourceIndex = source.Length - 1;
                sourceIndex >= 0; sourceIndex--, resultIndex--)
            {
                if (resultIndex >= 0)
                {
                    resultBytes[resultIndex] = source[sourceIndex];
                }
                else if (source[sourceIndex] != 0)
                {
                    
                    throw new DataConversionException("Loss of data while converting bytes stream");
                }
            }

            return resultBytes;
        }
    }
}