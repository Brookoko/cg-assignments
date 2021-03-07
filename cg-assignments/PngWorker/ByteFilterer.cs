namespace ImageConverter.Png
{
    internal interface IByteFilterer
    {
        byte[,] Filter(byte[] decoded, Header header);
        
        byte[] ReverseFilter(byte[,] data, Header header);
    }
    
    internal class ByteFilterer : IByteFilterer
    {
        public byte[,] Filter(byte[] decoded, Header header)
        {
            var bytesPerPixel = GetBytesPerPixel(header.colorType);
            var h = header.height;
            var w = header.width * bytesPerPixel;
            var filtered = new byte[h, w];
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
        
        private int GetBytesPerPixel(ColorType type)
        {
            return type == ColorType.Truecolor || type == ColorType.TruecolorAlpha ? 3 : 1;
        }
        
        public byte[] ReverseFilter(byte[,] data, Header header)
        {
            var h = data.GetLength(0);
            var w = data.GetLength(1) + 1;
            var filtered = new byte[w * h];
            var function = FilterFunctionLibrary.GetReverseFunction(header.filterType);
            
            for (var i = 0; i < h; i++)
            {
                filtered[i * w] = 0;
                for (var j = 1; j < w; j++)
                {
                    var x = data[i, j - 1];
                    var ia = j - 1 + i * w;
                    var a = j > 1 ? filtered[ia] : 0;
                    var ib = j + (i - 1) * w;
                    var b = i > 0 ? filtered[ib] : 0;
                    var ic = j - 1 + (i - 1) * w;
                    var c = i > 0 && j > 1 ? filtered[ic] : 0;
                    var v = function(x, a, b, c);
                    filtered[j + i * w] = (byte) (v % 256);
                }
            }
            return filtered;
        }
    }
}