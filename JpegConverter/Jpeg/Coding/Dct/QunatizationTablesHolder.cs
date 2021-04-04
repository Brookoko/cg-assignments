namespace JpegConverter.Jpeg.Coding.Dct
{
    using System;
    using FileStructure.Structures;
    using Structures;

    internal class QunatizationTablesHolder : IQuantizatonTablesProvider
    {
        private readonly QuantizationTable[] luminanceTables = new QuantizationTable[4];

        private readonly QuantizationTable[] chrominanceTables = new QuantizationTable[4];

        public QuantizationTable GetLuminanceTable(QuantizationTablePrecision precisionMode, byte destinationIdentifier)
        {
            var selectedTable = luminanceTables[destinationIdentifier];
            if (selectedTable == default || (ByteHalf)(byte) precisionMode != selectedTable.Pq)
            {
                throw new Exception($"Required table at destination id: {destinationIdentifier} " +
                                    $"with precision: {precisionMode} couldn't be provided");
            }

            return selectedTable;
        }

        public QuantizationTable GetChrominanceTable(QuantizationTablePrecision precisionMode, byte destinationIdentifier)
        {
            var selectedTable = chrominanceTables[destinationIdentifier];
            if (selectedTable == default || (ByteHalf)(byte) precisionMode != selectedTable.Pq)
            {
                throw new Exception($"Required table at destination id: {destinationIdentifier} " +
                                    $"with precision: {precisionMode} couldn't be provided");
            }

            return selectedTable;
        }
    }
}