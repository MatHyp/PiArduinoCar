using H264Sharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;

public class H264ImageProcessor : IDisposable
{
    private readonly H264Encoder _encoder;
    private readonly H264Decoder _decoder;
    private readonly int _width;
    private readonly int _height;
    private readonly byte[] _outBuffer;

    public H264ImageProcessor(int width, int height)
    {
        // Konfiguracja H264Sharp
        var config = ConverterConfig.Default;
        config.EnableSSE = 1;
        config.EnableNeon = 1;
        config.EnableAvx2 = 1;
        config.NumThreads = Environment.ProcessorCount;
        config.EnableCustomthreadPool = 1;
        Converter.SetConfig(config);

        _width = width;
        _height = height;
        _outBuffer = new byte[_width * _height * 3];

        _encoder = new H264Encoder();
        _decoder = new H264Decoder();

        _decoder.Initialize();
        _encoder.Initialize(_width, _height, 200_000_000, 30, ConfigType.CameraCaptureAdvanced);
    }

    public void ProcessImage(string inputPath, string outputPrefix, int iterations = 2)
    {
        using var image = Image.Load<Rgb24>(inputPath);

        if (image.Width != _width || image.Height != _height)
            throw new ArgumentException("Rozdzielczość obrazu nie pasuje do ustawień procesora.");

        // Pobranie pikseli w formacie RGB
        byte[] data = new byte[_width * _height * 3];
        image.CopyPixelDataTo(data);

        var rgbIn = new RgbImage(ImageFormat.Rgb, _width, _height, data);
        var rgbOut = new RgbImage(ImageFormat.Rgb, _width, _height, _outBuffer);

        for (int j = 0; j < iterations; j++)
        {
            if (!_encoder.Encode(rgbIn, out EncodedData[] encodedFrames))
            {
                Console.WriteLine("skipped");
                continue;
            }

            foreach (var encoded in encodedFrames)
            {
                if (_decoder.Decode(encoded, noDelay: true, out DecodingState _, ref rgbOut))
                {
                    Console.WriteLine($"F:{encoded.FrameType} size: {encoded.Length}");

                    using var outputImage = Image.LoadPixelData<Bgr24>(_outBuffer, _width, _height);
                    outputImage.Save($"{outputPrefix}_{j}.jpg");
                }
            }
        }
    }

    public void Dispose()
    {
        _encoder.Dispose();
        _decoder.Dispose();
    }
}
