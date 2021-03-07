namespace ImageConverter.Png
{
    using System.Linq;
    using System.Text;

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
            var typeBytes = Encoding.UTF8.GetBytes(type.ToHeader());
            var crcBytes = typeBytes.Concat(data).ToArray();
            return (int) CrcExtractor.Extract(crcBytes);
        }
        
        public byte[] ToBytes()
        {
            var sizeBytes = size.ToBytes();
            var typeBytes = Encoding.UTF8.GetBytes(type.ToHeader());
            var crcBytes = crc.ToBytes();
            return sizeBytes
                .Concat(typeBytes)
                .Concat(data)
                .Concat(crcBytes)
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