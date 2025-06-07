namespace Huffman_Compressor.Models
{
    public class ListElement(char caracter, double probabilidad) : IComparable
    {
        private char caracter = caracter;
        private double probabilidad = probabilidad;
        private TreeNode huffmanTreeNode = new TreeNode();

        public void AsignarCaracter(char caracter)
        {
            this.caracter = caracter;
        }
        public void AsignarProbabilidad(double probabilidad)
        {
            this.probabilidad = probabilidad;
        }
        public void AsignarHuffmanTreeNode(TreeNode huffmanTreeNode)
        {
            this.huffmanTreeNode = huffmanTreeNode;
        }
        public int CompareTo(Object obj)
        {
            var compareToObj = obj as ListElement;
            return this.probabilidad.CompareTo(compareToObj.probabilidad);
        }
        public char RetornarCaracter()
        {
            return this.caracter;
        }
        public double RetornarProbabilidad()
        {
            return this.probabilidad;
        }
        public TreeNode RetornarHuffmanTreeNode()
        {
            return this.huffmanTreeNode;
        }
    }
}
