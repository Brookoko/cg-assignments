namespace JpegConverter.Jpeg.FileStructure.Structures
{
    using Jpeg.Structures;

    internal class ArithmeticConditioningTable
    {
        // Table class: 0=DC/Lossless, 1=AC
        public ByteHalf Tc;

        public ByteHalf Tb;

        // todo: use both AC-type byte value and DC-type 2x(4-bit params)
        public byte Cs;

        public byte[] ToBytes()
        {
            return new[]
            {
                ByteHalf.Join(Tc, Tb),
                Cs
            };
        }
    }
}