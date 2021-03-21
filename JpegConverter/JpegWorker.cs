namespace JpegConverter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ImageConverter;
    using Segmentation;

    public class JpegWorker : IImageWorker
    {
        private static readonly string[] targetFileExtensions = {"jpg","jpeg"};
        
        public bool CanWorkWith(string extension)
        {
            return targetFileExtensions.Contains(extension);
        }

        public bool CanWorkWith(byte[] bytes, string extension)
        {
            return CanWorkWith(extension) && CheckIfCompatibleWith(bytes);
        }

        private bool CheckIfCompatibleWith(byte[] bytes)
        {
            var requirements = new List<bool>();
            
            requirements.Add(bytes.Take(2).ToHexString() == StartOfImageSegment.SegmentHexMarker);
            requirements.Add(bytes.Skip(4).Take(5).ToHexString() == JfifApp0Segment.JfifIdentifierHex);

            return requirements.All(requirement => requirement);
        }

        public Image Decode(byte[] bytes)
        {
            throw new NotImplementedException("JPEG/JPG files decoding is not supported");
        }
        
        public byte[] Encode(Image image)
        {
            throw new NotImplementedException();
        }
    }
}