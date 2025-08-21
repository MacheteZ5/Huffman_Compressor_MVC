using Huffman_Compressor.Models;

namespace Huffman_Compressor.Interfaces
{
    public interface IFile
    {
        Task FileReadingCompression(IFormFile postedFile, long maxBufferSize);
        void FileWritingCompression(string rutaArchivo, string caracteresYSusPrefijos, Dictionary<char, DictionaryValueElement> diccionario);
        Dictionary<string, char> FileReadingDecompression(IFormFile postedFile);
        void FileWritingDecompression(string decompressedFilePath, string text, int maxBufferSize);
    }
}
