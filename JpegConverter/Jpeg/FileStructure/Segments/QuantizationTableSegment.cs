namespace JpegConverter.Jpeg.FileStructure.Segments
{
    using System;
    using System.Linq;
    using Markers;
    using Structures;

    internal class QuantizationTableSegment
    {
        // DQT marker: FFDB
        public JpegMarker SegmentMarker = new JpegMarker(0xDB);

        public ushort Lq => (ushort) (2 + QuantizationTables.Sum(table => 65 + table.Pq.Value * 64));

        public QuantizationTable[] QuantizationTables;

        public byte[] ToBytes()
        {
            return SegmentMarker.ToBytes()
                .Concat(BitConverter.GetBytes(Lq))
                .Concat(QuantizationTables.SelectMany(table => table.ToBytes()))
                .ToArray();
        }
    }
}