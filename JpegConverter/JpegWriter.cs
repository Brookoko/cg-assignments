namespace JpegConverter
{
    using ImageConverter;
    using Jpeg;
    using Jpeg.Coding.Dct;
    using Jpeg.FileStructure;
    using Jpeg.FileStructure.Markers;
    using Jpeg.FileStructure.Segments;
    using Jpeg.FileStructure.Structures;
    using Jpeg.Structures;

    public class JpegWriter
    {
        public void Process(Image image)
        {
            var quantizationTablesProvider = new DefaultQuantizationTablesProvider();
            var luminanceTable = quantizationTablesProvider
                .GetLuminanceTable(QuantizationTablePrecision.Bit8, 0, 0.50f);
            var chrominanceTable = quantizationTablesProvider
                .GetChrominanceTable(QuantizationTablePrecision.Bit8, 1, 0.50f); 
            
            // convert to YCbCr
            var yCbCrConverted = new YCbCrColor[image.Height, image.Width];
            for (var i = 0; i < image.Height; i++)
            {
                for (var j = 0; j < image.Width; j++)
                {
                    yCbCrConverted[i, j] = (YCbCrColor) image[i, j];
                }
            }
            
            // Split Y Cb and Cr components
            var ySamples = new byte[image.Height, image.Width];
            var cbSamples = new byte[image.Height, image.Width];
            var crSamples = new byte[image.Height, image.Width];
            for (var i = 0; i < image.Height; i++)
            {
                for (var j = 0; j < image.Width; j++)
                {
                    ySamples[i, j] = yCbCrConverted[i, j].Y;
                    cbSamples[i, j] = yCbCrConverted[i, j].Cb;
                    crSamples[i, j] = yCbCrConverted[i, j].Cr;
                }
            }

            var jpegImage = new JpegImage()
            {
                StartOfImage = new SoiSegment(),
                ImageFrame = new Frame(),
                EndOfImage = new EoiSegment()
            };

            var quantizationTablesSegment = new QuantizationTableSegment()
            {
                QuantizationTables = new QuantizationTable[2]
                {
                    luminanceTable,
                    chrominanceTable
                }
            };

            jpegImage.ImageFrame.QuantizationTables = quantizationTablesSegment;
            // todo: huffman tables segment

            // ProcessHeader
            var frameHeader = new FrameHeader();
            jpegImage.ImageFrame.Header = frameHeader;
            
            // Quantization tables: 0 for luma and 1 for chroma
            var frameCsp = new FrameComponentSpecificationParameters[3]
                {
                    new FrameComponentSpecificationParameters(0, ByteHalf.SelectRightBits(1),ByteHalf.SelectRightBits(1), 0),
                    new FrameComponentSpecificationParameters(1, ByteHalf.SelectRightBits(1),ByteHalf.SelectRightBits(1), 1),
                    new FrameComponentSpecificationParameters(2, ByteHalf.SelectRightBits(1),ByteHalf.SelectRightBits(1), 1)
                };
            frameHeader.SegmentMarker = new JpegMarker(0xC0); // For sequential DCT
            frameHeader.P = 8; // todo: imagePrecision
            frameHeader.Y = (ushort) image.Height;
            frameHeader.X = (ushort) image.Width;
            frameHeader.Nf = (byte)frameCsp.Length; // todo: components per frame amount
            frameHeader.FrameCspArray = frameCsp;

            // Define frame scan
            var singleScan = new Scan();
            var scanHeader = new ScanHeader();
            singleScan.Header = scanHeader;

            // AC/DC tables: 0 for luma and 1 for chroma
            var scanCsp = new ScanComponentSpecificationParameters[3]
            {
                new ScanComponentSpecificationParameters(0, ByteHalf.SelectRightBits(0), ByteHalf.SelectRightBits(0)),
                new ScanComponentSpecificationParameters(1, ByteHalf.SelectRightBits(1), ByteHalf.SelectRightBits(1)),
                new ScanComponentSpecificationParameters(2, ByteHalf.SelectRightBits(1), ByteHalf.SelectRightBits(1)),
            };
            scanHeader.Ns = 3; // todo: components per scan amount
            scanHeader.ScanCspArray = scanCsp;
            scanHeader.Ss = 0; // For sequential DCT mode of operations
            scanHeader.Se = 63; // For sequential DCT mode of operations
            scanHeader.Ah = ByteHalf.SelectRightBits(0);
            scanHeader.Al = ByteHalf.SelectRightBits(0);

            // No restart intervals for prototype purpose
            var scanCodedSegment = new CodedSegment();
            
            // encode data units and write them to ScanCodedSegment
            
        }
    }
}