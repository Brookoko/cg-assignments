namespace ImageConverter
{
    using Png;

    class Program
    {
        static void Main(string[] args)
        {
            var file = args[0];
            var worker = new IoWorker();
            var bytes = worker.Read(file);
            var reader = new PngWorker();
            var image = reader.Decode(bytes);
        }
    }
}
