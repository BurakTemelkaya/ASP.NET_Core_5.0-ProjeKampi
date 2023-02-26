using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.FileUtilities
{
    public class ImageFileManager
    {
        public static async Task<string> ImageAddAsync(IFormFile image, string folderLocation)
        {
            try
            {
                var extension = Path.GetExtension(image.FileName);
                var newImageName = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + folderLocation, newImageName);
                using var stream = new FileStream(location, FileMode.Create);
                await image.CopyToAsync(stream);
                return folderLocation + newImageName;
            }
            catch
            {
                return null;
            }
            
        }
        public static string StaticProfileImageLocation()
        {
            return "/WriterImageFiles/";
        }
        public static string StaticAboutImageLocation()
        {
            return "/AboutImageFiles/";
        }      
    }
}
