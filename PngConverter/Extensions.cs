namespace ImageConverter.Png
{
    internal static class Extensions
    {
        public static int GetBytesPerPixel(this ColorType colorType)
        {
            return
                colorType == ColorType.TruecolorAlpha ? 4 :
                colorType == ColorType.Truecolor ? 3 :
                colorType == ColorType.GreyscaleAlpha ? 2 : 1;
        }

        public static bool HasAlpha(this ColorType colorType)
        {
            return colorType == ColorType.TruecolorAlpha || colorType == ColorType.GreyscaleAlpha;
        }
    }
}