namespace JpegConverter.Jpeg.FileStructure.Segments
{
    using System;
    using System.Linq;
    using Markers;

    internal class DefineLinesNumberSegment
    {
        // DNL marker: FFDC
        public JpegMarker SegmentMarker = new JpegMarker(0xDC);

        public ushort Ld;

        public ushort NL;

        public byte[] ToBytes()
        {
            return SegmentMarker.ToBytes()
                .Concat(BitConverter.GetBytes(Ld))
                .Concat(BitConverter.GetBytes(NL))
                .ToArray();
        }
    }
}