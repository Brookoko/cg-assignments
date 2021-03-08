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
        
        public Color(byte v)
        {
            r = v;
            g = v;
            b = v;
        }
        
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
    }
}