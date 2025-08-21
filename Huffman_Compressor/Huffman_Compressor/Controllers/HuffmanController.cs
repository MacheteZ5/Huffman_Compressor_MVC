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
        private readonly IWebHostEnvironment env;
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
            await readedFile.FileReadingCompression(postedFile, maxBufferSize);
            compressionMethod.DictionaryCreation(readedFile.ListadoBytesArchivo);
            compressionMethod.GenerateElementsList(readedFile.TotalBytesLeidos);
            var huffmanTree = compressionMethod.CreateTree();
            var dictionary = huffmanTree.GeneratePrefixCode(huffmanTree.Root, compressionMethod.Dictionary, string.Empty);
            readedFile.FileWritingCompression(compressedFilePath, huffmanTree.CaracteresYSusPrefijos, dictionary);
            return View();
        }
        public ActionResult DownloadCompressedFiles()
        {
            var filePath = env.ContentRootPath;
            var directoryInfo = new DirectoryInfo($"{filePath}\\Compressed_Files\\");
            var files = directoryInfo.GetFiles(".").ToList();
            var filesList = new List<string>();
            files.ForEach(file => filesList.Add(file.Name));
            return View(filesList);
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
            var dictionary = readedFile.FileReadingDecompression(postedFile);
            var decompression = new Decompression();
            var text = decompression.DecompressFile(readedFile.ListadoBytesArchivo, dictionary);
            readedFile.FileWritingDecompression(decompressedFilePath, text, maxBufferSize);
            return View();
        }
        public ActionResult DownloadDecompressedFiles()
        {
            var filePath = env.ContentRootPath;
            var directoryInfo = new DirectoryInfo($"{filePath}\\Decompressed_Files\\");
            var files = directoryInfo.GetFiles(".").ToList();
            var filesList = new List<string>();
            files.ForEach(file => filesList.Add(file.Name));
            return View(filesList);
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
