namespace JpegConverter.Jpeg.FileStructure.Structures
{
    using System;
    using Jpeg.Structures;

    internal readonly struct ScanComponentSpecificationParameters
    {
        public const int BytesSize = 3;
        
        public byte Cs { get; }

        public ByteHalf Td { get; }
        
        public ByteHalf Ta { get; }

        public ScanComponentSpecificationParameters(byte cs, ByteHalf td, ByteHalf ta)
        {
            Cs = cs;
            Td = td;
            Ta = ta;
        }

        public ScanComponentSpecificationParameters(byte[] bytes)
        {
            if (bytes.Length != 3)
            {
                throw new ArgumentException("Component specification parameters can not" +
                                            $"be created from {bytes.Length} bytes.");
            }
            
            Cs = bytes[0];
            var byteSplit = ByteHalf.Split(bytes[1]);
            Td = byteSplit.Item1;
            Ta = byteSplit.Item2;
        }

        public byte[] ToBytes()
        {
            return new[]
            {
                Cs,
                ByteHalf.Join(Td,Ta),
            };
        }
    }
}