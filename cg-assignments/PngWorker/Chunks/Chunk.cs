namespace ImageConverter.Png
{
    internal class Chunk
    {
        public int size;
        public ChunkType type;
        public byte[] data;
        public int crc;
    }
    
    internal enum ChunkType
    {
        Header,
        Palette,
        Data,
        End,
        Unknown,
    }
}