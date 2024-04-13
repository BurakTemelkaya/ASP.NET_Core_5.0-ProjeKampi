using System;
using System.IO;

namespace CoreLayer.Utilities.FileUtilities
{
    public class DeleteFileManager
    {
        public static void DeleteFile(string fileLocation)
        {
            try
            {
                var mainPath = "wwwroot";
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), mainPath + fileLocation));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }
    }
}
