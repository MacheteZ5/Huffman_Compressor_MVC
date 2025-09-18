using Huffman_Compressor.Interfaces;
using Huffman_Compressor.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Collections.Generic;
using System.Text;

namespace Huffman_Compressor.Services
{
    public class DecompressionService : IDecompression
    {
        public string DecompressFile(List<byte> ASCII, Dictionary<string, char> dictionary)
        {
            var text = new StringBuilder();
            var prefixCode = string.Empty;
            var binary = string.Empty;
            foreach (byte bit in ASCII)
            {
                binary = ConvertByteToString(bit);
                foreach (char character in binary)
                {
                    prefixCode += character;
                    if (dictionary.ContainsKey(prefixCode))
                    {
                        text.Append(dictionary.GetValueOrDefault(prefixCode)); ;
                        prefixCode = string.Empty;
                    }
                }
            }
            return text.ToString();
        }
        private string ConvertByteToString(byte bit)
        {
            var binary = string.Empty;
            while (bit>0)
            {
                binary = ((bit % 2) != 0) ? $"1{binary}" : $"0{binary}";
                bit /= 2;
            }
            if (binary.Length < 8)
            {
                binary = binary.PadLeft(8, '0');
            }
            return binary;
        }
    }
}
