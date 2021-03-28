namespace JpegConverter.Jpeg
{
    using Exceptions;
    using ImageConverter;

    internal class SofTypeQualifier
    {
        public (OperationMode, EncodingProcedureType, DifferentiationType) Qualify(byte[] marker)
        {
            if (marker.Length != 2)
            {
                throw new MarkerLengthException(marker);
            }

            if (marker[0] != 0xFF)
            {
                throw new MarkerException($"Not recognized marker `{marker.ToHexString()}` usage.");
            }

            if (marker[1] == 0xC8)
            {
                throw new MarkerException($"Marker `{marker.ToHexString()}` is reserved for JPEG extensions" +
                                          $" and can not be used.");
            }

            switch (marker[1])
            {
                case 0xC0 : return (OperationMode.SequnetialDct, EncodingProcedureType.Huffman, DifferentiationType.NonDifferential); // Baseline DCT
                case 0xC1 : return (OperationMode.SequnetialDct, EncodingProcedureType.Huffman, DifferentiationType.NonDifferential); // Extended sequential DCT
                case 0xC2 : return (OperationMode.ProgressiveDct, EncodingProcedureType.Huffman, DifferentiationType.NonDifferential); // Progressive DCT
                case 0xC3 : return (OperationMode.Lossless, EncodingProcedureType.Huffman, DifferentiationType.NonDifferential); // Lossless (sequential)
                
                case 0xC5 : return (OperationMode.SequnetialDct, EncodingProcedureType.Huffman, DifferentiationType.Differential); // Differential sequential DCT
                case 0xC6 : return (OperationMode.ProgressiveDct, EncodingProcedureType.Huffman, DifferentiationType.Differential); // Differential progressive DCT
                case 0xC7 : return (OperationMode.Lossless, EncodingProcedureType.Huffman, DifferentiationType.Differential); // Differential lossless (sequential)
                
                case 0xC9 : return (OperationMode.SequnetialDct, EncodingProcedureType.Arithmetic, DifferentiationType.NonDifferential); // Extended sequential DCT
                case 0xCA : return (OperationMode.ProgressiveDct, EncodingProcedureType.Arithmetic, DifferentiationType.NonDifferential); // Progressive DCT
                case 0xCB : return (OperationMode.Lossless, EncodingProcedureType.Arithmetic, DifferentiationType.NonDifferential); // Lossless (sequential)
                
                case 0xCD : return (OperationMode.SequnetialDct, EncodingProcedureType.Arithmetic, DifferentiationType.Differential); // Differential sequential DCT
                case 0xCE : return (OperationMode.ProgressiveDct, EncodingProcedureType.Arithmetic,DifferentiationType.Differential); // Differential progressive DCT
                case 0xCF : return (OperationMode.Lossless, EncodingProcedureType.Arithmetic, DifferentiationType.NonDifferential); // Differential lossless (sequential)
            }

            throw new MarkerException($"Could not qualify SOF marker `{marker.ToHexString()}`.");
        }
    }
}