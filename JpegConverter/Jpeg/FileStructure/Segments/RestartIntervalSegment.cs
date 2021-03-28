namespace JpegConverter.Jpeg.FileStructure.Segments
{
    using System;
    using System.Linq;
    using Markers;

    internal class RestartIntervalSegment
    {
        // DRI marker: FFDD
        public JpegMarker SegmentMarker = new JpegMarker(0xDD);

        public ushort Lr;

        public ushort Ri;

        public byte[] ToBytes()
        {
            return SegmentMarker.ToBytes()
                .Concat(BitConverter.GetBytes(Lr))
                .Concat(BitConverter.GetBytes(Ri))
                .ToArray();
        }
    }
}