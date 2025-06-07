using Huffman_Compressor.Models;
using System.Xml.Linq;

namespace Huffman_Compressor.Services
{
    public class Compression
    {
        private List<ListElement> elementsList = new List<ListElement>();
        private Dictionary<char, DictionaryValueElement> dictionary = new Dictionary<char, DictionaryValueElement>();
        public void CreacionDiccionario(List<byte> listadoBuffersArchivo)
        {
            foreach (byte bit in listadoBuffersArchivo)
            {
                var newDictionaryValueElement = new DictionaryValueElement();
                if (!dictionary.ContainsKey((char) bit))
                {
                    newDictionaryValueElement.AsignarQuantity(1);
                }
                else
                {
                    var dictionaryValueElement = dictionary.GetValueOrDefault((char) bit);
                    dictionary.Remove((char) bit);
                    newDictionaryValueElement.AsignarQuantity(dictionaryValueElement.RetornarQuantity() + 1);
                }
                dictionary.Add((char) bit, newDictionaryValueElement);
            }
        }
        public void GenerarListaElementos(long totalBytesLeidos)
        {
            var sortedDictionary = from entry in dictionary orderby entry.Value ascending select entry;
            foreach (var sortedDictionaryItem in sortedDictionary)
            {
                var dictionaryValueElementQuantity = Convert.ToDouble(sortedDictionaryItem.Value.RetornarQuantity());
                var listElementProbability = Convert.ToDouble((dictionaryValueElementQuantity / totalBytesLeidos));
                var listElement = new ListElement(sortedDictionaryItem.Key, listElementProbability);
                elementsList.Add(listElement);
            }
            elementsList.Sort();
        }
        public HuffmanTree GenerarArbol()
        {
            var repeticiones = elementsList.Count();
            var i = 1;
            while (repeticiones > 1)
            {
                var treeNodeAuxiliar = new TreeNode();
                var izquierdo = new TreeNode();
                var derecho = new TreeNode();
                var nodeName = $"n{(i + 1)}";
                if (elementsList[0].RetornarHuffmanTreeNode() is null && elementsList[1].RetornarHuffmanTreeNode() is null)
                {
                    //hijo izquierdo
                    izquierdo.AsignarCaracterNodoArbol(Convert.ToString(elementsList[0].RetornarCaracter()));
                    izquierdo.AsignarProbabilidadNodoArbol(elementsList[0].RetornarProbabilidad());
                    //hijo derecho
                    derecho.AsignarCaracterNodoArbol(Convert.ToString(elementsList[1].RetornarCaracter()));
                    derecho.AsignarProbabilidadNodoArbol(elementsList[1].RetornarProbabilidad());
                }
                else
                {
                    if (elementsList[0].RetornarHuffmanTreeNode() is not null && elementsList[1].RetornarHuffmanTreeNode() is null)
                    {
                        //hijo izquierdo
                        izquierdo = elementsList[0].RetornarHuffmanTreeNode();
                        //hijo derecho
                        derecho.AsignarCaracterNodoArbol(Convert.ToString(elementsList[1].RetornarCaracter()));
                        derecho.AsignarProbabilidadNodoArbol(elementsList[1].RetornarProbabilidad());
                    }
                    else
                    {
                        if (elementsList[0].RetornarHuffmanTreeNode() is null && elementsList[1].RetornarHuffmanTreeNode() is not null)
                        {
                            //hijo izquierdo
                            izquierdo.AsignarCaracterNodoArbol(Convert.ToString(elementsList[0].RetornarCaracter()));
                            izquierdo.AsignarProbabilidadNodoArbol(elementsList[0].RetornarProbabilidad());
                            //hijo derecho
                            derecho = elementsList[1].RetornarHuffmanTreeNode();
                        }
                        else
                        {
                            //hijo izquierdo
                            izquierdo = elementsList[0].RetornarHuffmanTreeNode();
                            //hijo derecho
                            derecho = elementsList[1].RetornarHuffmanTreeNode();
                        }
                    }
                }
                elementsList.Remove(elementsList.First());
                elementsList.Remove(elementsList.First());
                treeNodeAuxiliar.AsignarCaracterNodoArbol(nodeName);
                treeNodeAuxiliar.AsignarNodosHijosNodoArbol(izquierdo, derecho);
                var newListElement = new ListElement(' ', treeNodeAuxiliar.RetornarProbabilidad());
                newListElement.AsignarHuffmanTreeNode(treeNodeAuxiliar);
                elementsList.Add(newListElement);
                elementsList.Sort();
                repeticiones = elementsList.Count;
                i++;
            }
            var huffmanTree = new HuffmanTree(elementsList[0].RetornarHuffmanTreeNode());
            return huffmanTree;
        }
        public Dictionary<char, DictionaryValueElement> RetornarDiccionario()
        {
            return this.dictionary;
        }
    }
}
