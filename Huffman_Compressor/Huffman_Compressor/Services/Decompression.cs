using Huffman_Compressor.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Collections.Generic;
using System.Text;

namespace Huffman_Compressor.Services
{
    public class Decompression
    {
        public string DescomprimirArchivo(List<byte> ASCII, Dictionary<string, char> diccionario)
        {
            var text = new StringBuilder();
            var prefixCode = string.Empty;
            foreach (byte bit in ASCII)
            {
                var binary = string.Empty;
                binary += ConvertByteToString(bit);
                foreach (char character in binary)
                {
                    prefixCode += character;
                    if (diccionario.ContainsKey(prefixCode))
                    {
                        text.Append(diccionario.GetValueOrDefault(prefixCode)); ;
                        prefixCode = string.Empty;
                    }
                }
            }
            return text.ToString();
        }
        private string ConvertByteToString(byte bit)
        {
            var binary = string.Empty;
            while (true)
            {
                binary = ((bit % 2) != 0) ? $"1{binary}" : $"0{binary}";
                bit /= 2;
                if (bit <= 0)
                {
                    break;
                }
            }
            while (binary.Length < 8)
            {
                binary = $"0{binary}";
            }
            return binary;
        }
    }
}
