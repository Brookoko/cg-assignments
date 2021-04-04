namespace JpegConverter.Jpeg.Coding.Dct
{
    using System;

    internal class DctPerformer
    {
        public double[,] GetFDctCoefficientsFor(short[,] samplesBlock)
        {
            var blockSize = (samplesBlock.GetLength(0), samplesBlock.GetLength(1));

            var fDctResultTable = new double[blockSize.Item1, blockSize.Item2];
            for (byte v = 0; v < blockSize.Item1; v++)
            {
                for (byte u = 0; u < blockSize.Item2; u++)
                {
                    fDctResultTable[v, u] = GetFDctValue(v, u, samplesBlock, blockSize);
                }
            }

            return fDctResultTable;
        }

        private double GetFDctValue(byte v, byte u, in short[,] samplesBlock, (int, int) blockSize)
        {
            // For FDct: y - downside, x - right 
            var cv = (v == 0) ? 1 / Math.Sqrt(2) : 1;
            var cu = (u == 0) ? 1 / Math.Sqrt(2) : 1;

            var result = 0d;
            // todo: what the fuck is going on with orientation?
            for (var y = 0; y < blockSize.Item1; y++)
            {
                for (var x = 0; x < blockSize.Item2; x++)
                {
                    result += samplesBlock[y, x] * Math.Cos((2 * x + 1) * u * Math.PI / 16d) *
                              Math.Cos((2 * y + 1) * v * Math.PI / 16d);
                }
            }
            result *= 0.25 * cv * cu;

            return result;
        }
    }
}