namespace ImageConverter
{
    using System;
    using System.Linq;

    public interface IArgumentsParser
    {
        (string source, string output, string from, string to) Parse(string[] args);
    }
    
    public class ArgumentsParser : IArgumentsParser
    {
        public (string source, string output, string from, string to) Parse(string[] args)
        {
            if (args.Length < 2)
            {
                throw new ArgumentParseException("Insufficient number of parameters");
            }
            var source = ParseArgument("--source", args);
            var sourceFormat = GetExtension(source);
            var outputFormat = ParseArgument("--goal-format", args);
            if (!TryParseArgument("--output", args, out var output))
            {
                output = ChangeExtension(source, sourceFormat, outputFormat);
            }
            return (source, output, sourceFormat, outputFormat);
        }
        
        private bool TryParseArgument(string parameter, string[] args, out string res)
        {
            try
            {
                res = ParseArgument(parameter, args);
                return true;
            }
            catch (Exception _)
            {
                res = "";
                return false;
            }
        }
        
        private string ParseArgument(string parameter, string[] args)
        {
            var arg = args.FirstOrDefault(a => a.Contains(parameter));
            if (string.IsNullOrEmpty(arg))
            {
                throw new ArgumentParseException($"No parameter with name {parameter}");
            }
            var split = arg.Split('=');
            if (split.Length != 2 || split[0] != parameter)
            {
                throw new ArgumentParseException($"Invalid value for {parameter}");
            }
            return split[1];
        }
        
        private string GetExtension(string path)
        {
            var split = path.Split('.');
            return split[split.Length - 1];
        }
        
        private string ChangeExtension(string source, string sourceFormat, string outputFormat)
        {
            return source.Substring(0, source.Length - sourceFormat.Length) + outputFormat;
        }
    }
}