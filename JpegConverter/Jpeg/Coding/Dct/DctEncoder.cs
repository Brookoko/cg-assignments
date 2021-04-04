namespace JpegConverter.Jpeg.Coding.Dct
{
    using System;

    internal class DctEncoder
    {
        private readonly DctPerformer dctPerformer;

        private readonly Quantizer quantizer;

        // private EntropyEncoder

        public DctEncoder(Quantizer quantizerInstance /*, encoderInstance*/)
        {
            dctPerformer = new DctPerformer();
            quantizer = quantizerInstance;
        }


        /// <summary> Non-differential encoding for first block encoding (without last quantized DC required) </summary>
        /// <returns> Entropy encoded data and quantized DC coefficient</returns>
        public (byte[], short) EncodeSamplesBlock(ushort[,] targetSamplesBlock, byte precision)
        {
            return EncodeSamplesBlock(targetSamplesBlock, 0, precision);
        }

        /// <returns> Entropy encoded data and encoded quantized DC coefficient</returns>
        public (byte[], short) EncodeSamplesBlock(ushort[,] targetSamplesBlock, short lastQuantizedDc, byte precision)
        {
            var blockSize = (targetSamplesBlock.GetLength(0), targetSamplesBlock.GetLength(1));
            var processedBlock = new short[blockSize.Item1, blockSize.Item2];

            for (var i = 0; i < blockSize.Item1; i++)
            {
                for (var j = 0; j < blockSize.Item2; j++)
                {
                    processedBlock[i, j] = (short) targetSamplesBlock[i, j];
                }
            }

            // Dct encoding only works with 8x8 samples blocks
            if (blockSize.Item1 != 8 || blockSize.Item2 != 8)
            {
                throw new Exception($"Unprocessable block size `{blockSize.Item1}" +
                                    $"x{blockSize.Item2}` met.");
            }

            // Dct encoding only supports 8/12 bits precision
            if (precision != 12 && precision != 8)
            {
                throw new Exception($"Unprocessable precision `{precision}` met.");
            }

            // Shifting block values by 2^(precision-1)
            var shiftValue = (short) Math.Pow(2, precision - 1);
            for (var i = 0; i < blockSize.Item1; i++)
            {
                for (var j = 0; j < blockSize.Item2; j++)
                {
                    processedBlock[i, j] -= shiftValue;
                }
            }

            // Getting dct coefficients
            var fDctCoefficients = dctPerformer.GetFDctCoefficientsFor(processedBlock);

            // Performing quantization
            processedBlock = quantizer.Quantize(fDctCoefficients);

            // todo: Investigate for successive approximation and spectral selection procedures
            // Investigation results: for sequential DCT encoding, scan header specifies band that covers 0-63 DCT
            // coefficients inclusively. So that, all coefficients are encoded.

            // Ordering quantized coefficients in zig-zag order, selecting specific band of them (spectral selection)
            var zigZagOrderedData = ReorderToZigZag(processedBlock);

            // See: F.1.1.5.1 Encoding model for DC coefficients
            var quantizedDc = zigZagOrderedData[0];
            zigZagOrderedData[0] = (short) (zigZagOrderedData[0] - lastQuantizedDc);

            // todo: search for effective AC coefficients encoding procedures (F.1.1.5.2), EOB, run length encoding
            
            throw new NotImplementedException();
            return (new byte[1], quantizedDc);
        }

        private T[] ReorderToZigZag<T>(T[,] source)
        {
            var size = (source.GetLength(0), source.GetLength(1));
            var result = new T[size.Item1 * size.Item2];
            var resultIndex = 0;
            for (var i = 0; i < size.Item1; i++)
            {
                var jMax = Math.Min(i, size.Item2 - 1);
                for (var j = 0; j <= jMax; j++)
                {
                    result[resultIndex++] = source[i - j, j];
                }
            }

            return result;
        }
    }
}