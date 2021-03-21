namespace JpegConverter.Segmentation
{
    internal interface ISegment
    {
        string HexMarker { get; }

        int BytesLength { get; }
        
        byte[] ToBytes();
    }

    internal interface IAutoReadableSegment : ISegment
    {
        bool ReadFromHead(byte[] bytes);
    }
}