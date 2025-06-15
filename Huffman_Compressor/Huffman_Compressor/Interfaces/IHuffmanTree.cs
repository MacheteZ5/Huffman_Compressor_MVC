using Huffman_Compressor.Models;

namespace Huffman_Compressor.Interfaces
{
    public interface IHuffmanTree
    {
        Dictionary<char, DictionaryValueElement> GeneratePrefixCode(TreeNode treeNode, Dictionary<char, DictionaryValueElement> dictionary, string prefixCode);
    }
}
