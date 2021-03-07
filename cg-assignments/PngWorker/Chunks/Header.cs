namespace ImageConverter.Png
{
    internal class Header : IDecodedChunk
    {
        private static int headerChunkSize = 13;
        
        public int width;
        public int height;
        public byte bitDepth;
        public ColorType colorType;
        public byte compression;
        public FilterType FilterType;
        public byte transferMethod;
        
        public void Init(Chunk[] chunks)
        {
            var chunk = chunks[0];
            width = chunk.data.ExtractInt(0);
            height = chunk.data.ExtractInt(4);
            bitDepth = chunk.data[8];
            colorType = (ColorType) chunk.data[9];
            compression = chunk.data[10];
            FilterType = (FilterType) chunk.data[11];
            transferMethod = chunk.data[12];
        }
        
        public bool IsCompatible(Chunk[] chunks)
        {
            if (chunks.Length > 1)
            {
                return false;
            }
            var chunk = chunks[0];
            return chunk.type == ChunkType.Header && chunk.size == headerChunkSize;
        }
    }
}