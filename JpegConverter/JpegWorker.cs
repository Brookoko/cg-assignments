namespace JpegConverter
{
    using System;
    using System.Linq;
    using ImageConverter;

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
            throw new NotImplementedException();
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