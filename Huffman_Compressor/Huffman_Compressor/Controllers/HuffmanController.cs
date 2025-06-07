using Huffman_Compressor.Models;
using Huffman_Compressor.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using static System.Net.WebRequestMethods;

namespace Huffman_Compressor.Controllers
{
    public class HuffmanController : Controller
    {
        private const int maxBufferSize = 10000;
        public readonly IWebHostEnvironment env;
        public HuffmanController(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public ActionResult Compression()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Compression(IFormFile postedFile)
        {
            var serverPath = env.ContentRootPath;
            var fileNameIndex = postedFile.FileName.IndexOf(".");
            var fileName = postedFile.FileName.Substring(0, fileNameIndex);
            var compressedFilePath = $"{serverPath}\\Compressed_Files\\{fileName}.huff";
            var readedFile = new Files();
            var compressionMethod = new Compression();
            await readedFile.LecturaArchivoCompresion(postedFile, maxBufferSize);
            compressionMethod.CreacionDiccionario(readedFile.RetornarListadoBytesArchivo());
            compressionMethod.GenerarListaElementos(readedFile.RetornarCantidadTotalBytesLeidosArchivo());
            var huffmanTree = compressionMethod.GenerarArbol();
            var dictionary = huffmanTree.GenerarPrefixCode(huffmanTree.RetornarRaiz(), compressionMethod.RetornarDiccionario(), string.Empty);
            readedFile.EscrituraArchivoCompresion(compressedFilePath, huffmanTree.RetornarCaracteresPrefijos(), dictionary);
            return View();
        }
        public ActionResult DownloadCompressedFiles()
        {
            var filePath = env.ContentRootPath;
            var directoryInfo = new DirectoryInfo($"{filePath}\\Compressed_Files\\");
            var files = directoryInfo.GetFiles(".");
            var lista = new List<string>(files.Length);
            foreach (var item in files)
            {
                lista.Add(item.Name);
            }
            return View(lista);
        }
        public ActionResult DownloadSelectedCompressedFile(string fileName)
        {
            var filePath = env.ContentRootPath;
            var fullPath = $"{filePath}\\Compressed_Files\\{fileName}";
            var filebytes = System.IO.File.ReadAllBytes(fullPath);
            return File(filebytes, "application/octet-stream", fileName);
        }
        public ActionResult Decompression()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Decompression(IFormFile postedFile)
        {
            var filePath = env.ContentRootPath;
            var fileNameIndex = postedFile.FileName.IndexOf(".");
            var fileName = postedFile.FileName.Substring(0, fileNameIndex);
            var decompressedFilePath = $"{filePath}\\Decompressed_Files\\{fileName}_decompressed.huff";
            var readedFile = new Files();
            var dictionary = readedFile.LecturaArchivoDescompresion(postedFile);
            var decompression = new Decompression();
            var text = decompression.DescomprimirArchivo(readedFile.RetornarListadoBytesArchivo(), dictionary);
            readedFile.EscrituraArchivoDescompresion(decompressedFilePath, text);
            return View();
        }
        public ActionResult DownloadDecompressedFiles()
        {
            var filePath = env.ContentRootPath;
            var directoryInfo = new DirectoryInfo($"{filePath}\\Decompressed_Files\\");
            var files = directoryInfo.GetFiles(".");
            var lista = new List<string>(files.Length);
            foreach (var item in files)
            {
                lista.Add(item.Name);
            }
            return View(lista);
        }
        public ActionResult DownloadSelectedDecompressedFile(string fileName)
        {
            var filePath = env.ContentRootPath;
            var fullPath = $"{filePath}\\Decompressed_Files\\{fileName}";
            var filebytes = System.IO.File.ReadAllBytes(fullPath);
            return File(filebytes, "application/octet-stream", fileName);
        }
    }
}
