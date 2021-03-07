namespace ImageConverter.Png
{
    internal interface IByteFilterer
    {
        byte[,] Filter(byte[] decoded, Header header);
    }
    
    internal class ByteFilterer : IByteFilterer
    {
        public byte[,] Filter(byte[] decoded, Header header)
        {
            var filtered = new byte[header.width, header.height];
            var h = header.height;
            var w = header.width;
            var depth = header.bitDepth;
            for (var i = 0; i < h; i++)
            {
                var offset = i * w * depth + (i + 1) * 8;
                var type = (FilterType) decoded.ReadBits(offset - 8, 8);
                var function = FilterFunctionLibrary.GetFunction(type);
                for (var j = 0; j < w; j++)
                {
                    var valueOffset = offset + j * depth;
                    var x = decoded.ReadBits(valueOffset, depth);
                    var a = j > 0 ? decoded.ReadBits(valueOffset - depth, depth) : 0;
                    var b = i > 0 ? decoded.ReadBits(valueOffset - w * depth, depth) : 0;
                    var c = i > 0 && j > 0 ? decoded.ReadBits(valueOffset - w * depth - depth, depth) : 0;
                    var v = function(x, a, b, c);
                    filtered[i, j] = (byte) (v % 256);
                }
            }
            return filtered;
        }
    }
}