namespace JpegConverterTests
{
    using System;
    using JpegConverter.Jpeg.Coding.Dct;
    using NUnit.Framework;

    [TestFixture]
    public class DctPerformerTests
    {
        // test samples block from https://www.math.cuhk.edu.hk/~lmlui/dct.pdf
        private readonly FDctTestData test1Data = new FDctTestData()
        {
            ShiftedSampleBlock = new short[8, 8]
            {
                {26, -5, -5, -5, -5, -5, -5, 8},
                {64, 52, 8, 26, 26, 26, 8, -18},
                {126, 70, 26, 26, 52, 26, -5, -5},
                {111, 52, 8, 52, 52, 38, -5, -5},
                {52, 26, 8, 39, 38, 21, 8, 8},
                {0, 8, -5, 8, 26, 52, 70, 26},
                {-5, -23, -18, 21, 8, 8, 52, 38},
                {-18, 8, -5, -5, -5, 8, 26, 8}
            },
            ExpectedFDctResult = new double[8, 8]
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
            ExpectedPrecision = 0.1d
        };
        
        [Test]
        public void TestFDct_Sample1()
        {
            TestFDct(test1Data);
        }

        private void TestFDct(FDctTestData testData)
        {
            var performer = new DctPerformer();
            var result = performer.GetFDctCoefficientsFor(testData.ShiftedSampleBlock);
            var resultSize = (result.GetLength(0), result.GetLength(1));
            
            var hasDifferentValues = false;
            var differences = string.Empty;
            
            for (var i = 0; i < resultSize.Item1; i++)
            {
                for (var j = 0; j < resultSize.Item2; j++)
                {
                    if (Math.Abs(testData.ExpectedFDctResult[i, j] - result[i, j]) > testData.ExpectedPrecision)
                    {
                        hasDifferentValues = true;
                        differences += $"At [{i},{j}] expected {testData.ExpectedFDctResult[i, j]} but got {result[i, j]}\n";
                    }
                }
            }

            Assert.IsFalse(hasDifferentValues, differences);
        } 
    }

    internal class FDctTestData
    {
        public short[,] ShiftedSampleBlock;

        public double[,] ExpectedFDctResult;
        
        public double ExpectedPrecision = 0.1d;
    }
}