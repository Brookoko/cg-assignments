namespace ImageConverter.Png
{
    using System.Collections.Generic;
    using System.Linq;

    internal interface IChunkExtractor
    {
        List<Chunk> ExtractAllChunks(byte[] bytes, int startIndex);
        
        Chunk ExtractChunk(byte[] bytes, int index, out int i);
    }
    
    internal class ChunkExtractor : IChunkExtractor
    {
        private static int chunkType = 4;
        private static int chunkCrc = 4;
        private static int chunkSize = 4;
        
        public List<Chunk> ExtractAllChunks(byte[] bytes, int startIndex)
        {
            var chunks = new List<Chunk>();
            for (var i = startIndex; i < bytes.Length;)
            {
                var chunk = ExtractChunk(bytes, i, out i);
                chunks.Add(chunk);
            }
            return chunks;
        }
        
        public Chunk ExtractChunk(byte[] bytes, int index, out int i)
        {
            var size = bytes.ExtractInt(index);
            var typeString = bytes.ExtractString(index + chunkSize, chunkType);
            i = index + chunkSize + chunkType + size + chunkCrc;
            var chunk = new Chunk()
            {
                size = size,
                type = ChunkExtension.FromHeader(typeString),
                data = bytes.Skip(index + chunkSize + chunkType).Take(size).ToArray(),
                crc = bytes.ExtractInt(i - chunkCrc),
            };
            if (chunk.type != ChunkType.Unknown && !chunk.IsValid())
            {
                throw new ImageDecodingException($"Invalid chunk detected: {chunk.type}");
            }
            return chunk;
        }
    }
}