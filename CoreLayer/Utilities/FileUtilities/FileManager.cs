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
        public static string FileAdd(IFormFile file, string fileLocation)
        {
            try
            {
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    var newImageName = Guid.NewGuid() + extension;
                    var mainPath = "wwwroot";
                    var path = "/Image/" + fileLocation + "/";
                    var location = Path.Combine(Directory.GetCurrentDirectory(), mainPath + path, newImageName);
                    var stream = new FileStream(location, FileMode.Create);
                    file.CopyTo(stream);
                    string filePath = path + newImageName;
                    return filePath;
                }
            }
            catch
            {
            }
            return FileNotUploadReturnValue();
        }
        public static string FileNotUploadReturnValue()
        {
            return "File Not Uploaded";
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
