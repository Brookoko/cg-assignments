namespace JpegConverter.Jpeg.FileStructure.Segments
{
    using System;
    using System.Linq;
    using Markers;
    using Structures;

    internal class FrameHeader
    {
        // SOFn marker FFC0 - FFCF
        public JpegMarker SegmentMarker;

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