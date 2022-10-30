using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.FileUtilities
{
    public class TextFileManager
    {
        public static async Task<string> TextFileAdd(string text, string folderLocation)
        {
            var newFileName = Guid.NewGuid() + ".txt";
            var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + folderLocation, newFileName);
            using FileStream fileStream = new(location, FileMode.Create, FileAccess.Write);
            using StreamWriter streamWriter = new(fileStream);
            await streamWriter.WriteLineAsync(text);
            await streamWriter.FlushAsync();
            streamWriter.Close();
            fileStream.Close();
            return folderLocation + "/" + newFileName;
        }
        public static async Task<string> ReadTextFile(string folderLocation)
        {
            string fileLocation = @"wwwroot\" + folderLocation;
            using FileStream fileStream = new(fileLocation, FileMode.Open, FileAccess.Read);
            using StreamReader streamWriter = new(fileStream);
            string content = await streamWriter.ReadLineAsync();
            streamWriter.Close();
            fileStream.Close();
            return content;
        }
        public static string GetBlogContentFileLocation()
        {
            return "/BlogContents";
        }
    }
}
