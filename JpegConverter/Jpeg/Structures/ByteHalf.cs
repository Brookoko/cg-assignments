namespace JpegConverter.Jpeg.Structures
{
    using System;

    public readonly struct ByteHalf : IEquatable<ByteHalf>
    {
        public byte Value { get; }

        private ByteHalf(byte value)
        {
            Value = value;
        }
        
        public static (ByteHalf, ByteHalf) Split(byte sourceByte)
        {
            return (new ByteHalf((byte) (sourceByte >> 4)), new ByteHalf((byte) (sourceByte & 15)));
        }

        public static byte Join(ByteHalf firstHalf, ByteHalf secondHalf)
        {
            return (byte) ((firstHalf.Value << 4) + secondHalf.Value);
        }

        public static ByteHalf SelectLeftBits(byte sourceByte)
        {
            return new ByteHalf((byte) (sourceByte >> 4));
        }

        public static ByteHalf SelectRightBits(byte sourceByte)
        {
            return new ByteHalf((byte) (sourceByte & 15));
        }
        

        public bool Equals(ByteHalf other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is ByteHalf other && Equals(other);
        }
        
        public static bool operator ==(ByteHalf first, ByteHalf second)
        {
            return first.Value == second.Value;
        }

        public static bool operator !=(ByteHalf first, ByteHalf second)
        {
            return !(first == second);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static explicit operator ByteHalf(byte value)
        {
            return ByteHalf.SelectRightBits(value);
        }
    }
}