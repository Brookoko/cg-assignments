namespace ImageConverter.Png
{
    internal static class ChunkExtension
    {
        public static string ToHeader(this ChunkType type)
        {
            switch (type)
            {
                case ChunkType.Header:
                    return "IHDR";
                case ChunkType.Palette:
                    return "PLTE";
                case ChunkType.Data:
                    return "IDAT";
                case ChunkType.End:
                    return "IEND";
                default:
                    return "Unknown";
            }
        }
        
        public static ChunkType FromHeader(string name)
        {
            switch (name)
            {
                case "IHDR":
                    return ChunkType.Header;
                case "PLTE":
                    return ChunkType.Palette;
                case "IDAT":
                    return ChunkType.Data;
                case "IEND":
                    return ChunkType.End;
                default:
                    return ChunkType.Unknown;
            }
        }
    }
}