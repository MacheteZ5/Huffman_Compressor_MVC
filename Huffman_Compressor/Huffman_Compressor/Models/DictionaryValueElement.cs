namespace Huffman_Compressor.Models
{
    public class DictionaryValueElement : IComparable
    {
        private int quantity = 0;
        private string prefixCode = string.Empty;
        
        public int Quantity
        {
            get { return this.quantity; }
            set { this.quantity = value; }
        }
        public string PrefixCode
        {
            get { return this.prefixCode; }
            set { this.prefixCode = value; }
        }

        public int CompareTo(object obj)
        {
            var compareToObj = obj as DictionaryValueElement;
            return this.quantity.CompareTo(compareToObj.quantity);
        }
    }
}
