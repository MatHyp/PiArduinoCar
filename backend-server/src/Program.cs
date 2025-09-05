

using H264Sharp;
using h264;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

string inputPath = "input.jpg";
using var image = Image.Load<Rgb24>(inputPath); 

byte[] pixels = new byte[image.Width * image.Height * 3];
image.CopyPixelDataTo(pixels);

var rgbIn = new RgbImage(ImageFormat.Rgb, image.Width, image.Height, pixels);
var h264processor = new H264ImageProcessor(1920, 1080);

EncodedData[] encodedFrames = h264processor.encodedImage(rgbIn);

h264processor.decodeImage(encodedFrames);

void CheckH264Format(EncodedData[] frames)
{
    foreach (var frame in frames)
    {
        byte[] bytes = frame.GetBytes();
        for (int i = 0; i < bytes.Length - 4; i++)
        {
            // Look for 3- or 4-byte start code
            if ((bytes[i] == 0x00 && bytes[i + 1] == 0x00 && bytes[i + 2] == 0x01) ||
                (bytes[i] == 0x00 && bytes[i + 1] == 0x00 && bytes[i + 2] == 0x00 && bytes[i + 3] == 0x01))
            {
                int nalIndex = (bytes[i + 2] == 0x01) ? i + 3 : i + 4;
                byte nalHeader = bytes[nalIndex];
                int nalType = nalHeader & 0x1F; // last 5 bits
                Console.WriteLine($"NAL unit found at {i}, type: {nalType}");
            }
        }
    }
}
