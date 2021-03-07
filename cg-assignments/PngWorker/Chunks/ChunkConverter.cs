namespace ImageConverter.Png
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    internal interface IChunkConverter
    {
        T Extract<T>(IEnumerable<Chunk> chunks, ChunkType type) where T : IDecodedChunk, new();
        
        T Convert<T>(Chunk[] chunks) where T : IDecodedChunk, new();
        
        T Convert<T>(Chunk chunk) where T : IDecodedChunk, new();
    }
    
    internal class ChunkConverter : IChunkConverter
    {
        public T Extract<T>(IEnumerable<Chunk> chunks, ChunkType type) where T : IDecodedChunk, new()
        {
            var filteredChunks = chunks.Where(c => c.type == type).ToArray();
            if (filteredChunks.Length == 0)
            {
                throw new Exception();
            }
            return Convert<T>(filteredChunks);
        }
        
        public T Convert<T>(Chunk[] chunks) where T : IDecodedChunk, new()
        {
            var decoded = new T();
            if (!decoded.IsCompatible(chunks))
            {
                throw new Exception();
            }
            decoded.Init(chunks);
            return decoded;
        }
        
        public T Convert<T>(Chunk chunk) where T : IDecodedChunk, new()
        {
            return Convert<T>(new[] {chunk});
        }
    }
}