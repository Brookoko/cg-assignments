namespace JpegConverter.Jpeg.FileStructure.Segments
{
    using Markers;

    internal class EoiSegment
    {
        // EOI marker: FFD9
        public JpegMarker SegmentMarker = new JpegMarker(0xD9);

        public byte[] ToBytes()
        {
            return SegmentMarker.ToBytes();
        }
    }
}