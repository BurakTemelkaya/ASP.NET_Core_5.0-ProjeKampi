using CoreLayer.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.FileUtilities
{
    public static class ImageFileManager
    {
        public static string ImageAdd(IFormFile file, string folderLocation, Size size)
        {
            try
            {
                using (var image = ResizeImage(file, size))
                {
                    if (image == null)
                    {
                        return null;
                    }

                    var extension = Path.GetExtension(file.FileName);
                    var newImageName = Guid.NewGuid() + extension;
                    var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot" + folderLocation, newImageName);

                    var myImageCodecInfo = GetTypeInfo("image/" + extension[1..]);

                    var myEncoder = System.Drawing.Imaging.Encoder.Quality;

                    var myEncoderParameters = new EncoderParameters(1);

                    var myEncoderParameter = new EncoderParameter(myEncoder, 1L);

                    myEncoderParameters.Param[0] = myEncoderParameter;

                    image.Save(location, myImageCodecInfo, myEncoderParameters);

                    return folderLocation + newImageName;
                }
            }
            catch
            {
                return null;
            }
        }

        public static Image ResizeImage(IFormFile image, Size size)
        {
            try
            {
                Image imgToResize = Image.FromStream(image.OpenReadStream(), true, true);

                var destRect = new Rectangle(0, 0, size.Width, size.Height);
                var destImage = new Bitmap(size.Width, size.Height);

                destImage.SetResolution(imgToResize.HorizontalResolution, imgToResize.VerticalResolution);

                using (var graphics = Graphics.FromImage(destImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var wrapMode = new ImageAttributes())
                    {
                        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                        graphics.DrawImage(imgToResize, destRect, 0, 0, imgToResize.Width, imgToResize.Height, GraphicsUnit.Pixel, wrapMode);
                    }
                }

                return destImage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static IFormFile DownloadImage(string imageUrl)
        {
            try
            {
                var request = WebRequest.Create(imageUrl);
                using (var response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (Bitmap bitmap = new Bitmap(stream))
                        {
                            var resultStream = new MemoryStream();

                            bitmap.Save(resultStream, ImageFormat.Png);
                            return new FormFile(resultStream, 0, resultStream.Length, "test.png", "deneme.png")
                            {
                                Headers = new HeaderDictionary(),
                                ContentType = "image/*"
                            };

                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        private static ImageCodecInfo GetTypeInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

    }
}
