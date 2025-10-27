using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.FileUtilities;

public static class FileManager
{
    public async static Task<bool> FileMoveAsync(string oldPath, string newPath, bool isOldPathDelete = true, CancellationToken cancellationToken = default)
    {
        try
        {
            oldPath = @"wwwroot/" + oldPath;

            File.Move(oldPath, @"wwwroot/" + newPath);

            if (isOldPathDelete)
                File.Delete(oldPath);

            await TextFileManager.TextFileAddAsync(oldPath + newPath, "/ExceptionLogs", string.Empty, cancellationToken);

            return true;

        }
        catch (Exception e)
        {
            await TextFileManager.TextFileAddAsync(e.ToString(), "/ExceptionLogs", string.Empty, cancellationToken);
            return false;
        }
    }
}
