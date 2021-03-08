namespace ImageConverter.Png
{
    public static class CrcExtractor
    {
        private static readonly long[] table = new long[256];
        
        static CrcExtractor()
        {
            for (var n = 0; n < 256; n++)
            {
                var c = (long) n;
                for (var k = 0; k < 8; k++)
                {
                    if ((c & 1) > 0)
                    {
                        c = 0xedb88320L ^ (c >> 1);
                    }
                    else
                    {
                        c >>= 1;
                    }
                }
                table[n] = c;
            }
        }
        
        public static long Extract(byte[] buf)
        {
            return Update(0xffffffffL, buf) ^ 0xffffffffL;
        }
        
        private static long Update(long crc, byte[] buf)
        {
            var c = crc;
            int n;
            
            for (n = 0; n < buf.Length; n++) {
                c = table[(c ^ buf[n]) & 0xff] ^ (c >> 8);
            }
            return c;
        }
    }
}