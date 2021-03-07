namespace ImageConverter
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    public class Deflate
    {
        public byte[] Encode(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var res = new MemoryStream())
                {
                    using (var deflate = new DeflateStream(res, CompressionMode.Compress))
                    {
                        ms.CopyTo(deflate);
                        deflate.Close();
                        return res.ToArray();
                    }
                }
            }
        }
        
        public byte[] Decode(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var res = new MemoryStream())
                {
                    using (var deflate = new DeflateStream(ms, CompressionMode.Decompress))
                    {
                        deflate.CopyTo(res);
                        return res.ToArray();
                    }
                }
            }
            for (var i = 0; i < data.Length; i += 4)
            {
                var chunk = data.Skip(i).Take(4).ToArray();
                DecodeChunk(chunk);
            }
            return data;
        }
        
        private byte[] DecodeChunk(byte[] chunk)
        {
            var isEnd = ByteExtension.ReadBits(chunk, 0, 1);
            var compression = ByteExtension.ReadBits(chunk, 1, 2);
            var hlit = ByteExtension.ReadBits(chunk, 3, 5) + 257;
            var hdist = ByteExtension.ReadBits(chunk, 8, 5) + 1;
            var hclen = ByteExtension.ReadBits(chunk, 13, 4) + 4;
            var codes = new [] {16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15};
            var lengths = new int [19];

            for (var i = 0; i < hclen; i++)
            {
                lengths[codes[i]] = ByteExtension.ReadBits(chunk, 17 + i * 3, 3);
            }
            
            Console.WriteLine($"{isEnd}:{compression}");
            var tree = BuildHuffmanCode(lengths, 19);

            return chunk;
        }


        private int GetMaxBitLength(int[] codeBitLengths)
        {
            return codeBitLengths.Max();
        }
        
        private int[] GetBitCount(int[] codeBitLength, int max, int length)
        {
            var codeCounts = new int[max + 1];
            for (var i = 0; i < length; i++)
            {
                codeCounts[codeBitLength[i]]++;
            }
            codeCounts[0] = 0;
            return codeCounts;
        }

        private int[] BuildHuffmanCode(int[] codeBitLengths, int length)
        {
            var max = GetMaxBitLength(codeBitLengths);
            var codeCounts = GetBitCount(codeBitLengths, max, length);
            var codes = CodeForBitLen(codeCounts, max);
            return AssignCodes(codes, codeBitLengths, length);
        }
        
        private int[] CodeForBitLen(int[] codeCount, int max)
        {
            var code = 0;
            var codes = new int[max + 1];
            for (var i = 0; i <= max; i++)
            {
                code = (code + codeCount[i]) << 1;
                if (codeCount[i] > 0) codes[i] = code;
            }
            return codes;
        }
        
        private int[] AssignCodes(int[] codes, int[] codeBitLength, int length)
        {
            var assignedCodes = new int[length];
            for (var i = 0; i < length; i++)
            {
                var bit = codeBitLength[i];
                if (bit > 0)
                {
                    assignedCodes[i] = codes[bit]++;
                }
            }
            return assignedCodes;
        }
    }
}