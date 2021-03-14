namespace ImageConverter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IImageWorkerProvider
    {
        IImageDecoder FindDecoder(byte[] data, string extension);
        
        IImageEncoder FindEncoder(string extension);
    }
    
    public class ImageWorkerProvider : IImageWorkerProvider
    {
        private readonly List<IImageEncoder> encoders;
        private readonly List<IImageDecoder> decoders;
     
        public ImageWorkerProvider(IPluginProvider pluginProvider)
        {
            encoders = pluginProvider.Types
                .Where(t => typeof(IImageEncoder).IsAssignableFrom(t) && !t.IsInterface)
                .Select(t => (IImageEncoder) Activator.CreateInstance(t))
                .ToList();
            decoders = pluginProvider.Types
                .Where(t => typeof(IImageDecoder).IsAssignableFrom(t) && !t.IsInterface)
                .Select(t => (IImageDecoder) Activator.CreateInstance(t))
                .ToList();
        }
        
        public IImageDecoder FindDecoder(byte[] data, string extension)
        {
            var decoder = decoders.FirstOrDefault(d => d.CanWorkWith(data, extension));
            if (decoder == null)
            {
                throw new Exception("Cannot decode image. Format is not supported");
            }
            return decoder;
        }
        
        public IImageEncoder FindEncoder(string extension)
        {
            var encoder = encoders.FirstOrDefault(en => en.CanWorkWith(extension));
            if (encoder == null)
            {
                throw new Exception("Cannot encode image. Format is not supported");
            }
            return encoder;
        }
    }
}