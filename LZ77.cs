namespace ImageConverter
{
    using System.Collections.Generic;
    using System.Linq;

    public class LZ77
    {
        private List<byte> buffer = new List<byte>();
        private int pos;
        
        public byte[] Compress(byte[] data)
        {
            var result = new List<byte>();
            buffer = new List<byte>();
            pos = 0;
            while (pos < data.Length)
            {
                var (offset, length) = FindMatching(data);
                for (var i = 0; i < length + 1; i++)
                {
                    buffer.Add(data[pos + i]);
                }
                pos += length;
                result.Add((byte) offset);
                result.Add((byte) length);
                result.Add(data[pos]);
                pos++;
            }
            return result.ToArray();
        }
        
        private (int offset, int length) FindMatching(byte[] data)
        {
            var length = 0;
            var offset = 0;
            var hasMatch = false;
            while (pos + length < data.Length)
            {
                length++;
                var sub = data.Skip(pos).Take(length).ToArray();
                if (!HasMatch(sub, ref offset))
                {
                    break;
                }
                hasMatch = true;
                buffer.Add(data[pos + length - 1]);
            }
            length = hasMatch ? length - 1 : 0;
            buffer = buffer.Take(pos).ToList();
            return (offset, length);
        }

        private bool HasMatch(byte[] sub, ref int offset)
        {
            for (var i = 0; i < buffer.Count - (sub.Length - 1); i++)
            {
                var bufferSub = buffer.Skip(i).Take(sub.Length).ToArray();
                if (Compare(sub, bufferSub))
                {
                    offset = pos - i;
                    return true;
                }
            }
            return false;
        }
        
        private bool Compare(byte[] a, byte[] b)
        {
            for (var i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }
        
        public byte[] Decompress(byte[] data)
        {
            var result = new List<byte>();
            var p = 0;
            for (var i = 0; i < data.Length; i += 3)
            {
                var offset = data[i];
                var length = data[i + 1];
                var next = data[i + 2];
                for (var j = p - offset; j < p - offset + length; j++)
                {
                    var b = buffer[j];
                    result.Add(b);
                }
                result.Add(next);
                p += length + 1;
            }
            return result.ToArray();
        }
    }
}