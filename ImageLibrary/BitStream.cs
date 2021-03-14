namespace ImageConverter
{
    using System.Collections.Generic;

    public class BitStream
    {
        public bool IsEnded => bytes.Count * 8 == offset;
        
        private readonly List<byte> bytes = new List<byte>();
        private int offset;
        
        public void Write(byte data)
        {
            bytes.Add(data);
        }
        
        public void Write(byte[] data)
        {
            bytes.AddRange(data);
        }

        public void Reverse()
        {
            var reversedBytes = new List<byte>();
            foreach (var b in bytes)
            {
                var a = 0;
                for (var i = 0; i < 8; i++)
                {
                    if ((b & (1 << i)) != 0)
                    {
                        a |= 1 << (7- i);
                    }
                }
                reversedBytes.Add((byte)a);
            }
            bytes.Clear();
            bytes.AddRange(reversedBytes);
        }
        
        public int Read(int bits)
        {
            var result = 0;
            for (var i = offset; i < bits + offset; i++)
            {
                var index = i / 8;
                var b = bytes[index];
                var bitOffset = 7 - (i - index * 8);
                var bit = (b >> bitOffset) & 1;
                result = (result << 1) | bit;
            }
            offset += bits;
            return result;
        }
        
        public int ReadReverse(int bits)
        {
            var result = 0;
            for (var i = offset; i < bits + offset; i++)
            {
                var index = i / 8;
                var b = bytes[index];
                var bitOffset = 7 - (i - index * 8);
                var bit = (b >> bitOffset) & 1;
                result |= bit << (i - offset);
            }
            offset += bits;
            return result;
        }
        
        public void ReadToByteBoundary()
        {
            if (offset % 8 != 0)
            {
                var lower = offset / 8;
                offset = (lower + 1) * 8;
            }
        }
        
        public void MoveBack(int bits)
        {
            offset -= bits;
        }
    }
}