namespace ImageConverter.Png
{
    using System.Collections.Generic;
    using System.Linq;

    public class PngWorker : IImageWorker
    {
        private static int headerSize = 8;
        private static string pngFormatHeader = "89504E470D0A1A0A";
        
        private readonly IChunkExtractor chunkExtractor = new ChunkExtractor();
        private readonly Deflate deflate = new Deflate();
        private readonly IChunkConverter chunkConverter = new ChunkConverter();
        private readonly IByteFilterer byteFilterer = new ByteFilterer();
        private readonly IImageByteConverter imageByteConverter = new ImageByteConverter();
        
        public bool CanWorkWith(byte[] bytes, string extension)
        {
            var format = bytes.Take(8).ToHexString();
            return format == pngFormatHeader && CanWorkWith(extension);
        }
        
        public bool CanWorkWith(string extension)
        {
            return extension == "png";
        }
        
        public Image Decode(byte[] bytes)
        {
            var chunks = chunkExtractor.ExtractAllChunks(bytes, headerSize);
            var header = chunkConverter.Extract<Header>(chunks, ChunkType.Header);
            var zlib = chunkConverter.Extract<Zlib>(chunks, ChunkType.Data);
            
            CheckCompability(header, zlib);
            
            var decoded = deflate.Decode(zlib.data);
            var filtered = byteFilterer.ReverseFilter(decoded, header);
            var image = header.colorType == ColorType.Indexed ?
                MapIndexes(filtered, chunks) :
                imageByteConverter.ToImage(filtered, header.colorType);
            return image;
        }
        
        private void CheckCompability(Header header, Zlib zlib)
        {
            if (header.colorType.HasAlpha())
            {
                throw new ImageDecodingException("Alpha is not supported");
            }
            if (!HasValidCompression(zlib))
            {
                throw new ImageDecodingException("Compression is not supported");
            }
        }
        
        private bool HasValidCompression(Zlib zlib)
        {
            return zlib.windowSize == 7 &&
                   zlib.compression == 8 &&
                   zlib.compressionLevel == 2 &&
                   !zlib.hasDictionary &&
                   zlib.check == 28;
        }
        
        private Image MapIndexes(byte[,] filtered, List<Chunk> chunks)
        {
            var palette = chunkConverter.Extract<Palette>(chunks, ChunkType.Palette);
            return palette.Map(filtered);
        }
        
        public byte[] Encode(Image image)
        {
            var header = CreateHeader(image);
            var data = imageByteConverter.ToBytes(image);
            var filtered = byteFilterer.Filter(data, FilterType.None);
            var encoded = deflate.Encode(filtered);
            
            var chunks = new [] {header.ToChunk(), CreateDataChunk(encoded), CreateEndChunk()};
            var content = chunks.SelectMany(c => c.ToBytes());
            var pngHeader = pngFormatHeader.FromHexString();
            
            return pngHeader.Concat(content).ToArray();
        }
        
        private Header CreateHeader(Image image)
        {
            return new Header()
            {
                bitDepth = 8,
                colorType = ColorType.Truecolor,
                compression = 0,
                filterMethod = 0,
                height = image.Height,
                width = image.Width,
                transferMethod = 0
            };
        }
        
        private Chunk CreateDataChunk(byte[] data)
        {
            return new Chunk(data, ChunkType.Data);
        }
        
        private Chunk CreateEndChunk()
        {
            return new Chunk(new byte[0], ChunkType.End);
        }
    }
}