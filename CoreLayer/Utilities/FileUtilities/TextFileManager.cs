﻿using HtmlAgilityPack;
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
            try
            {
                var newFileName = Guid.NewGuid() + ".txt";
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + folderLocation, newFileName);
                using FileStream fileStream = new(location, FileMode.Create, FileAccess.Write);
                using StreamWriter streamWriter = new(fileStream);
                await streamWriter.WriteAsync(text);
                await streamWriter.FlushAsync();
                streamWriter.Close();
                fileStream.Close();
                return folderLocation + newFileName;
            }
            catch
            {
                return null;
            }

        }
        public static async Task<string> ReadTextFileAsync(string folderLocation, int numberOfLetters = 0)
        {
            try
            {
                string fileLocation = @"wwwroot\" + folderLocation;
                FileStream fileStream = new(fileLocation, FileMode.Open, FileAccess.Read);
                StreamReader streamReader = new(fileStream);
                string content = await streamReader.ReadToEndAsync();
                if (numberOfLetters > 0)
                {
                    content = Regex.Replace(content, "&nbsp;", " ");
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(content);
                    content = htmlDoc.DocumentNode.InnerText;
                    if (content.Length > numberOfLetters)
                    {
                        content = content[..numberOfLetters];
                    }                 
                }
                streamReader.Close();
                fileStream.Close();
                return content;
            }
            catch
            {
                return null;
            }

        }
    }
}
