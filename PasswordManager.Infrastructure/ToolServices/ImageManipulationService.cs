using PasswordManager.Domain.Exceptions;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Pbm;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Infrastructure.ToolServices;

internal sealed class ImageManipulationService
{
    private const int JPEG_COMPRESSION_FACTOR = 20;

    //jpeg, bmp, gif, pbm, png, tga, tiff, and webp
    private readonly Dictionary<string, IImageDecoder> _imageDecoders;
    private readonly IImageDecoder _imageDecoder;
    private readonly IImageEncoder _imageEncoder;
    private readonly IImageFormat _imageFormat;

    public ImageManipulationService()
    {
        _imageDecoders = new Dictionary<string, IImageDecoder>
        {
            { ".jpeg", new JpegDecoder() },
            { ".jpg", new JpegDecoder() },
            { ".bmp", new BmpDecoder() { RleSkippedPixelHandling = RleSkippedPixelHandling.Transparent } },
            { ".gif", new GifDecoder() { DecodingMode = SixLabors.ImageSharp.Metadata.FrameDecodingMode.First } },
            { ".pbm", new PbmDecoder() },
            { ".png", new PngDecoder() },
            { ".tga", new TgaDecoder() },
            { ".tiff", new TiffDecoder() { DecodingMode = SixLabors.ImageSharp.Metadata.FrameDecodingMode.First } },
            { ".tif", new TiffDecoder() { DecodingMode = SixLabors.ImageSharp.Metadata.FrameDecodingMode.First } },
            { ".webp", new WebpDecoder() }
        };

        _imageDecoder = new JpegDecoder();

        _imageEncoder = new JpegEncoder
        {
            Quality = JPEG_COMPRESSION_FACTOR
        };

        _imageFormat = JpegFormat.Instance;
    }

    public IImageEncoder ImageEncoder => _imageEncoder;

    public IImageDecoder ImageDecoder => _imageDecoder;

    public IImageDecoder GetImageDecoder(string fileExtension)
    {
        var supportedExtension = _imageDecoders.TryGetValue(fileExtension, out var decoder);

        if (!supportedExtension)
        {
            throw new UnsupportedFileExtensionException(fileExtension);
        }

        return decoder!;
    }

    public IImageFormat ImageFormat => _imageFormat;
}
