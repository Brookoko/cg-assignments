namespace ImageConverter
{
    public class Image
    {
        public readonly Color[,] data;
        
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
        public byte r;
        public byte g;
        public byte b;
        
        public Color(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
}