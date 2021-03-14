namespace ImageConverter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Huffman
    {
        public static readonly Huffman Literal;
        public static readonly Huffman Distance;
        
        private int[] codes;
        private int[] codeBitLengths;
        private int maxBitLength;

        private Dictionary<int, List<int>> dictionary;

        static Huffman()
        {
            Literal = new Huffman(286);
            var nextCode = 0;
            for (var i = 256; i <= 279; i++) {
                Literal.codes[i] = nextCode++;
                Literal.codeBitLengths[i] = 7;
            }
            nextCode = 48;
            for (var i = 0; i <= 143; i++) {
                Literal.codes[i] = nextCode++;
                Literal.codeBitLengths[i] = 8;
            }
            nextCode = 192;
            for (var i = 280; i <= 285; i++) {
                Literal.codes[i] = nextCode++;
                Literal.codeBitLengths[i] = 8;
            }
            nextCode = 400;
            for (var i = 144; i <= 255; i++) {
                Literal.codes[i] = nextCode++;
                Literal.codeBitLengths[i] = 9;
            }
            Literal.BuildDictionary();
            Literal.maxBitLength = 9;
            
            // Generate fixed distance codes
            Distance = new Huffman(30);
            for (var i = 0; i <= 29; i++) {
                Distance.codes[i] = i;
                Distance.codeBitLengths[i] = 5;
            }
            Distance.BuildDictionary();
            Distance.maxBitLength = 5;
        }
        
        public Huffman()
        {
        }
        
        public Huffman(int length)
        { 
            codes = new int[length]; 
            codeBitLengths = new int[length];
        }
        
        public void Build(int[] codeBitLengths)
        {
            this.codeBitLengths = codeBitLengths;
            maxBitLength = codeBitLengths.Max();
            var codeCounts = GetBitCount();
            var firstCodes = GetFirstCodes(codeCounts);
            codes = AssignCodes(firstCodes);
            BuildDictionary();
        }
        
        private int[] GetBitCount()
        {
            var codeCounts = new int[maxBitLength + 1];
            foreach (var length in codeBitLengths)
            {
                codeCounts[length]++;
            }
            codeCounts[0] = 0;
            return codeCounts;
        }
        
        private int[] GetFirstCodes(int[] codeCount)
        {
            var code = 0;
            var codes = new int[maxBitLength + 1];
            for (var i = 1; i <= maxBitLength; i++)
            {
                code = (code + codeCount[i - 1]) << 1;
                codes[i] = code;
            }
            return codes;
        }
        
        private int[] AssignCodes(int[] firstCodes)
        {
            var assignedCodes = new int[codeBitLengths.Length];
            for (var i = 0; i < codeBitLengths.Length; i++)
            {
                var l = codeBitLengths[i];
                if (l > 0)
                {
                    assignedCodes[i] = firstCodes[l];
                    firstCodes[l]++;
                }
            }
            return assignedCodes;
        }
        
        private void BuildDictionary()
        {
            dictionary = new Dictionary<int, List<int>>();

            for (var i = 0; i < codeBitLengths.Length; i++)
            {
                var len = codeBitLengths[i];
                if (len > 0)
                {
                    if (!dictionary.TryGetValue(len, out var list))
                    {
                        dictionary[len] = list = new List<int>();
                    }
                    list.Add(codes[i]);
                }
            }
        }
        
        public int Decode(BitStream stream)
        {
            var code = 0;
            for (var i = 1; i <= maxBitLength; i++)
            {
                code <<= 1;
                code |= stream.Read(1);

                var hasValue = dictionary.TryGetValue(i, out var list);
                var hasCode = hasValue && list.Contains(code);
                
                if (hasCode)
                {
                    return Array.IndexOf(codes, code);
                }
            }
            throw new CompressionException("Failed to decode huffman code");
        }
    }
}