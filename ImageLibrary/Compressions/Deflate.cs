namespace ImageConverter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;

    public class Deflate
    {
        private static byte[] Flags => new byte[] {0x78, 0x9C};
        private static readonly byte[] Order = {16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15};
        
        private readonly Action<BitStream, List<byte>>[] decodeFunctions = new Action<BitStream, List<byte>>[4];
        
        private readonly LZ77 lz = new LZ77();
        
        public Deflate()
        {
            decodeFunctions[0] = DecodeUncompressed;
            decodeFunctions[1] = DecodeStatic;
            decodeFunctions[2] = DecodeDynamic;
            decodeFunctions[3] = ThrowOnReservedValue;
        }
        
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
                        return Flags.Concat(res.ToArray()).ToArray();
                    }
                }
            }
        }
        
        public byte[] Decode(byte[] data)
        {
            var isFinal = false;
            var decoded = new List<byte>();
            var stream = new BitStream();
            stream.Write(data);
            stream.Reverse();
            do
            {
                isFinal = stream.Read(1) == 1;
                var compression = stream.ReadReverse(2);
                var function = decodeFunctions[compression];
                function(stream, decoded);
            } while (!isFinal);
            return decoded.ToArray();
        }
        
        private void DecodeUncompressed(BitStream stream, List<byte> bytes)
        {
            stream.ReadToByteBoundary();
            var len = stream.ReadReverse(2);
            var nlen = stream.ReadReverse(2) ^ 0xffff;
            if (len != nlen)
            {
                throw new CompressionException("Invalid block lenght");
            }
            var block = stream.ReadReverse(len * 8);
            var data = block.ToBytes()
                .Reverse().Take(len)
                .Reverse().ToArray();
            bytes.AddRange(data);
        }
        
        private void DecodeStatic(BitStream stream, List<byte> bytes)
        {
            lz.Decompress(stream, Huffman.Literal, Huffman.Distance, bytes);
        }
        
        private void DecodeDynamic(BitStream stream, List<byte> bytes)
        {
            var hlit = stream.ReadReverse(5) + 257;
            var hdist = stream.ReadReverse(5) + 1;
            var hclen = stream.ReadReverse(4) + 4;
            var lengths = new int [Order.Length];

            for (var i = 0; i < hclen; i++)
            {
                lengths[Order[i]] = stream.ReadReverse(3);
            }
            
            var tree = new Huffman();
            tree.Build(lengths);
            
            var decoded = DecodeDictionary(tree, stream, hlit + hdist);
            
            var literalTree = new Huffman();
            literalTree.Build(decoded.Take(hlit).ToArray());
            var distanceTree = new Huffman();
            distanceTree.Build(decoded.Skip(hlit).Take(hdist).ToArray());

            lz.Decompress(stream, literalTree, distanceTree, bytes);
        }
        
        private int[] DecodeDictionary(Huffman tree, BitStream stream, int length)
        {
            var decoded = new int[length];
            var index = 0;
            while (index < length)
            {
                var code = tree.Decode(stream);
                if (code < 16)
                {
                    decoded[index] = code;
                    index++;
                    continue;
                }
                
                var repeat = 0;
                var codeLenght = 0;
                switch (code)
                {
                    case 16:
                        repeat = stream.ReadReverse(2) + 3;
                        codeLenght = decoded[index - 1];
                        break;
                    case 17:
                        repeat = stream.ReadReverse(3) + 3;
                        break;
                    case 18:
                        repeat = stream.ReadReverse(7) + 11;
                        break;
                }
                
                for (var i = 0; i < repeat; i++)
                {
                    decoded[index + i] = codeLenght;
                }
                index += repeat;
            }
            return decoded;
        }
        
        private void ThrowOnReservedValue(BitStream stream, List<byte> bytes)
        {
            throw new ImageDecodingException("Failed to decode. Reserved value");
        }
    }
}