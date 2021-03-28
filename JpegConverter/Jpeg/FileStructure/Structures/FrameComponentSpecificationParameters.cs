namespace JpegConverter.Jpeg.FileStructure.Structures
{
    using System;
    using Jpeg.Structures;

    internal readonly struct FrameComponentSpecificationParameters
    {
        public const int BytesSize = 3;
        
        public byte C { get; }

        public ByteHalf H { get; }
        
        public ByteHalf V { get; }

        public byte T { get; }

        public FrameComponentSpecificationParameters(byte c, ByteHalf h, ByteHalf v, byte t )
        {
            C = c;
            H = h;
            V = v;
            T = t;
        }

        public FrameComponentSpecificationParameters(byte[] bytes)
        {
            if (bytes.Length != 3)
            {
                throw new ArgumentException("Component specification parameters can not" +
                                            $"be created from {bytes.Length} bytes.");
            }
            
            C = bytes[0];
            var byteSplit = ByteHalf.Split(bytes[1]);
            H = byteSplit.Item1;
            V = byteSplit.Item2;
            T = bytes[2];
        }

        public byte[] ToBytes()
        {
            return new[]
            {
                C,
                ByteHalf.Join(H,V),
                T
            };
        }
    }
}