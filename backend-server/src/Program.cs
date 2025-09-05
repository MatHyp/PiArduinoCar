using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using H264Sharp;
using h264;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SD = System.Drawing;




void StartTcpDecode()
{
    int port = 12345;
    TcpListener listener = new TcpListener(IPAddress.Any, port);
    listener.Start();
    TcpClient client = listener.AcceptTcpClient();

    NetworkStream stream = client.GetStream();
    byte[] buffer = new byte[10 * 1024 * 1024]; // buffer large enough for one frame
    int bufferOffset = 0;

    var decoder = new H264Decoder();
    decoder.Initialize();

    RgbImage rgbImage = new RgbImage(ImageFormat.Rgb, 936, 1920); // Create an RgbImage

    int frameCount = 0;
    while (true)
    {

        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        if (bytesRead == 0) break;
        unsafe
        {
            bool result = decoder.Decode(buffer, 0, bytesRead, noDelay: false, out DecodingState state, ref rgbImage);
            // Console.WriteLine($"Decoded frame {state}");

            if (result)
            {
                // Console.WriteLine($"Decoded frame {state}");
                // // You can process the YUV frame here
                // byte[] pixels = rgbImage.GetBytes();
                // using var image = Image.LoadPixelData<Rgb24>(pixels, 936, 1920);

                // string filename = $"images/frame_{frameCount}.jpg";
                // image.Save(filename);

                // frameCount++;
                byte[] pixels = rgbImage.GetBytes();

                // Convert RGB24 byte array to Bitmap
                Bitmap bmp = new Bitmap(936, 1920, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                var bmpData = bmp.LockBits(new SD.Rectangle(0, 0, 936, 1920),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    bmp.PixelFormat);

                System.Runtime.InteropServices.Marshal.Copy(pixels, 0, bmpData.Scan0, pixels.Length);
                bmp.UnlockBits(bmpData);
            }
            bufferOffset = 0;
        }
    }

}




