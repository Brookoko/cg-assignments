namespace PpmConverter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using ImageConverter;

    public class PpmWorker : IImageWorker
    {
        private static readonly Regex whitespace = new Regex("[\t\\s]+");
        
        public bool CanWorkWith(byte[] bytes, string extension)
        {
            var formatName = bytes.ExtractString(0, 2);
            return formatName == "P3" && CanWorkWith(extension);
        }
        
        public bool CanWorkWith(string extension)
        {
            return extension == "ppm";
        }
        
        public byte[] Encode(Image image)
        {
            var format = "P3\n";
            var dimensions = $"{image.Width} {image.Height}\n";
            var max = "255\n";
            var values = ConvertToString(image);
            var data = format + dimensions + max + values;
            return data.ToBytes();
        }
        
        private string ConvertToString(Image image)
        {
            var values = new StringBuilder();
            for (var i = 0; i < image.Height; i++)
            {
                for (var j = 0; j < image.Width; j++)
                {
                    var color = image[i, j];
                    values.Append(color.r).Append(" ")
                        .Append(color.g).Append(" ")
                        .Append(color.b).Append(" ");
                }
                values.AppendLine();
            }
            return values.ToString();
        }
        
        public Image Decode(byte[] bytes)
        {
            var data = bytes.ExtractString(0, bytes.Length);
            var lines = Split(data);
            var (width, height) = ExtractDimensions(lines[1]);
            var max = ExtractMaxValue(lines[2]);
            var values = ExtractValues(lines);
            return ConvertToImage(values, width, height, max);
        }
        
        private string[] Split(string data)
        {
            var lines = data.Split('\n').Where(l => l[0] != '#').ToArray();
            if (lines.Length < 3)
            {
                throw new ImageDecodingException("Invalid image dimensions line");
            }
            return lines;
        }
        
        private (int width, int height) ExtractDimensions(string line)
        {
            var dimensions = line.Split(' ');
            if (dimensions.Length != 2)
            {
                throw new ImageDecodingException("Invalid image dimensions line");
            }
            if (!int.TryParse(dimensions[0], out var width))
            {
                throw new ImageDecodingException("Invalid image width format");
            }
            if (!int.TryParse(dimensions[1], out var height))
            {
                throw new ImageDecodingException("Invalid image height format");
            }
            return (width, height);
        }
        
        private int ExtractMaxValue(string line)
        {
            if (!int.TryParse(line, out var max))
            {
                throw new ImageDecodingException("Invalid image max value format");
            }
            return max;
        }
        
        private string[] ExtractValues(IEnumerable<string> lines)
        {
            return lines
                .Skip(3)
                .SelectMany(l => whitespace.Split(l))
                .Where(s => !string.IsNullOrEmpty(s))
                .ToArray();
        }
        
        private Image ConvertToImage(string[] values, int width, int height, int max)
        {
            var image = new Image(width, height);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var index = 3 * i * width + 3 * j;
                    var r = ExtractValue(values[index], max);
                    var g = ExtractValue(values[index + 1], max);
                    var b = ExtractValue(values[index + 2], max);
                    var color = new Color(r, g, b);
                    image[i, j] = color;
                }
            }
            return image;
        }
        
        private byte ExtractValue(string value, int max)
        {
            if (!float.TryParse(value, out var v))
            {
                throw new ImageDecodingException("Invalid image value format");
            }
            return (byte) (v / max * 255);
        }
    }
}