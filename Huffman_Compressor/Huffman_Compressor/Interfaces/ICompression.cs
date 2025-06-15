using Huffman_Compressor.Models;
using Huffman_Compressor.Services;

namespace Huffman_Compressor.Interfaces
{
    public interface ICompression
    {
        void DictionaryCreation(List<byte> listadoBuffersArchivo);
        void GenerateElementsList(long totalBytesLeidos);
        HuffmanTree CreateTree();
    }
}
