namespace JpegConverter.Jpeg.FileStructure
{
    using Segments;

    internal class Scan
    {
        // todo: add tables
        
        public ScanHeader Header;

        public CodedSegment[] CodedSegments;

        public RestartIntervalSegment[] RestartIntervalSegments;
    }
}