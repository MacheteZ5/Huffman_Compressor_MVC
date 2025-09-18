using Huffman_Compressor.Interfaces;
using Huffman_Compressor.Models;
using System.Xml.Linq;

namespace Huffman_Compressor.Services
{
    public class CompressionService : ICompression
    {
        private List<ListElement> elementsList = new List<ListElement>();
        private Dictionary<char, DictionaryValueElement> dictionary = new Dictionary<char, DictionaryValueElement>();
        public Dictionary<char, DictionaryValueElement> Dictionary
        {
            get { return this.dictionary; }
        }
        public void DictionaryCreation(List<byte> listadoBuffersArchivo)
        {
            foreach (byte bit in listadoBuffersArchivo)
            {
                var newDictionaryValueElement = new DictionaryValueElement();
                if (!dictionary.ContainsKey((char) bit))
                {
                    newDictionaryValueElement.Quantity = (1);
                }
                else
                {
                    var dictionaryValueElement = dictionary.GetValueOrDefault((char) bit);
                    dictionary.Remove((char) bit);
                    newDictionaryValueElement.Quantity = (dictionaryValueElement.Quantity + 1);
                }
                dictionary.Add((char) bit, newDictionaryValueElement);
            }
        }
        public void GenerateElementsList(long totalBytesLeidos)
        {
            var sortedDictionary = from entry in dictionary orderby entry.Value ascending select entry;
            foreach (var sortedDictionaryItem in sortedDictionary)
            {
                var dictionaryValueElementQuantity = Convert.ToDouble(sortedDictionaryItem.Value.Quantity);
                var listElementProbability = Convert.ToDouble((dictionaryValueElementQuantity / totalBytesLeidos));
                var listElement = new ListElement(sortedDictionaryItem.Key, listElementProbability);
                elementsList.Add(listElement);
            }
            elementsList.Sort();
        }
        public HuffmanTreeService CreateTree()
        {
            var repeticiones = elementsList.Count();
            var i = 1;
            while (repeticiones > 1)
            {
                var (treeNodeAuxiliar, izquierdo, derecho) = (new TreeNode(), new TreeNode(), new TreeNode());
                var nodeName = $"n{(i + 1)}";
                if (elementsList[0].HuffmanTreeNode is null && elementsList[1].HuffmanTreeNode is null)
                {
                    //hijo izquierdo
                    izquierdo.Caracter = (Convert.ToString(elementsList[0].Caracter));
                    izquierdo.Probabilidad = (elementsList[0].Probabilidad);
                    //hijo derecho
                    derecho.Caracter = (Convert.ToString(elementsList[1].Caracter));
                    derecho.Probabilidad = (elementsList[1].Probabilidad);
                }
                else
                {
                    if (elementsList[0].HuffmanTreeNode is not null && elementsList[1].HuffmanTreeNode is null)
                    {
                        //hijo izquierdo
                        izquierdo = elementsList[0].HuffmanTreeNode;
                        //hijo derecho
                        derecho.Caracter = (Convert.ToString(elementsList[1].Caracter));
                        derecho.Probabilidad = (elementsList[1].Probabilidad);
                    }
                    else
                    {
                        if (elementsList[0].HuffmanTreeNode is null && elementsList[1].HuffmanTreeNode is not null)
                        {
                            //hijo izquierdo
                            izquierdo.Caracter = (Convert.ToString(elementsList[0].Caracter));
                            izquierdo.Probabilidad = (elementsList[0].Probabilidad);
                            //hijo derecho
                            derecho = elementsList[1].HuffmanTreeNode;
                        }
                        else
                        {
                            //hijo izquierdo
                            izquierdo = elementsList[0].HuffmanTreeNode;
                            //hijo derecho
                            derecho = elementsList[1].HuffmanTreeNode;
                        }
                    }
                }
                elementsList.Remove(elementsList.First());
                elementsList.Remove(elementsList.First());
                treeNodeAuxiliar.Caracter = nodeName;
                treeNodeAuxiliar.AsignarNodosHijosNodoArbol(izquierdo, derecho);
                var newListElement = new ListElement(' ', treeNodeAuxiliar.Probabilidad);
                newListElement.HuffmanTreeNode = treeNodeAuxiliar;
                elementsList.Add(newListElement);
                elementsList.Sort();
                repeticiones = elementsList.Count;
                i++;
            }
            var huffmanTree = new HuffmanTreeService(elementsList[0].HuffmanTreeNode);
            return huffmanTree;
        }
    }
}
