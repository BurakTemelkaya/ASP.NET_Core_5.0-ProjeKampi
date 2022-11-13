using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CoreLayer.Utilities.FileUtilities
{
    public class TextFileManager
    {
        public static async Task<string> TextFileAddAsync(string text, string folderLocation)
        {
            var newFileName = Guid.NewGuid() + ".txt";
            var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + folderLocation, newFileName);
            using FileStream fileStream = new(location, FileMode.Create, FileAccess.Write);
            using StreamWriter streamWriter = new(fileStream);
            await streamWriter.WriteLineAsync(text);
            await streamWriter.FlushAsync();
            streamWriter.Close();
            fileStream.Close();
            return folderLocation + newFileName;
        }
        public static async Task<string> ReadTextFileAsync(string folderLocation, int numberOfLetters = 0)
        {
            string fileLocation = @"wwwroot\" + folderLocation;
            FileStream fileStream = new(fileLocation, FileMode.Open, FileAccess.Read);
            StreamReader streamReader = new(fileStream);
            string content = "";
            if (numberOfLetters > 0)
            {
                var result = new char[numberOfLetters];
                int readLetterLength = await streamReader.ReadAsync(result, 0, numberOfLetters);
                int i = 0;
                while (readLetterLength - 1 > i)
                {
                    content += result[i];
                    i++;
                }
                content = Regex.Replace(content, "<.*?>", String.Empty);
            }
            else
            {
                content = await streamReader.ReadLineAsync();
            }
            streamReader.Close();
            fileStream.Close();
            return content;
        }
        public static string GetBlogContentFileLocation()
        {
            return "/BlogContents/";
        }
        public static string GetMessageContentFileLocation()
        {
            return "/MessageContents/";
        }
        public static string GetMessageDraftContentFileLocation()
        {
            return "/MessageDraftContents/";
        }
    }
}
