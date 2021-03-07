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
        
        private static readonly FilterFunction[] reverseFunctions =
        {
            None, SubReverse, UpReverse, AverageReverse, PaethReverse
        };
        
        public static FilterFunction GetFunction(FilterType filterType)
        {
            return functions[(int) filterType];
        }
        
        public static FilterFunction GetReverseFunction(FilterType filterType)
        {
            return reverseFunctions[(int) filterType];
        }
        
        private static byte None(int x, int a, int b, int c)
        {
            return (byte) x;
        }
        
        private static byte Sub(int x, int a, int b, int c)
        {
            return (byte) (x - a);
        }
        
        private static byte SubReverse(int x, int a, int b, int c)
        {
            return (byte) (x + a);
        }
        
        private static byte Up(int x, int a, int b, int c)
        {
            return (byte) (x - b);
        }
        
        private static byte UpReverse(int x, int a, int b, int c)
        {
            return (byte) (x + b);
        }
        
        private static byte Average(int x, int a, int b, int c)
        {
            return (byte) (x - (a + b) / 2);
        }
        
        private static byte AverageReverse(int x, int a, int b, int c)
        {
            return (byte) (x + (a + b) / 2);
        }
        
        private static byte Paeth(int x, int a, int b, int c)
        {
            var v = GetPaethCoef(a, b, c);
            return (byte) (x - v);
        }
        
        private static byte PaethReverse(int x, int a, int b, int c)
        {
            var v = GetPaethCoef(a, b, c);
            return (byte) (x + v);
        }
        
        private static int GetPaethCoef(int a, int b, int c)
        {
            var p = a + b - c;
            var pa = Math.Abs(p - a);
            var pb = Math.Abs(p - b);
            var pc = Math.Abs(p - c);
            var min = Math.Min(pa, Math.Min(pb, pc));
            return min == pa ? a : min == pb ? b : c;
        }
    } 
}