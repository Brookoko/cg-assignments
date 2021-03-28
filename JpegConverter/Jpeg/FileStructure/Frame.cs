namespace JpegConverter.Jpeg.FileStructure
{
    using Segments;

    internal class Frame
    {
        // todo: add tables
        
        public FrameHeader Header;

        // Optional
        public DefineLinesNumberSegment DnlSegment;

        public Scan[] FrameScans;
    }
}