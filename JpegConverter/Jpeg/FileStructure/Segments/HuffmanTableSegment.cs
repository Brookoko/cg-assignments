namespace JpegConverter.Jpeg.FileStructure.Segments
{
    using System;
    using System.Linq;
    using Markers;
    using Structures;

    internal class HuffmanTableSegment
    {
        // DHT marker: FFC4
        public JpegMarker SegmentMarker = new JpegMarker(0xC4);

        public ushort Lh;

        public HuffmanTable[] HuffmanTables;

        public byte[] ToBytes()
        {
            return SegmentMarker.ToBytes()
                .Concat(BitConverter.GetBytes(Lh))
                .Concat(HuffmanTables.SelectMany(table => table.ToBytes()))
                .ToArray();
        }
    }
}