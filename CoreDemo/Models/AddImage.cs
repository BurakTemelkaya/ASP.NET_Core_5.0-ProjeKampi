using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Models
{
    public class AddImage
    {
        public static string ImageAdd(IFormFile image, string Folder)
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

    }
}
