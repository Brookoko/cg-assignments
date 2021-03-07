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
        
        private static string pngFormatHeader = "89504E470D0A1A0A";
        
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
            var chunk = new Chunk()
            {
                size = size,
                type = ChunkExtension.FromHeader(typeString),
                data = bytes.Skip(index + 8).Take(size).ToArray(),
                crc = bytes.ExtractInt(i - chunkCrc),
            };
            if (chunk.type != ChunkType.Unknown && !chunk.IsValid())
            {
                throw new ImageDecodingException($"Invalid chunk detected: {chunk.type}");
            }
            return chunk;
        }
        
        private void CheckCompability(Header header, Zlib zlib)
        {
            if (header.colorType != ColorType.Indexed)
            {
                throw new ImageDecodingException("Do not support other color type except indexed");
            }
            if (zlib.compression != 8 || zlib.windowSize != 7 || zlib.hasDictionary)
            {
                throw new ImageDecodingException("Unsupported compression algorithm");
            }
        }
        
        public byte[] Encode(Image image)
        {
            var header = CreateHeader(image);
            var data = image.ToBytes();
            var filtered = byteFilterer.ReverseFilter(data, header);
            var encoded = deflate.Encode(filtered);
            
            var headerChunk = header.ToChunk();
            var dataChunk = CreateDataChunk(encoded);
            var endChunk = CreateEndChunk();

            return pngFormatHeader.FromHexString()
                .Concat(headerChunk.ToBytes())
                .Concat(dataChunk.ToBytes())
                .Concat(endChunk.ToBytes())
                .ToArray();
        }
        
        private Header CreateHeader(Image image)
        {
            return new Header()
            {
                bitDepth = 8,
                colorType = ColorType.Truecolor,
                compression = 0,
                filterType = FilterType.None,
                height = image.Height,
                width = image.Width,
                transferMethod = 0
            };
        }
        
        private Chunk CreateDataChunk(byte[] data)
        {
            var flags = new byte[] {0x78, 0x9C};
            return new Chunk(flags.Concat(data).ToArray(), ChunkType.Data);
        }
        
        private Chunk CreateEndChunk()
        {
            return new Chunk(new byte[0], ChunkType.End);
        }
    }
}