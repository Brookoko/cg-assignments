namespace ImageConverter
{
    using Png;

    class Program
    {
        static void Main(string[] args)
        {
            var file = args[0];
            var ioWorker = new IoWorker();
            var bytes = ioWorker.Read(file);
            var imageWorker = new PngWorker();
            var outputFile = "C:/Projects/cg-assignments/assets/out.png";
            var image = imageWorker.Decode(bytes);
            var outBytes = imageWorker.Encode(image);
            ioWorker.Write(outBytes, outputFile);
        }
    }
}
