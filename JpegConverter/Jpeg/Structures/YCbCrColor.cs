namespace JpegConverter.Jpeg.Structures
{
    using ColorFormatting;
    using ImageConverter;

    // todo: precisions
    public readonly struct YCbCrColor
    {
        public readonly byte Y;

        public readonly byte Cb;

        public readonly byte Cr;

        public YCbCrColor(byte cr, byte cb, byte y)
        {
            Cr = cr;
            Cb = cb;
            Y = y;
        }
        
        public static explicit operator YCbCrColor(Color color)
        {
            return YCbCrColorTransformer.ToYCbCr(color);
        }
    }
}