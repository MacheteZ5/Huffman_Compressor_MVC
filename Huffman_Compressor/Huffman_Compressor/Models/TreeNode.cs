namespace Huffman_Compressor.Models
{
    public class TreeNode()
    {
        private string caracter = string.Empty;
        private double probabilidad = 0;
        private TreeNode? hijoIzquierdo = null;
        private TreeNode? hijoDerecho = null;

        public void AsignarCaracterNodoArbol(string caracter)
        {
            this.caracter = caracter;
        }
        public void AsignarProbabilidadNodoArbol(double probabilidad)
        {
            this.probabilidad = probabilidad;
        }
        public void AsignarNodosHijosNodoArbol(TreeNode hijoIzquierdo, TreeNode hijoDerecho)
        {
            this.hijoIzquierdo = hijoIzquierdo;
            this.hijoDerecho = hijoDerecho;
            AsignarProbabilidadNodoArbol(hijoIzquierdo.probabilidad + hijoDerecho.probabilidad);
        }
        public string RetornarCaracter()
        {
            return this.caracter;
        }
        public double RetornarProbabilidad()
        {
            return this.probabilidad;
        }
        public TreeNode? RetornarHijoIzquierdo()
        {
            return hijoIzquierdo;
        }
        public TreeNode? RetornarHijoDerecho()
        {
            return hijoDerecho;
        }
    }
}
