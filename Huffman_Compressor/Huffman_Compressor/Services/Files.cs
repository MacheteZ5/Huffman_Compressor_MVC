using Huffman_Compressor.Interfaces;
using Huffman_Compressor.Models;
using Humanizer.Bytes;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System.Text;

namespace Huffman_Compressor.Services
{
    public class Files : IFile
    {
        private long totalBytesLeidos = 0;
        private List<byte> listadoBytesArchivo = new List<byte>();
        public long TotalBytesLeidos
        {
            get { return this.totalBytesLeidos; }
        }
        public List<byte> ListadoBytesArchivo
        {
            get { return this.listadoBytesArchivo; }
        }
        public async Task FileReadingCompression(IFormFile postedFile, long maxBufferSize)
        {
            var buffer = new byte[maxBufferSize];
            var bytesLeidos = 0;
            using (var stream = postedFile.OpenReadStream())
            {
                while ((bytesLeidos = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    for(int i = 0; i< bytesLeidos; i++)
                    {
                        listadoBytesArchivo.Add(buffer[i]);
                    }
                }
            }
            totalBytesLeidos = listadoBytesArchivo.Count();
        }
        public void FileWritingCompression(string rutaArchivo, string caracteresYSusPrefijos, Dictionary<char, DictionaryValueElement> diccionario)
        {
            var longitudCadena = caracteresYSusPrefijos.Length;
            var buffer = new byte[longitudCadena + 2];
            for (int i = 0; i < longitudCadena; i++)
            {
                buffer[i] = Convert.ToByte(caracteresYSusPrefijos[i]);
            }
            buffer[longitudCadena] = Convert.ToByte('-');
            buffer[longitudCadena + 1] = Convert.ToByte('-');
            var conteo = 0;
            File.Delete(rutaArchivo);
            using (var writeStream = new FileStream(rutaArchivo, FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    for (int j = 0; j < buffer.Count(); j++)
                    {
                        if (j == longitudCadena)
                        {
                            writer.Write("\r\n");
                            writer.Write(buffer[j]);
                            writer.Write(buffer[j + 1]);
                            writer.Write("\r\n");
                            break;
                        }
                        if (buffer[j + 1] == 124)
                        {
                            if (conteo != 0)
                            {
                                writer.Write("\r\n");
                                writer.Write(buffer[j]);
                            }
                            else
                            {
                                writer.Write(buffer[j]);
                                conteo++;
                            }
                        }
                        else
                        {
                            writer.Write(buffer[j]);
                        }
                    }
                }
            }
            using (var writeStream = new FileStream(rutaArchivo, FileMode.Open))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    var bytebuffer = new byte[500];
                    var cadena = new List<char>();
                    var cantidadbuffer = 0;
                    foreach (byte bit in listadoBytesArchivo)
                    {
                        var separacion = new DictionaryValueElement();
                        separacion = diccionario.GetValueOrDefault((char) bit);
                        foreach (char caracter in separacion.PrefixCode)
                        {
                            cadena.Add(caracter);
                        }
                    }
                    var (binario, pref) = (string.Empty, string.Empty);
                    var DECABYTE = new byte();
                    foreach (char character in cadena)
                    {
                        if (binario.Count() == 8)
                        {
                            DECABYTE = Convert.ToByte(Convert.ToInt32(binario, 2));
                            bytebuffer[cantidadbuffer] = DECABYTE;
                            cantidadbuffer++;
                            binario = string.Empty;
                            binario += character;
                        }
                        else
                        {
                            binario += character;
                        }
                        if (cantidadbuffer == 500)
                        {
                            writer.Seek(0, SeekOrigin.End);
                            writer.Write(bytebuffer);
                            cantidadbuffer = 0;
                            bytebuffer = new byte[500];
                        }
                    }
                    if (binario != string.Empty)
                    {
                        while (binario.Count() != 8)
                        {
                            binario += "0";
                        }
                        DECABYTE = Convert.ToByte(Convert.ToInt32(binario, 2));
                        bytebuffer[cantidadbuffer] = DECABYTE;
                        writer.Seek(0, SeekOrigin.End);
                        writer.Write(bytebuffer);
                    }
                }
            }
        }
        public Dictionary<string, char> FileReadingDecompression(IFormFile postedFile)
        {
            var dictionary = new Dictionary<string, char>();
            using (var stream = postedFile.OpenReadStream())
            {
                using (var reader = new BinaryReader(stream))
                {
                    var conteo0 = 0;
                    var prefijos = string.Empty;
                    var caracter = ' ';
                    var byteBuffer = new byte[10000];
                    var (encontrado, separador, demasiado) = (false, false, false);
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        byteBuffer = reader.ReadBytes(10000);
                        if (demasiado == false)
                        {
                            for (int i = 0; i < byteBuffer.Count(); i++)
                            {
                                if (separador != true)
                                {
                                    if (byteBuffer[i] == 45)
                                    {
                                        if (byteBuffer[i + 1] == 45)
                                        {
                                            separador = true;
                                            i = i + 2;
                                        }
                                    }
                                    if (encontrado == false)
                                    {
                                        if (byteBuffer[i + 1] == 124)
                                        {
                                            caracter = (char)byteBuffer[i];
                                            encontrado = true;
                                            i++;
                                        }
                                    }
                                    else
                                    {
                                        if ((byteBuffer[i] != 13) && (byteBuffer[i] != 2))
                                        {
                                            prefijos = prefijos + (char)byteBuffer[i];
                                        }
                                        else
                                        {
                                            var prefijo = new DictionaryValueElement();
                                            prefijo.PrefixCode = prefijos;
                                            i++;
                                            if (prefijo.PrefixCode[0] == '|')
                                            {
                                                var prueba = string.Empty;
                                                for (int j = 1; j < prefijo.PrefixCode.Length; j++)
                                                {
                                                    prueba = prueba + prefijo.PrefixCode[j];
                                                }
                                                prefijo.PrefixCode = prueba;
                                            }
                                            dictionary.Add(prefijo.PrefixCode, caracter);
                                            encontrado = false;
                                            prefijos = string.Empty;
                                        }
                                    }
                                }
                                else
                                {
                                    if (byteBuffer[i] == 0)
                                    {
                                        conteo0++;
                                    }
                                    listadoBytesArchivo.Add(byteBuffer[i]);
                                    if (conteo0 == 10000)
                                    {
                                        demasiado = true;
                                    }
                                }
                                if (demasiado == true)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }

                }
            }
            for (int i = 0; i < 2; i++)
            {
                listadoBytesArchivo.Remove(listadoBytesArchivo[0]);
            }
            return dictionary;
        }
        public void FileWritingDecompression(string decompressedFilePath, string text, int maxBufferSize)
        {
            File.Delete(decompressedFilePath);
            using (var writeStream = new FileStream(decompressedFilePath, FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    var quantity = 0;
                    var start = true;
                    var finalBuffer = new byte[maxBufferSize];
                    var shortText = new StringBuilder();
                    foreach (char character in text)
                    {
                        shortText.Append(character); 
                        if(shortText.Length == maxBufferSize)
                        {
                            foreach (char shortCaracter in shortText.ToString())
                            {
                                finalBuffer[quantity] = Convert.ToByte(shortCaracter);
                                quantity++;
                            }
                            if (start)
                            {
                                writer.Seek(0, SeekOrigin.End);
                                start = false;
                            }
                            writer.Write(finalBuffer);
                            finalBuffer = new byte[maxBufferSize];
                            quantity = 0;
                            shortText.Clear();
                        }
                    }
                    if(shortText.Length > 0)
                    {
                        finalBuffer = new byte[shortText.Length];
                        foreach (char shortCaracter in shortText.ToString())
                        {
                            finalBuffer[quantity] = Convert.ToByte(shortCaracter);
                            quantity++;
                        }
                        writer.Write(finalBuffer);
                    }
                }
            }
        }
    } 
}