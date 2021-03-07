namespace ImageConverter.Png
{
    using System;

    internal delegate byte FilterFunction(int x, int a , int b, int c);
    
    internal static class FilterFunctionLibrary
    {
        private static readonly FilterFunction[] functions =
        {
            None, Sub, Up, Average, Paeth
        };
        
        public static FilterFunction GetFunction(FilterType filterType)
        {
            return functions[(int) filterType];
        }
        
        private static byte None(int x, int a, int b, int c)
        {
            return (byte) x;
        }
        
        private static byte Sub(int x, int a, int b, int c)
        {
            return (byte) (x - a);
        }
        
        private static byte Up(int x, int a, int b, int c)
        {
            return (byte) (x - b);
        }
        
        private static byte Average(int x, int a, int b, int c)
        {
            return (byte) (x - (a + b) / 2);
        }
        
        private static byte Paeth(int x, int a, int b, int c)
        {
            var p = a + b - c;
            var pa = Math.Abs(p - a);
            var pb = Math.Abs(p - b);
            var pc = Math.Abs(p - c);
            var min = Math.Min(pa, Math.Min(pb, pc));
            var v = min == pa ? a : min == pb ? b : c;
            return (byte) v;
        }
    } 
}