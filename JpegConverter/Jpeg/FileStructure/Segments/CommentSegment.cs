namespace JpegConverter.Jpeg.FileStructure.Segments
{
    using System;
    using System.Linq;
    using Markers;

    internal class CommentSegment
    {
        // COM marker: FFFE
        public JpegMarker SegmentMarker = new JpegMarker(0xFE);

        public ushort Lc;

        // Cm
        public byte[] CommentBytes;

        public byte[] ToBytes()
        {
            return SegmentMarker.ToBytes()
                .Concat(BitConverter.GetBytes(Lc))
                .Concat(CommentBytes)
                .ToArray();
        }
    }
}