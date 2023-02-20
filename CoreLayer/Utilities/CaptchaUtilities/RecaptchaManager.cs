using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CoreLayer.Utilities.CaptchaUtilities
{
    public class RecaptchaManager : ICaptchaService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RecaptchaManager(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public async Task<bool> CheckCaptchaValidate(HttpContext httpContext, string captcharesponse = null)
        {
            var responseRecaptchaValue = "";
            if (captcharesponse == null)
            {
                responseRecaptchaValue = httpContext.Request.Form["g-recaptcha-response"];
            }
            else if (captcharesponse != null)
            {
                responseRecaptchaValue = captcharesponse;
            }
            else
            {
                return false;
            }
            var postData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("secret", GetSecretKey()),
                new KeyValuePair<string, string>("remoteip", httpContext.Connection.RemoteIpAddress.ToString()),
                new KeyValuePair<string, string>("response", responseRecaptchaValue)
            };

            var client = new HttpClient();
            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(postData));

            var o = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return (bool)o["success"];
        }

        public async Task<string> RecaptchaControl(HttpContext httpContext, string captcharesponse = null)
        {
            string image = CheckCaptchaImage(httpContext);
            if (string.IsNullOrEmpty(image) && captcharesponse == null)
            {
                return "Lütfen doğrulamayı yapın.";
            }
            bool verified = await CheckCaptchaValidate(httpContext, captcharesponse);
            if (!verified)
            {
                return "Recaptcha doğrulamasını yanlış yaptınız.";
            }
            
            return null;
        }
        public string CheckCaptchaImage(HttpContext httpContext)
        {
            return httpContext.Request.Form["g-recaptcha-response"];
        }

        public string GetSecretKey()
        {
            if (_webHostEnvironment.IsProduction())
            {
                return Configuration["RecaptchaKeys:SecretKey"];
            }
            return Configuration["RecaptchaTestKeys:SecretKey"];
        }

        public string GetSiteKey()
        {
            if (_webHostEnvironment.IsProduction())
            {
                return Configuration["RecaptchaKeys:SiteKey"];
            }
            return Configuration["RecaptchaTestKeys:SiteKey"];
        }
    }
}
