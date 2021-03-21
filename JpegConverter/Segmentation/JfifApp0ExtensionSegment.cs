namespace JpegConverter.Segmentation
{
    using System.Linq;
    using ImageConverter;

    internal class JfifApp0ExtensionSegment : ISegment
    {
        public const string SegmentHexMarker = "FFD8";
        
        public const string JfifExtensionIdentifierHex = "4A46585800";
        
        public string HexMarker => SegmentHexMarker;

        public int BytesLength => HexMarker.HexStringLength() + nonMarkerSegmentLength;

        public int nonMarkerSegmentLength;
        
        public string identifierHex;
        public ThumbnailFormat thumbnailFormat;
        public byte[] thumbnailData;
        
        public byte[] ToBytes()
        {
            return HexMarker.FromHexString()
                .Concat(nonMarkerSegmentLength.ToBytes(2))
                .Concat(identifierHex.FromHexString())
                .Concat(((int)thumbnailFormat).ToBytes(1))
                .Concat(thumbnailData)
                .ToArray();
        }
    }

    internal enum ThumbnailFormat
    {
        Jpeg = 16,
        Palette = 17,
        Rgb = 19
    }
}