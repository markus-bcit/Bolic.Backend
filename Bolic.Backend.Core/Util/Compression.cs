namespace Bolic.Backend.Core.Util;

public class Compressor
{
    public static Stream Decompress(Stream body) =>
         new GZipStream(body, CompressionMode.Decompress);
}
