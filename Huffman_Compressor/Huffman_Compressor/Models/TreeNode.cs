namespace Huffman_Compressor.Models
{
    public class TreeNode()
    {
        private string caracter = string.Empty;
        private double probabilidad = 0;
        private TreeNode? hijoIzquierdo = null;
        private TreeNode? hijoDerecho = null;

        public string Caracter
        {
            get { return this.caracter; }
            set { this.caracter = value; }
        }
        public double Probabilidad
        {
            get { return this.probabilidad; }
            set { this.probabilidad = value; }
        }
        public TreeNode? HijoIzquierdo
        {
            get { return this.hijoIzquierdo; }
        }
        public TreeNode? HijoDerecho
        {
            get { return this.hijoDerecho; }
        }
        public void AsignarNodosHijosNodoArbol(TreeNode hijoIzquierdo, TreeNode hijoDerecho)
        {
            this.hijoIzquierdo = hijoIzquierdo;
            this.hijoDerecho = hijoDerecho;
            Probabilidad = (hijoIzquierdo.Probabilidad + hijoDerecho.Probabilidad);
        }
    }
}
