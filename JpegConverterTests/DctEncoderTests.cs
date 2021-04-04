namespace JpegConverterTests
{
    using NUnit.Framework;
    using JpegConverter.Jpeg.Coding.Dct;
    using JpegConverter.Jpeg.FileStructure.Structures;

    [TestFixture]
    public class DctEncoderTests
    {
        // test samples block from https://www.math.cuhk.edu.hk/~lmlui/dct.pdf
        private readonly ushort[,] samplesBlock1 = new ushort[8, 8]
        {
            {154, 123, 123, 123, 123, 123, 123, 136},
            {192, 180, 136, 154, 154, 154, 136, 110},
            {254, 198, 154, 154, 180, 154, 123, 123},
            {239, 180, 136, 180, 180, 166, 123, 123},
            {180, 154, 136, 167, 166, 149, 136, 136},
            {128, 136, 123, 136, 154, 180, 198, 154},
            {123, 105, 110, 149, 136, 136, 180, 166},
            {110, 136, 123, 123, 123, 136, 154, 136}
        };
        
        [Test]
        public void SamplesBlockEncodingTest_8bit()
        {
            byte precision = 8;

            IQuantizatonTablesProvider quantizationTablesProvider = new DefaultQuantizationTablesProvider();
            var luminanceTable = quantizationTablesProvider.GetLuminanceTable(QuantizationTablePrecision.Bit8, 1);
            var quantizer = new Quantizer(luminanceTable);
            var encoder = new DctEncoder(quantizer);

            var encodingResult = encoder.EncodeSamplesBlock(samplesBlock1, precision);
        }
    }
}