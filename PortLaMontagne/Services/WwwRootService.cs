using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Slugify;

namespace PortLaMontagne.Services
{
    public class WwwRootService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public WwwRootService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadFormFile(IFormFile formFile, string path, string previousFileName = null)
        {
            var uniqueFileName = GetUniqueFileName(formFile.FileName);
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory((uploadPath));

            var filePath = Path.Combine(uploadPath, uniqueFileName);

            await formFile.CopyToAsync(new FileStream(filePath, FileMode.Create));

            if (previousFileName is not null)
            {
                DeleteFile(Path.Combine( uploadPath, previousFileName));
            }

            return uniqueFileName;
        }

        public void DeleteFile(string path)
        {
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            if (File.Exists(fullPath))
            {
                try
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    File.Delete(fullPath);
                }
                catch (Exception e) { }
            }
        }

        private static string GetUniqueFileName(string fullFileName)
        {
            var helper = new SlugHelper();
            fullFileName = Path.GetFileName(fullFileName);
            
            var fileName = Path.GetFileNameWithoutExtension(fullFileName);
            fileName = helper.GenerateSlug(fileName);
            
            fileName += "_"
                        + Guid.NewGuid().ToString().Substring(0, 4)
                        + Path.GetExtension(fullFileName);

            return fileName;
        }
    }
}