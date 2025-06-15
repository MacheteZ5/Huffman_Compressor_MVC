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

        public TreeNode Root
        {
            get { return this.root; }
        }

        public string CaracteresYSusPrefijos
        {
            get { return this.caracteresYSusPrefijos; }
        }

        public Dictionary<char, DictionaryValueElement> GenerarPrefixCode(TreeNode treeNode, Dictionary<char, DictionaryValueElement> dictionary, string prefixCode)
        {
            if (treeNode is not null)
            {
                dictionary = GenerarPrefixCode(treeNode.HijoIzquierdo, dictionary, $"{prefixCode}0");
                if (treeNode.HijoDerecho is null && treeNode.HijoIzquierdo is null)
                {
                    if (dictionary.ContainsKey(Convert.ToChar(treeNode.Caracter)))
                    {
                        var dictionaryValueElementcantidad = new DictionaryValueElement();
                        dictionaryValueElementcantidad.PrefixCode = prefixCode;
                        dictionary.Remove(Convert.ToChar(treeNode.Caracter));
                        dictionary.Add(Convert.ToChar(treeNode.Caracter), dictionaryValueElementcantidad);
                        caracteresYSusPrefijos += $"{treeNode.Caracter}|{prefixCode}";
                    }
                }
                dictionary = GenerarPrefixCode(treeNode.HijoDerecho, dictionary, $"{prefixCode}1");
            }
            return dictionary;
        }
    }
}
