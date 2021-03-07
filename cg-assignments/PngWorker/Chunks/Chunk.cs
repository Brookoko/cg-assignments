namespace ImageConverter.Png
{
    using System.Linq;

    internal class Chunk
    {
        public int size;
        public ChunkType type;
        public byte[] data;
        public int crc;
        
        public Chunk()
        {
        }
        
        public Chunk(byte[] data, ChunkType type)
        {
            size = data.Length;
            this.type = type;
            this.data = data;
            crc = CalculateCrc();
        }
        
        public bool IsValid()
        {
            var targetCrc = CalculateCrc();
            return targetCrc == crc;
        }
        
        private int CalculateCrc()
        {
            var typeBytes = type.ToHeader().ToBytes();
            var crcBytes = typeBytes.Concat(data).ToArray();
            return (int) CrcExtractor.Extract(crcBytes);
        }
        
        public byte[] ToBytes()
        {
            return size.ToBytes()
                .Concat(type.ToHeader().ToBytes())
                .Concat(data)
                .Concat(crc.ToBytes())
                .ToArray();
        }
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