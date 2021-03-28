namespace JpegConverter.Jpeg.FileStructure.Structures
{
    using System.Linq;
    using Jpeg.Structures;

    internal class HuffmanTable
    {
        // Table class: 0=DC/Lossless, 1=AC
        public ByteHalf Tc;

        public ByteHalf Th;

        // Li
        public byte[] CodeLengthsAmount;

        // Vij
        public byte[][] SymbolLengthAssignment;

        public byte[] ToBytes()
        {
            return new[] {ByteHalf.Join(Tc, Th)}
                .Concat(CodeLengthsAmount)
                .Concat(SymbolLengthAssignment.SelectMany(lengthAssignments => lengthAssignments))
                .ToArray();
        }
    }
}