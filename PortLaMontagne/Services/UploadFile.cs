using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Slugify;

namespace PortLaMontagne.Services
{
    public class UploadFile
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadFile(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> UploadFormFile(IFormFile formFile, string path)
        {
            var uniqueFileName = GetUniqueFileName(formFile.FileName);
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory((uploadPath));

            var filePath = Path.Combine(uploadPath, uniqueFileName);

            await formFile.CopyToAsync(new FileStream(filePath, FileMode.Create));

            return uniqueFileName;
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