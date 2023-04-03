using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Constants
{
    public static class ImageLocations
    {
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
    }
}
