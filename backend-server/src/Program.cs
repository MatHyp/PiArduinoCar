using H264Sharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;

// H264Sharp configuration
var config = ConverterConfig.Default;
config.EnableSSE = 1;
config.EnableNeon = 1;
config.EnableAvx2 = 1;
config.NumThreads = Environment.ProcessorCount;
config.EnableCustomthreadPool = 1;
Converter.SetConfig(config);

// Create H264 decoder
using var decoder = new H264Decoder();

// Try explicit configuration

decoder.Initialize();

// UDP setup
const int port = 27000;
using var udpClient = new UdpClient(port);
Console.WriteLine($"Listening for H.264 stream on port {port}...");

int frameCount = 0;
var remoteEP = new IPEndPoint(IPAddress.Any, 0);
var frameBuffer = new List<byte>();

// Diagnostic counters
int packetCount = 0;
int totalBytes = 0;

while (true)
{
    try
    {
        byte[] packet = udpClient.Receive(ref remoteEP);
        packetCount++;
        totalBytes += packet.Length;

        Console.WriteLine($"Packet #{packetCount}: {packet.Length} bytes | Total: {totalBytes} bytes");

        bool isNewFrame = packet.Length >= 4 &&
            ((packet[0] == 0 && packet[1] == 0 && packet[2] == 0 && packet[3] == 1) ||
             (packet[0] == 0 && packet[1] == 0 && packet[2] == 1));

        // Modified assembly logic
        if (isNewFrame && frameBuffer.Count > 0)
        {
            Console.WriteLine($"Processing frame with {frameBuffer.Count} bytes");
            ProcessFrame(decoder, frameBuffer.ToArray(), ref frameCount);
            frameBuffer.Clear();
        }

        frameBuffer.AddRange(packet);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

void ProcessFrame(H264Decoder decoder, byte[] frameData, ref int frameCount)
{
    try
    {
        // Diagnostic: Show frame header
        string header = frameData.Length >= 8
            ? BitConverter.ToString(frameData, 0, 8).Replace("-", " ")
            : "N/A";
        Console.WriteLine($"Frame {frameCount} header: {header}");

        // Create output image (temporary dimensions)
        var rgbOut = new RgbImage(ImageFormat.Bgr, 1, 1);

        // Save raw data for inspection
        System.IO.File.WriteAllBytes($"raw_{frameCount}.h264", frameData);
        Console.WriteLine($"Saved raw_{frameCount}.h264 ({frameData.Length} bytes)");

        // Try decoding
        if (decoder.Decode(frameData, 0, frameData.Length, true, out DecodingState ds, ref rgbOut))
        {
            Console.WriteLine($"Decoding successful! {rgbOut.Width}x{rgbOut.Height}");
            // ... rest of image saving code ...
        }
        else
        {
            Console.WriteLine($"Decoding failed. State: ");
        }

        frameCount++;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Processing error: {ex.Message}");
    }
}