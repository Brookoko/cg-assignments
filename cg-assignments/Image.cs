namespace ImageConverter
{
    public class Image
    {
        public int Height => data.GetLength(0);
        public int Width => data.GetLength(1);
        
        private readonly Color[,] data;
        
        public Image(int w, int h)
        {
            data = new Color[h, w];
        }
        
        public byte[,] ToBytes()
        {
            var bytes = new byte[Height, 3 * Width];
            for (var i = 0; i < Height; i++)
            {
                for (var j = 0; j < 3 * Width; j += 3)
                {
                    var b = data[i, j/3].ToBytes();
                    bytes[i, j] = b[0];
                    bytes[i, j+1] = b[1];
                    bytes[i, j+2] = b[2];
                }
            }
            return bytes;
        }
        
        public Color this[int i, int j]
        {
            get => data[i, j];
            set => data[i, j] = value;
        }
    }
    
    public struct Color
    {
        public readonly byte r;
        public readonly byte g;
        public readonly byte b;
        
        public Color(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
        
        public byte[] ToBytes()
        {
            return new []{r, g, b};
        }
        
        public override bool Equals(object obj)
        {
            return obj is Color other && Equals(other);
        }
        
        public bool Equals(Color other)
        {
            return r == other.r && g == other.g && b == other.b;
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = r.GetHashCode();
                hashCode = (hashCode * 397) ^ g.GetHashCode();
                hashCode = (hashCode * 397) ^ b.GetHashCode();
                return hashCode;
            }
        }
        
        public static bool operator ==(Color left, Color right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(Color left, Color right)
        {
            return !left.Equals(right);
        }
    }
}