namespace JpegConverterTests
{
    using JpegConverter.Jpeg.Coding.Dct;
    using JpegConverter.Jpeg.FileStructure.Structures;
    using JpegConverter.Jpeg.Structures;
    using NUnit.Framework;

    [TestFixture]
    public class QuantizerTests
    {
        private readonly QuantizerTestData test1Data = new QuantizerTestData()
        {
            InputData = new double[8, 8]
            {
                {162.3, 40.6, 20.0, 72.3, 30.3, 12.5, -19.7, -11.5},
                {30.5, 108.4, 10.5, 32.3, 27.7, -15.5, 18.4, -2.0},
                {-94.1, -60.1, 12.3, -43.4, -31.3, 6.1, -3.3, 7.1},
                {-38.6, -83.4, -5.4, -22.2, -13.5, 15.5, -1.3, 3.5},
                {-31.3, 17.9, -5.5, -12.4, 14.3, -6.0, 11.5, -6.0},
                {-0.9, -11.8, 12.8, 0.2, 28.1, 12.6, 8.4, 2.9},
                {4.6, -2.4, 12.2, 6.6, -18.7, -12.8, 7.7, 12.0},
                {-10.0, 11.2, 7.8, -16.3, 21.5, 0.0, 5.9, 10.7}
            },
            QuantizationElements = new ushort[]
            {
                16, 11, 10, 16, 24, 40, 51, 61,
                12, 12, 14, 19, 26, 58, 60, 55,
                14, 13, 16, 24, 40, 57, 69, 56,
                14, 17, 22, 29, 51, 87, 80, 62,
                18, 22, 37, 56, 68, 109, 103, 77,
                24, 35, 55, 64, 81, 104, 113, 92,
                49, 64, 78, 87, 103, 121, 120, 101,
                72, 92, 95, 98, 112, 100, 103, 99
            },
            ExpectedResult = new short[,]
            {
                {10, 4, 2, 5, 1, 0, 0, 0},
                {3, 9, 1, 2, 1, 0, 0, 0},
                {-7, -5, 1, -2, -1, 0, 0, 0},
                {-3, -5, 0, -1, 0, 0, 0, 0},
                {-2, 1, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0}
            }
        };

        [Test]
        public void TestQuantization_1()
        {
            TestQuantizationSample(test1Data);
        }

        private void TestQuantizationSample(QuantizerTestData testData)
        {
            var table = new QuantizationTable()
            {
                Pq = (ByteHalf) (byte) QuantizationTablePrecision.Bit8,
                Tq = (ByteHalf) 1,
                QuantizationElements = testData.QuantizationElements
            };
            var quantizer = new Quantizer(table);
            var result = quantizer.Quantize(testData.InputData);
            var resultSize = (result.GetLength(0), result.GetLength(1));

            var hasDifferentValues = false;
            var differences = string.Empty;
            
            for (var i = 0; i < resultSize.Item1; i++)
            {
                for (var j = 0; j < resultSize.Item2; j++)
                {
                    if (testData.ExpectedResult[i,j] != result[i,j])
                    {
                        hasDifferentValues = true;
                        differences += $"At [{i},{j}] expected {testData.ExpectedResult[i, j]} but got {result[i, j]}\n";
                    }
                }
            }

            Assert.IsFalse(hasDifferentValues, differences);
        }
    }

    internal class QuantizerTestData
    {
        public double[,] InputData;

        public ushort[] QuantizationElements;

        public short[,] ExpectedResult;
    }
}