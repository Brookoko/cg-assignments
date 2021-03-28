namespace JpegConverter.Jpeg.Exceptions
{
    using System;

    public class MarkerLengthException : Exception
    {
        public override string Message => $"Use of marker with forbidden length exception thrown.";

        private readonly byte[] marker;
        
        public string MarkerString => BitConverter.ToString(marker).Replace('-',' ');
        
        public MarkerLengthException(string message) : base(message)
        {
        }

        public MarkerLengthException(byte[] marker)
        {
            this.marker = marker;
        }
    }
}