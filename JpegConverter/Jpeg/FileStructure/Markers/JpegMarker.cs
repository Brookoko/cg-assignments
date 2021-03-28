namespace JpegConverter.Jpeg.FileStructure.Markers
{
    using Exceptions;

    internal readonly struct JpegMarker
    {
        public const int BytesSize = 2;
        
        public byte[] Code { get; }
        
        public JpegMarker(byte code)
        {
            if (code == 0x00 || code == 0xFF)
            {
                throw new MarkerException($"Jpeg marker's second byte can not be {code:X2}.");
            }
            Code = new byte[] {0xFF, code};
        }

        public byte[] ToBytes()
        {
            return Code;
        }

        public static implicit operator byte[](JpegMarker marker)
        {
            return marker.ToBytes();
        }
    }
}