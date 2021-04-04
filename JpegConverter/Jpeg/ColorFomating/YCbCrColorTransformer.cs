namespace JpegConverter.Jpeg.ColorFormatting
{
    using System;
    using ImageConverter;
    using Structures;

    internal static class YCbCrColorTransformer
    {
        public static YCbCrColor ToYCbCr(Color rgbColor)
        {
            var y = 0.299f * rgbColor.r + 0.587f * rgbColor.g + 0.114f * rgbColor.b;
            var cb = -0.16874f * rgbColor.r - 0.33126f * rgbColor.g + 0.5f * rgbColor.b;
            var cr = 0.5f * rgbColor.r - 0.41869f * rgbColor.g - 0.08131f * rgbColor.b;
            return new YCbCrColor((byte)Math.Round(y), (byte)Math.Round(cb), (byte)Math.Round(cr));
        }
    }
}