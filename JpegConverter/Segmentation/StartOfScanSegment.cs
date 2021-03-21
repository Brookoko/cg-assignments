namespace JpegConverter.Segmentation
{
    internal class StartOfScanSegment : ISegment
    {
        public const string SegmentHexMarker = "FFDA";
        
        public string HexMarker { get; }
        
        public int BytesLength { get; }
        
        public byte[] ToBytes()
        {
            
        }
    }
}