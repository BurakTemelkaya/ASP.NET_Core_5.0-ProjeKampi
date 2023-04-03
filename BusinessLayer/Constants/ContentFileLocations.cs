using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Constants
{
    public static class ContentFileLocations
    {
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
        public static string GetNewsLetterDraftContentFileLocation()
        {
            return "/NewsLetterDraftContents/";
        }
    }
}
