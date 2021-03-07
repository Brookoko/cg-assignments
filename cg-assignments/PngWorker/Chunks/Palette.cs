namespace ImageConverter.Png
{
    using System.Collections.Generic;

    internal class Palette : IDecodedChunk
    {
        private readonly Dictionary<int, Color> palette = new Dictionary<int, Color>();
        
        public void Init(Chunk[] chunks)
        {
            var chunk = chunks[0];
            for (var i = 0; i < chunk.size / 3; i++)
            {
                palette[i] = ConvertToColor(chunk.data, i * 3);
            }
        }
        
        private Color ConvertToColor(byte[] bytes, int index)
        {
            var r = bytes[index];
            var g = bytes[index + 1];
            var b = bytes[index + 2];
            return new Color(r, g, b);
        } 
        
        public bool IsCompatible(Chunk[] chunks)
        {
            if (chunks.Length > 1)
            {
                return false;
            }
            var chunk = chunks[0];
            return chunk.type == ChunkType.Palette && chunk.size % 3 == 0;
        }
        
        public Image Map(byte[,] data)
        {
            var h = data.GetLength(1);
            var w = data.GetLength(0);
            var image = new Image(w, h);
            for (var i = 0; i < h; i++)
            {
                for (var j = 0; j < w; j++)
                {
                    var index = data[i, j];
                    if (palette.TryGetValue(index, out var color))
                    {
                        image[i, j] = color;
                    }
                }
            }
            return image;
        }
    }
}