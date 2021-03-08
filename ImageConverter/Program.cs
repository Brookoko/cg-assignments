namespace ImageConverter
{
    using System;

    public static class Program
    {
        private static IIoWorker ioWorker;
        private static IArgumentsParser parser;
        private static IImageWorkerProvider workerProvider;

        private static void Main(string[] args)
        {
            Initialize();
            try
            {
                var (source, output, from, to) = parser.Parse(args);
                var data = ioWorker.Read(source);

                var decoder = workerProvider.FindDecoder(data, from);
                var encoder = workerProvider.FindEncoder(to);
                
                var image = decoder.Decode(data);
                var convertedData = encoder.Encode(image);
                
                ioWorker.Write(convertedData, output);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to convert image. See exception below for more details.");
                Console.WriteLine(e);
                throw;
            }
        }
        
        private static void Initialize()
        {
            ioWorker = new IoWorker();
            parser = new ArgumentsParser();
            workerProvider = new ImageWorkerProvider();
        }
    }
}
