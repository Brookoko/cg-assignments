namespace JpegConverter.Segmentation
{
    using ImageConverter;

    internal class EndOfImageSegment : ISegment
    {
        public const string SegmentHexMarker = "FFD8";
        
        public string HexMarker => SegmentHexMarker;

        public int BytesLength => HexMarker.HexStringLength();

        public byte[] ToBytes()
        {
            return HexMarker.FromHexString();
        }
    }
}