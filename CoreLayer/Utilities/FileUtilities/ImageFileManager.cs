using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.FileUtilities
{
    public static class ImageFileManager
    {
        public static string ImageAdd(IFormFile file, string folderLocation, int[] resolution)
        {
            try
            {
                using (var image = ResizeImage(file, resolution))
                {
                    var extension = Path.GetExtension(file.FileName);
                    var newImageName = Guid.NewGuid() + extension;
                    var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + folderLocation, newImageName);

                    image.Save(location);

                    return folderLocation + newImageName;
                }
            }
            catch
            {
                return null;
            }
        }

        public static Image ResizeImage(IFormFile image, int[] resolution)
        {
            Image resizedImage = Image.FromStream(image.OpenReadStream(), true, true);
            var newImage = new Bitmap(resolution[0], resolution[1]);
            using var graphics = Graphics.FromImage(newImage);
            graphics.DrawImage(resizedImage, 0, 0, resolution[0], resolution[1]);
            return resizedImage;
        }

        public static string StaticProfileImageLocation()
        {
            return "/WriterImageFiles/";
        }

        public static string StaticAboutImageLocation()
        {
            return "/AboutImageFiles/";
        }

        public static string StaticBlogImageLocation()
        {
            return "/BlogImageFiles/";
        }

        public static int[] GetBlogThumbnailResolution()
        {
            return new int[] { 640, 360 };
        }

        public static int[] GetBlogImageResolution()
        {
            return new int[] { 800, 420 };
        }

        public static int[] GetProfileImageResolution()
        {
            return new int[] { 320, 320 };
        }

        public static int[] GetAboutImageResolution()
        {
            return new int[] { 900, 500 };
        }

    }
}
