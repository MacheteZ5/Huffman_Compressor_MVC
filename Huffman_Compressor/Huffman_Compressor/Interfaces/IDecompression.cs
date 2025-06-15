namespace Huffman_Compressor.Interfaces
{
    public interface IDecompression
    {
        string DecompressFile(List<byte> ASCII, Dictionary<string, char> dictionary);
    }
}
