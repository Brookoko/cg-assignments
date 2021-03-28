namespace JpegConverter.Jpeg.FileStructure.Segments
{
    using System;
    using System.Linq;
    using Markers;

    internal class ApplicationDataSegment
    {
        // APPn marker: FFE0-FFEF
        public JpegMarker SegmentMarker;

        public ushort Lp;

        // Ap
        public byte[] ApplicationData;

        public byte[] ToBytes()
        {
            return SegmentMarker.ToBytes()
                .Concat(BitConverter.GetBytes(Lp))
                .Concat(ApplicationData)
                .ToArray();
        }
    }
}