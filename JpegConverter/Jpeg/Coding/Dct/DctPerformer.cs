namespace JpegConverter.Jpeg.Coding.Dct
{
    using System;

    internal class DctPerformer
    {
        public double[,] PerformFDctFor(byte[,] dataUnit)
        {
            var dataUnitSize = (dataUnit.GetLength(0), dataUnit.GetLength(0));
            if (dataUnitSize.Item1 != 8 || dataUnitSize.Item2 != 8)
            {
                throw new ArgumentException($"Forward DCT cannot be performed on " +
                                            $"{dataUnitSize.Item1}x{dataUnitSize.Item2} size data unit. Required size: 8x8");
            }

            var fDctResultTable = new double[8, 8];
            for (byte v = 0; v < dataUnitSize.Item1; v++)
            {
                for (byte u = 0; u < dataUnitSize.Item2; u++)
                {
                    fDctResultTable[v, u] = GetFDctValue(v, u, dataUnit);
                }
            }

            return fDctResultTable;
        }

        private double GetFDctValue(byte v, byte u, in byte[,] dataUnit)
        {
            var cv = (v == 0) ? 1 / Math.Sqrt(2) : 1;
            var cu = (u == 0) ? 1 / Math.Sqrt(2) : 1;

            var result = 0.25 * cv * cu;
            // todo: what the fuck is going on with orientation?
            for (var x = 0; x <= 7; x++)
            {
                for (var y = 0; y <= 7; y++)
                {
                    result += dataUnit[y, x] * Math.Cos((2 * x + 1) * u * Math.PI / 16d) *
                                   Math.Cos((2 * y + 1) * v * Math.PI / 16d);
                }
            }

            return result;
        }
    }
}