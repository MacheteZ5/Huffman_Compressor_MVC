namespace Huffman_Compressor.Models
{
    public class ListElement(char caracter, double probabilidad) : IComparable
    {
        private char caracter = caracter;
        private double probabilidad = probabilidad;
        private TreeNode huffmanTreeNode = null;

        public char Caracter
        {
            get { return this.caracter; }
            set { this.caracter = value; }
        }
        public double Probabilidad
        {
            get { return this.probabilidad; }
            set { this.probabilidad = value; }
        }
        public TreeNode HuffmanTreeNode
        {
            get { return this.huffmanTreeNode; }
            set { this.huffmanTreeNode = value; }
        }
        public int CompareTo(Object obj)
        {
            var compareToObj = obj as ListElement;
            return this.probabilidad.CompareTo(compareToObj.probabilidad);
        }
    }
}
