namespace JpegConverter.Jpeg.FileStructure.Segments
{
    using System;
    using System.Linq;
    using Markers;
    using Structures;

    internal class ArithmeticConditioningTableSegment
    {
        // DAC marker: FFCC
        public JpegMarker SegmentMarker = new JpegMarker(0xCC);

        public ushort La;

        public ArithmeticConditioningTable[] ArithmeticConditioningTables;

        public byte[] TobBytes()
        {
            return SegmentMarker.ToBytes()
                .Concat(BitConverter.GetBytes(La))
                .Concat(ArithmeticConditioningTables.SelectMany(table => table.ToBytes()))
                .ToArray();
        }
    }
}