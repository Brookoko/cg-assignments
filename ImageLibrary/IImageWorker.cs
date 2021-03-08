namespace ImageConverter
{
    public interface IImageWorker : IImageDecoder, IImageEncoder
    {
    }
    
    public interface IImageDecoder
    {
        Image Decode(byte[] bytes);
        
        bool CanWorkWith(byte[] bytes, string extension);
    }
    
    public interface IImageEncoder
    {
        byte[] Encode(Image image);
        
        bool CanWorkWith(string extension);
    }
}