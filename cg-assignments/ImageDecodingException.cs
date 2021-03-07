namespace ImageConverter
{
    using System;

    public class ImageDecodingException : Exception
    {
        public ImageDecodingException(string message) : base(message)
        {
        }
    }
}