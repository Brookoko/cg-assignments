namespace ImageConverter
{
    using System;
    using System.IO;

    public interface IIoWorker
    {
        byte[] Read(string path);
        
        void Write(byte[] bytes, string path);
    }
    
    public class IoWorker : IIoWorker
    {
        public byte[] Read(string path)
        {
            path = path.Replace('/', Path.DirectorySeparatorChar);
            if (!File.Exists(path))
            {
                throw new Exception("File doesn't exist");
            }
            return File.ReadAllBytes(path);
        }
        
        public void Write(byte[] bytes, string path)
        {
            path = path.Replace('/', Path.DirectorySeparatorChar);
            File.WriteAllBytes(path, bytes);
        }
    }
}