namespace ImageConverter
{
    using System;

    public class DataConversionException : Exception
    {
        public DataConversionException(string message) : base(message)
        {
        }
    }
}