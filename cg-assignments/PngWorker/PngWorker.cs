namespace ImageConverter.Png
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PngWorker : IImageWorker
    {
        private static int headerSize = 8;
        private static int chunkType = 4;
        private static int chunkCrc = 4;
        private static int chunkSize = 4;
        
        private static string pngFormatHeader = "89-50-4E-47-0D-0A-1A-0A";
        
        private readonly Deflate deflate = new Deflate();
        private readonly IChunkConverter chunkConverter = new ChunkConverter();
        private readonly IByteFilterer byteFilterer = new ByteFilterer();
        
        public Image Decode(byte[] bytes)
        {
            var chunks = ExtractAllChunks(bytes, headerSize);
            var header = chunkConverter.Extract<Header>(chunks, ChunkType.Header);
            var zlib = chunkConverter.Extract<Zlib>(chunks, ChunkType.Data);
            var palette = chunkConverter.Extract<Palette>(chunks, ChunkType.Palette);
            
            CheckCompability(header, zlib);
            
            var decoded = deflate.Decode(zlib.data);
            var filtered = byteFilterer.Filter(decoded, header);
            var image = palette.Map(filtered);
            return image;
        }
        
        public bool CanWorkWith(byte[] bytes, string extension)
        {
            var format = bytes.Take(8).ToHexString();
            return format == pngFormatHeader && extension == "png";
        }
        
        private List<Chunk> ExtractAllChunks(byte[] bytes, int startIndex)
        {
            var chunks = new List<Chunk>();
            for (var i = startIndex; i < bytes.Length;)
            {
                var chunk = ExtractChunk(bytes, i, out i);
                chunks.Add(chunk);
            }
            return chunks;
        }
        
        private Chunk ExtractChunk(byte[] bytes, int index, out int i)
        {
            var size = bytes.ExtractInt(index);
            i = index + chunkSize + chunkType + size + chunkCrc;
            var typeString = bytes.ExtractString(index + chunkSize, chunkType);
            return new Chunk()
            {
                size = size,
                type = ChunkExtension.FromHeader(typeString),
                data = bytes.Skip(index + 8).Take(size).ToArray(),
                crc = bytes.ExtractInt(i - chunkCrc),
            };
        }
        
        private void CheckCompability(Header header, Zlib zlib)
        {
            if (header.colorType != ColorType.Indexed)
            {
                throw new Exception();
            }
            if (zlib.compression != 8 || zlib.windowSize != 7 || zlib.hasDictionary)
            {
                throw new Exception();
            }
        }
        
        public byte[] Encode(Image image)
        {
            throw new NotImplementedException();
        }
    }
}