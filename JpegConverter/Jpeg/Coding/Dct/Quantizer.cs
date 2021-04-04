namespace JpegConverter.Jpeg.Coding.Dct
{
    using System;
    using FileStructure.Structures;

    internal class Quantizer
    {
        private readonly QuantizationTable quantizationTable;
        
        public Quantizer(QuantizationTable quantizationTable)
        {
            this.quantizationTable = quantizationTable;
        }

        public short[,] Quantize(double[,] targetData)
        {
            var dataSize = (targetData.GetLength(0), targetData.GetLength(1));
            var quantizedData = new short[dataSize.Item1, dataSize.Item2];
            
            for (var i = 0; i < dataSize.Item1; i++)
            {
                for (var j = 0; j < dataSize.Item2; j++)
                {
                    quantizedData[i, j] = (short) Math.Round(targetData[i, j] / quantizationTable.QuantizationElements[i * dataSize.Item1 + j]);
                }
            }
            
            return quantizedData;
        }
    }
}