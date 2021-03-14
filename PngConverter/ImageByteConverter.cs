namespace ImageConverter.Png
{
    using System.Linq;

    internal interface IImageByteConverter
    {
        byte[,] ToBytes(Image image);
        
        Image ToImage(byte[,] data, ColorType colorType);
    }
    
    internal class ImageByteConverter : IImageByteConverter
    {
        public byte[,] ToBytes(Image image)
        {
            var h = image.Height;
            var w = image.Width;
            var bytes = new byte[h, 3 * w];
            for (var i = 0; i < h; i++)
            {
                for (var j = 0; j < 3 * w; j += 3)
                {
                    var b = image[i, j/3].ToBytes();
                    bytes[i, j] = b[0];
                    bytes[i, j + 1] = b[1];
                    bytes[i, j + 2] = b[2];
                }
            }
            return bytes;
        }
        
        public Image ToImage(byte[,] data, ColorType colorType)
        {
            var bytesPerColor = GetBytesPerPixel(colorType);
            var h = data.GetLength(0);
            var w = data.GetLength(1) / bytesPerColor;
            var image = new Image(w, h);
            for (var i = 0; i < h; i++)
            {
                var row = GetRow(data, i);
                for (var j = 0; j < w; j++)
                {
                    var d = row.Skip(j * bytesPerColor).Take(bytesPerColor).ToArray();
                    var color = d.Length == 1 ? new Color(d[0]) : new Color(d[0], d[1], d[2]);
                    image[i, j] = color;
                }
            }
            return image;
        }
        
        private int GetBytesPerPixel(ColorType type)
        {
            return type == ColorType.Truecolor ? 3 : 1;
        }
        
        public T[] GetRow<T>(T[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[rowNumber, x])
                .ToArray();
        }
    }
}