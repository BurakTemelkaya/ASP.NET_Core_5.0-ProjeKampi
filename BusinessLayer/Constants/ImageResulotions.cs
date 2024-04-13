using System.Drawing;

namespace BusinessLayer.Constants
{
    public static class ImageResulotions
    {
        public static Size GetBlogThumbnailResolution()
        {
            return new Size(640, 360);
        }

        public static Size GetBlogImageResolution()
        {
            return new Size(800, 420);
        }

        public static Size GetProfileImageResolution()
        {
            return new Size(320, 320);
        }

        public static Size GetAboutImageResolution()
        {
            return new Size(900, 500);
        }
    }
}
