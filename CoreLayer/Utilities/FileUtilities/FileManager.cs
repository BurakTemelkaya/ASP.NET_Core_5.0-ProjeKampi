using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.FileUtilities
{
    public class FileManager
    {
        public static string FileAdd(IFormFile image, string Folder)
        {
            var extension = Path.GetExtension(image.FileName);
            var newImageName = Guid.NewGuid() + extension;
            var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + Folder,
                newImageName);
            var stream = new FileStream(location, FileMode.Create);
            image.CopyTo(stream);
            return Folder + newImageName;
        }
        public static string StaticProfileImageLocation()
        {
            return "/WriterImageFiles/";
        }
        public static string StaticAboutImageLocation()
        {
            return "/AboutImageFiles/";
        }
        public static void DeleteFile(string fileLocation)
        {
            try
            {
                var mainPath = "wwwroot";
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), mainPath + fileLocation));
            }
            catch
            {

            }            
        }
    }
}
