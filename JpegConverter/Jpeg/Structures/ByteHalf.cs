namespace JpegConverter.Jpeg.Structures
{
    public readonly struct ByteHalf
    {
        public byte Value { get; }

        private ByteHalf(byte value)
        {
            Value = value;
        }

        public static (ByteHalf, ByteHalf) Split(byte sourceByte)
        {
            return (new ByteHalf((byte) (sourceByte >> 4)), new ByteHalf((byte) (sourceByte & 15)));
        }

        public static byte Join(ByteHalf firstHalf, ByteHalf secondHalf)
        {
            return (byte) ((firstHalf.Value << 4) + secondHalf.Value);
        }

        public static ByteHalf SelectFirstHalf(byte sourceByte)
        {
            return new ByteHalf((byte) (sourceByte >> 4));
        }

        public static ByteHalf SelectSecondHalf(byte sourceByte)
        {
            return new ByteHalf((byte) (sourceByte & 15));
        }
    }
}