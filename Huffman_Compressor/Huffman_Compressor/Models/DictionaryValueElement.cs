namespace Huffman_Compressor.Models
{
    public class DictionaryValueElement : IComparable
    {
        private int quantity = 0;
        private string prefixCode = "";
        
        public void AsignarQuantity(int quantity)
        {
            this.quantity = quantity;
        }
        public void AsignarPrefixCode(string prefixCode)
        {
            this.prefixCode = prefixCode;
        }
        public int CompareTo(object obj)
        {
            var compareToObj = obj as DictionaryValueElement;
            return this.quantity.CompareTo(compareToObj.quantity);
        }
        public int RetornarQuantity()
        {
            return quantity;
        }
        public string RetornarPrefixCode()
        {
            return prefixCode;
        }
    }
}
