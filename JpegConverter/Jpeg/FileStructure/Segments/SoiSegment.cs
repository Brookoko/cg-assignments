namespace JpegConverter.Jpeg.FileStructure.Segments
{
    using Markers;

    internal class SoiSegment
    {
        // SOI marker FFD8
        public JpegMarker SegmentMarker = new JpegMarker(0xD8);

        public byte[] ToBytes()
        {
            return SegmentMarker.ToBytes();
        }
    }
}