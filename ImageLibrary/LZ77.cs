namespace ImageConverter
{
    using System;
    using System.Collections.Generic;

    public class LZ77
    {
        private static readonly byte[] BaseBits = {
            0, 0, 0, 0, 0, 0, 0, 0, //257 - 264
            1, 1, 1, 1, //265 - 268
            2, 2, 2, 2, //269 - 272 
            3, 3, 3, 3, //273 - 276
            4, 4, 4, 4, //277 - 280
            5, 5, 5, 5, //281 - 284
            0           //285
        };
        
        private static readonly int[] BaseLengths = {
            3, 4, 5, 6, 7, 8, 9, 10, //257 - 264
            11, 13, 15, 17,          //265 - 268
            19, 23, 27, 31,          //269 - 273 
            35, 43, 51, 59,          //274 - 276
            67, 83, 99, 115,         //278 - 280
            131, 163, 195, 227,      //281 - 284
            258                      //285
        };
        
        private static readonly int[] DistBits = {
            0, 0, 0, 0, //0-3
            1, 1,       //4-5
            2, 2,       //6-7
            3, 3,       //8-9
            4, 4,       //10-11
            5, 5,       //12-13
            6, 6,       //14-15
            7, 7,       //16-17
            8, 8,       //18-19
            9, 9,       //20-21
            10, 10,     //22-23
            11, 11,     //24-25
            12, 12,     //26-27
            13, 13,     //28-29
            0, 0        //30-31 error, they shouldn't occur
        };
        
        private static readonly int[] DistBases = {
            1, 2, 3, 4,    //0-3
            5, 7,          //4-5
            9, 13,         //6-7
            17, 25,        //8-9
            33, 49,        //10-11
            65, 97,        //12-13
            129, 193,      //14-15
            257, 385,      //16-17
            513, 769,      //18-19
            1025, 1537,    //20-21
            2049, 3073,    //22-23
            4097, 6145,    //24-25
            8193, 12289,   //26-27
            16385, 24577,  //28-29
            0, 0           //30-31, error, shouldn't occur
        };
        
        public void Decompress(BitStream stream, Huffman literalTree, Huffman distanceTree, List<byte> decompressed)
        {
            while (true)
            {
                var value = literalTree.Decode(stream);
                if (value == 256) break;
                if (value <= 255)
                {
                    decompressed.Add((byte) value);
                    continue;
                }

                var baseIndex = value - 257;
                var duplicateLength = BaseLengths[baseIndex] + stream.ReadReverse(BaseBits[baseIndex]);

                var distanceIndex = distanceTree.Decode(stream);
                var distanceLength = DistBases[distanceIndex] + stream.ReadReverse(DistBits[distanceIndex]);

                var index = decompressed.Count - distanceLength;
                for (var i = 0; i < duplicateLength; i++)
                {
                    decompressed.Add(decompressed[index + i]);
                }
            }
        }
    }
}