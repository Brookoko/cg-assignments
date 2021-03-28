namespace JpegConverter.Jpeg.FileStructure.Structures
{
    using System;
    using System.Linq;
    using Jpeg.Structures;

    internal class QuantizationTable
    {
        public ByteHalf Pq;

        public ByteHalf Tq;

        public ushort[] QuantizationElements;

        public byte[] ToBytes()
        {
            var elementsBytes = ((QuantizationTablePrecision) Pq.Value).GetPrecisedTableBytes(QuantizationElements);
            return new[] {ByteHalf.Join(Pq,Tq)}
                .Concat(elementsBytes)
                .ToArray();
        }
    }

    internal enum QuantizationTablePrecision
    {
        Bit8 = 0,
        Bit16 = 1
    }

    internal static class QuantizationTablePrecisionExtensions
    {
        public static byte[] GetPrecisedTableBytes(this QuantizationTablePrecision precision, ushort[] quantizationElements)
        {
            if (precision == QuantizationTablePrecision.Bit16)
            {
                var resultBytesAmount = quantizationElements.Length * 2;
                var bytesResult = new byte[resultBytesAmount];
                Buffer.BlockCopy(quantizationElements, 0, bytesResult, 0, resultBytesAmount);
                return bytesResult;
            }

            if (precision == QuantizationTablePrecision.Bit8)
            {
                return quantizationElements.Select(element => (byte) element).ToArray();
            }

            throw new Exception($"Quantization table precision types wasn't set up for casting type {precision}" +
                                $" to bytes array.");
        }
    }
}