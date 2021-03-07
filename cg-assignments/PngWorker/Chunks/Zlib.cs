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
            var cmf = firstChunk.data[0];
            windowSize = cmf.ReadBits(0, 4);
            compression = cmf.ReadBits(4, 4);
            var flag = firstChunk.data[1];
            compressionLevel = flag.ReadBits(0, 2);
            hasDictionary = flag.ReadBits(2, 1) == 1;
            check = flag.ReadBits(3, 5);
            data = firstChunk.data
                .Skip(2).Take(firstChunk.size)
                .Concat(chunks.Skip(1).SelectMany(c => c.data))
                .ToArray();
        }
        
        public bool IsCompatible(Chunk[] chunks)
        {
            return chunks.All(c => c.type == ChunkType.Data);
        }
    }
}