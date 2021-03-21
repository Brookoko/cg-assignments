namespace JpegConverter.Segmentation
{
    using System.Linq;
    using ImageConverter;

    internal class JfifApp0Segment : ISegment
    {
        public const string SegmentHexMarker = "FFD8";
        
        public const string JfifIdentifierHex = "4A46494600";
        
        public string HexMarker => SegmentHexMarker;

        public int BytesLength => HexMarker.HexStringLength() + nonMarkerSegmentLength;

        public int nonMarkerSegmentLength;
        
        public string identifierHex;
        public int majorVersion;
        public int minorVersion;
        
        public DensityUnits densityUnits;
        public int xDensity;
        public int yDensity;
        
        public int xThumbnail;
        public int yThumbnail;
        public Color[] thumnailData;
        public int ThumbnailDataLength => xThumbnail * yThumbnail;
        
        public byte[] ToBytes()
        {
            return HexMarker.FromHexString()
                .Concat(nonMarkerSegmentLength.ToBytes(2))
                .Concat(identifierHex.FromHexString())
                .Concat(majorVersion.ToBytes(1))
                .Concat(minorVersion.ToBytes(1))
                .Concat(((int)densityUnits).ToBytes(1))
                .Concat(xDensity.ToBytes(2))
                .Concat(yDensity.ToBytes(2))
                .Concat(xThumbnail.ToBytes(1))
                .Concat(yThumbnail.ToBytes(1))
                .Concat(thumnailData.SelectMany(color => color.ToBytes()))
                .ToArray();
        }
    }

    internal enum DensityUnits
    {
        NoUnits = 0,
        PixelsPerInch = 1,
        PixelsPerCentimeter = 2
    }
}