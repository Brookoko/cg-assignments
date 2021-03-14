namespace ImageConverter.Png
{
    using System.Linq;

    internal class Zlib : IDecodedChunk
    {
        public int windowSize;
        public int compression;
        public int compressionLevel;
        public bool hasDictionary;
        public int check;
        public byte[] data;
        
        public void Init(Chunk[] chunks)
        {
            var firstChunk = chunks[0];
            var stream = new BitStream();
            
            var cmf = firstChunk.data[0];
            stream.Write(cmf);
            windowSize = stream.Read(4);
            compression = stream.Read(4);
            
            var flag = firstChunk.data[1];
            stream.Write(flag);
            compressionLevel = stream.Read(2);
            hasDictionary = stream.Read(1) == 1;
            check = stream.Read(5);
            
            data = firstChunk.data
                .Skip(2).Take(firstChunk.size)
                .Concat(chunks.Skip(1).SelectMany(c => c.data))
                .ToArray();
        }
        
        public bool IsCompatible(Chunk[] chunks)
        {
            var firstChunk = chunks[0];
            return chunks.All(c => c.type == ChunkType.Data) &&
                   firstChunk.data[0] == 0x78 &&
                   firstChunk.data[1] == 0x9C;
        }
        
        public Chunk ToChunk()
        {
            var window = windowSize.ToBytes()[3];
            var compress = compression.ToBytes()[3];
            var cmf = (byte) ((window << 4) | (compress & 0x0F));
            
            var level = compressionLevel.ToBytes()[3];
            var dictionary = (byte) (hasDictionary ? 1 : 0);
            var flag = (byte) ((level << 6) | (dictionary << 5) | check);
            
            var concat = new[] {cmf, flag}.Concat(data).ToArray();
            return new Chunk(concat, ChunkType.Data);
        }
    }
}