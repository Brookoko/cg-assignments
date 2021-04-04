namespace JpegConverter.Jpeg.Coding.Dct
{
    using FileStructure.Structures;

    internal interface IQuantizatonTablesProvider
    {
        QuantizationTable GetLuminanceTable(QuantizationTablePrecision precisionMode,
            byte destinationIdentifier);

        QuantizationTable GetChrominanceTable(QuantizationTablePrecision precisionMode,
            byte destinationIdentifier);
    }
}