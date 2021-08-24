using Microsoft.AspNetCore.Http;
using SafeChatAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SafeChatAPI.Services.Files
{
    public class SaveFileService
    {
        public async Task<List<string>> Save(List<IFormFile> formFiles, string origin)
        {
            List<string> filePaths = new();

            foreach (var formFile in formFiles)
            {
                var fileExtension = Path.GetExtension(formFile.FileName);

                var newFileName = (Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + fileExtension).Replace("-", "");

                var filePath = Path.Combine("wwwroot/ImagesFromMessages/", newFileName);

                using var stream = File.Create(filePath);

                await formFile.CopyToAsync(stream);
                filePaths.Add(origin + "/" + filePath.Replace("wwwroot/", ""));
            }

            return filePaths;
        }
    }
}
