namespace JpegConverter.Jpeg.FileStructure.Segments
{
    using System;
    using System.Linq;
    using Markers;
    using Structures;

    // todo: same as frame header?? Not sure if it's correct written
    internal class DhpSegment
    {
        // DHP marker: FFDE 
        public JpegMarker SegmentMarker = new JpegMarker(0xDE);

        public ushort Lf;

        public byte P;

        public ushort Y;

        public ushort X;

        public byte Nf;

        public FrameComponentSpecificationParameters[] FrameCspArray;

        public byte[] ToBytes()
        {
            return SegmentMarker.ToBytes()
                .Concat(BitConverter.GetBytes(Lf))
                .Concat(new[] {P})
                .Concat(BitConverter.GetBytes(Y))
                .Concat(BitConverter.GetBytes(X))
                .Concat(new[] {Nf})
                .Concat(FrameCspArray.SelectMany(csp => csp.ToBytes()))
                .ToArray();
        }
    }
}