namespace JpegConverter.Jpeg
{
    using FileStructure;
    using FileStructure.Segments;

    // Represents non-hierarchical jpeg image
    internal class JpegImage
    {
        public SoiSegment StartOfImage;

        public Frame ImageFrame;
        
        public EoiSegment EndOfImage;
    }
}