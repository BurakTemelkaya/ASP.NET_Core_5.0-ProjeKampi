using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.FileUtilities
{
    public static class FileManager
    {
        public async static Task<bool> FileMoveAsync(string oldPath, string newPath, bool isOldPathDelete = true)
        {
            try
            {
                oldPath = @"wwwroot/" + oldPath;

                File.Move(oldPath, @"wwwroot/" + newPath);

                if (isOldPathDelete)
                    File.Delete(oldPath);

                await TextFileManager.TextFileAddAsync(oldPath + "moved by " + newPath + " and deleted.", "/ExceptionLogs");

                return true;

            }
            catch (Exception e)
            {
                await TextFileManager.TextFileAddAsync(e.ToString(), "/ExceptionLogs");
                return false;
            }
        }
    }
}
