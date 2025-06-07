using Huffman_Compressor.Models;

namespace Huffman_Compressor.Services
{
    public class HuffmanTree
    {
        private TreeNode root = new TreeNode();
        private string caracteresYSusPrefijos = "";
        public HuffmanTree(TreeNode root)
        {
            this.root = root;
        }
        public Dictionary<char, DictionaryValueElement> GenerarPrefixCode(TreeNode treeNode, Dictionary<char, DictionaryValueElement> dictionary, string prefixCode)
        {
            if (treeNode is not null)
            {
                dictionary = GenerarPrefixCode(treeNode.RetornarHijoIzquierdo(), dictionary, $"{prefixCode}0");
                if (treeNode.RetornarHijoDerecho() is null && treeNode.RetornarHijoIzquierdo() is null)
                {
                    if (dictionary.ContainsKey(Convert.ToChar(treeNode.RetornarCaracter())))
                    {
                        var dictionaryValueElementcantidad = new DictionaryValueElement();
                        dictionaryValueElementcantidad.AsignarPrefixCode(prefixCode);
                        dictionary.Remove(Convert.ToChar(treeNode.RetornarCaracter()));
                        dictionary.Add(Convert.ToChar(treeNode.RetornarCaracter()), dictionaryValueElementcantidad);
                        caracteresYSusPrefijos += $"{treeNode.RetornarCaracter()}|{prefixCode}";
                    }
                }
                dictionary = GenerarPrefixCode(treeNode.RetornarHijoDerecho(), dictionary, $"{prefixCode}1");
            }
            return dictionary;
        }
        public TreeNode RetornarRaiz()
        {
            return this.root;
        }
        public string RetornarCaracteresPrefijos()
        {
            return this.caracteresYSusPrefijos;
        }
    }
}
