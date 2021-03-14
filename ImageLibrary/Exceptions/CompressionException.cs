namespace ImageConverter
{
    using System;

    public class CompressionException : Exception
    {
        public CompressionException(string message) : base(message)
        {
        }
    }
}