namespace JpegConverter.Jpeg.Coding.Dct
{
    using System.Collections.Generic;
    using System.Linq;
    using FileStructure.Structures;
    using Structures;

    internal class DefaultQuantizationTablesProvider : IQuantizatonTablesProvider
    {
        #region Luminance tables

        private static readonly ushort[] LuminanceQuantizationTable90 = new ushort[]
        {
            3, 2, 2, 3, 5, 8, 10, 12,
            2, 2, 3, 4, 5, 12, 12, 11,
            3, 3, 3, 5, 8, 11, 14, 11,
            3, 3, 4, 6, 10, 17, 16, 12,
            4, 4, 7, 11, 14, 22, 21, 15,
            5, 7, 11, 13, 16, 12, 23, 18,
            10, 13, 16, 17, 21, 24, 24, 21,
            14, 18, 19, 20, 22, 20, 20, 20
        };

        private static readonly ushort[] LuminanceQuantizationTable50 = new ushort[]
        {
            16, 11, 10, 16, 24, 40, 51, 61,
            12, 12, 14, 19, 26, 58, 60, 55,
            14, 13, 16, 24, 40, 57, 69, 56,
            14, 17, 22, 29, 51, 87, 80, 62,
            18, 22, 37, 56, 68, 109, 103, 77,
            24, 35, 55, 64, 81, 104, 113, 92,
            49, 64, 78, 87, 103, 121, 120, 101,
            72, 92, 95, 98, 112, 100, 103, 99
        };

        private static readonly ushort[] LuminanceQuantizationTable10 = new ushort[]
        {
            80, 60, 50, 80, 120, 200, 255, 255,
            55, 60, 70, 95, 130, 255, 255, 255,
            70, 65, 80, 120, 200, 255, 255, 255,
            70, 85, 110, 145, 255, 255, 255, 255,
            90, 110, 185, 255, 255, 255, 255, 255,
            120, 175, 255, 255, 255, 255, 255, 255,
            245, 255, 255, 255, 255, 255, 255, 255,
            255, 255, 255, 255, 255, 255, 255, 255
        };

        #endregion

        #region Chrominance tables

        private static readonly ushort[] ChrominanceQuantizationTable = new ushort[]
        {
            17, 18, 24, 47, 99, 99, 99, 99,
            18, 21, 26, 66, 99, 99, 99, 99,
            24, 26, 56, 99, 99, 99, 99, 99,
            47, 66, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99
        };

        #endregion

        private static readonly List<(float, ushort[])> LuminanceTables = new List<(float, ushort[])>()
        {
            (0.1f, LuminanceQuantizationTable10),
            (0.5f, LuminanceQuantizationTable50),
            (0.9f, LuminanceQuantizationTable90)
        };

        private static readonly List<(float, ushort[])> ChrominanceTables = new List<(float, ushort[])>()
        {
            (0.5f, ChrominanceQuantizationTable) // todo: Not sure if it's really 50% quality
        };

        public QuantizationTable GetLuminanceTable(QuantizationTablePrecision precisionMode,
            byte destinationIdentifier)
        {
            return GetLuminanceTable(precisionMode, destinationIdentifier, 0.5f);
        }

        public QuantizationTable GetLuminanceTable(QuantizationTablePrecision precisionMode,
            byte destinationIdentifier, float normalizedQuality)
        {
            return new QuantizationTable()
            {
                Pq = ByteHalf.SelectRightBits((byte) precisionMode),
                Tq = ByteHalf.SelectRightBits(destinationIdentifier),
                QuantizationElements = GetLuminanceQuantizationTableFor(normalizedQuality)
            };
        }

        private ushort[] GetLuminanceQuantizationTableFor(float normalizedQuality)
        {
            var result = LuminanceTables.FirstOrDefault(qualityTable =>
                qualityTable.Item1 >= normalizedQuality);
            if (result == default)
            {
                result = LuminanceTables.Last();
            }

            return result.Item2;
        }


        public QuantizationTable GetChrominanceTable(QuantizationTablePrecision precisionMode,
            byte destinationIdentifier)
        {
            return GetChrominanceTable(precisionMode, destinationIdentifier, 0.5f);
        }

        public QuantizationTable GetChrominanceTable(QuantizationTablePrecision precisionMode,
            byte destinationIdentifier, float normalizedQuality)
        {
            return new QuantizationTable()
            {
                Pq = ByteHalf.SelectRightBits((byte) precisionMode),
                Tq = ByteHalf.SelectRightBits(destinationIdentifier),
                QuantizationElements = GetChrominanceQuantizationTableFor(normalizedQuality)
            };
        }

        private ushort[] GetChrominanceQuantizationTableFor(float normalizedQuality)
        {
            var result = ChrominanceTables.FirstOrDefault(qualityTable =>
                qualityTable.Item1 >= normalizedQuality);
            if (result == default)
            {
                result = ChrominanceTables.Last();
            }

            return result.Item2;
        }
    }
}