namespace JpegConverter.Jpeg.FileStructure.Segments
{
    using System;
    using System.Linq;
    using Jpeg.Structures;
    using Markers;

    internal class ExpansionSegment
    {
        // EXP marker: FFDF
        public JpegMarker SegmentMarker = new JpegMarker(0xDF);

        public ushort Le;

        public ByteHalf Eh;

        public ByteHalf Ev;

        public byte[] ToBytes()
        {
            return SegmentMarker.ToBytes()
                .Concat(BitConverter.GetBytes(Le))
                .Concat(new[] {ByteHalf.Join(Eh, Ev)})
                .ToArray();
        }
    }
}