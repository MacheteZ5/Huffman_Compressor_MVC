using Huffman_Compressor.Models;
using Humanizer.Bytes;
using Microsoft.AspNetCore.Components.Forms;
using System.IO;
using System.Text;

namespace Huffman_Compressor.Services
{
    public class Files
    {
        private long totalBytesLeidos = 0;
        private List<byte> listadoBytesArchivo = new List<byte>();
        public async Task LecturaArchivoCompresion(IFormFile postedFile, long maxBufferSize)
        {
            var buffer = new byte[maxBufferSize];
            var bytesLeidos = 0;
            using (var stream = postedFile.OpenReadStream())
            {
                while ((bytesLeidos = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    totalBytesLeidos += bytesLeidos;
                    foreach (byte bit in buffer)
                    {
                        listadoBytesArchivo.Add(bit);
                    }
                }
            }
        }
        public void EscrituraArchivoCompresion(string rutaArchivo, string caracteresYSusPrefijos, Dictionary<char, DictionaryValueElement> diccionario)
        {
            var longitudCadena = caracteresYSusPrefijos.Length;
            var buffer = new byte[longitudCadena + 2];
            for (int i = 0; i < longitudCadena; i++)
            {
                buffer[i] = Convert.ToByte(caracteresYSusPrefijos[i]);
            }
            buffer[longitudCadena] = Convert.ToByte('-');
            buffer[longitudCadena + 1] = Convert.ToByte('-');
            int conteo = 0;
            System.IO.File.Delete(rutaArchivo);
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
                    byte[] bytebuffer = new byte[500];
                    var cadena = new List<char>();
                    var cantidadbuffer = 0;
                    foreach (byte bit in listadoBytesArchivo)
                    {
                        var separación = new DictionaryValueElement();
                        separación = diccionario.GetValueOrDefault((char) bit);
                        foreach (char caracter in separación.RetornarPrefixCode())
                        {
                            cadena.Add(caracter);
                        }
                    }
                    string binario = "";
                    foreach (char car in cadena)
                    {
                        if (binario.Count() == 8)
                        {
                            byte DECABYTE = new byte();
                            var pref = binario;
                            decimal x = Convert.ToInt32(pref, 2);
                            DECABYTE = Convert.ToByte(x);
                            bytebuffer[cantidadbuffer] = DECABYTE;
                            cantidadbuffer++;
                            binario = "";
                            binario = binario + car;
                        }
                        else
                        {
                            binario = binario + car;
                        }
                        if (cantidadbuffer == 500)
                        {
                            writer.Seek(0, SeekOrigin.End);
                            writer.Write(bytebuffer);
                            cantidadbuffer = 0;
                            bytebuffer = new byte[500];
                        }
                    }
                    if (binario != "")
                    {
                        while (binario.Count() != 8)
                        {
                            binario = binario + "0";
                        }
                        byte DECABYTE = new byte();
                        var pref = binario;
                        decimal x = Convert.ToInt32(pref, 2);
                        DECABYTE = Convert.ToByte(x);
                        bytebuffer[cantidadbuffer] = DECABYTE;
                        writer.Seek(0, SeekOrigin.End);
                        writer.Write(bytebuffer);
                    }
                }
            }
        }
        public Dictionary<string, char> LecturaArchivoDescompresion(IFormFile postedFile)
        {
            var diccionario = new Dictionary<string, char>();
            using (var stream = postedFile.OpenReadStream())
            {
                using (var reader = new BinaryReader(stream))
                {
                    int conteo0 = 0;
                    string prefijos = "";
                    char caracter = ' ';
                    bool encontrado = false;
                    byte[] byteBuffer = new byte[10000];
                    bool separador = false;
                    bool demasiado = false;
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
                                            prefijo.AsignarPrefixCode(prefijos);
                                            i++;
                                            if (prefijo.RetornarPrefixCode()[0] == '|')
                                            {
                                                string prueba = "";
                                                for (int j = 1; j < prefijo.RetornarPrefixCode().Count(); j++)
                                                {
                                                    prueba = prueba + prefijo.RetornarPrefixCode()[j];
                                                }
                                                prefijo.AsignarPrefixCode(prueba);
                                            }
                                            diccionario.Add(prefijo.RetornarPrefixCode(), caracter);
                                            encontrado = false;
                                            prefijos = "";
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
            return diccionario;
        }
        public void EscrituraArchivoDescompresion(string decompressedFilePath, string text)
        {
            System.IO.File.Delete(decompressedFilePath);
            using (var writeStream = new FileStream(decompressedFilePath, FileMode.OpenOrCreate))
            {
                using (var writer = new BinaryWriter(writeStream))
                {
                    var cantidadvecesbuffer = 0;
                    var byteBufferfinal = new byte[100];
                    var cantidad = 0;
                    foreach (char carfinal in text)
                    {
                        byteBufferfinal[cantidad] = Convert.ToByte(carfinal);
                        cantidad++;
                        if (cantidad == 100)
                        {
                            if (cantidadvecesbuffer == 0)
                            {
                                writer.Write(byteBufferfinal);
                                byteBufferfinal = new byte[100];
                                cantidadvecesbuffer++;
                                cantidad = 0;
                            }
                            else
                            {
                                writer.Seek(0, SeekOrigin.End);
                                writer.Write(byteBufferfinal);
                                byteBufferfinal = new byte[100];
                                cantidad = 0;
                            }
                        }
                    }
                }
            }
        }
        public long RetornarCantidadTotalBytesLeidosArchivo()
        {
            return this.totalBytesLeidos;
        }
        public List<byte> RetornarListadoBytesArchivo()
        {
            return this.listadoBytesArchivo;
        }
    } 
}
