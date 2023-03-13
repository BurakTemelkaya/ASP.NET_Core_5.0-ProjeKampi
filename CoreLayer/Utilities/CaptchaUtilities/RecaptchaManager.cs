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
        private IConfiguration Configuration { get; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecaptchaManager(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CheckCaptchaValidate(string captcharesponse = null)
        {
            string responseRecaptchaValue;
            if (string.IsNullOrEmpty(captcharesponse))
            {
                responseRecaptchaValue = _httpContextAccessor.HttpContext.Request.Form["g-recaptcha-response"];
            }
            else if (!string.IsNullOrEmpty(captcharesponse))
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
                new KeyValuePair<string, string>("remoteip", _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()),
                new KeyValuePair<string, string>("response", responseRecaptchaValue)
            };

            var client = new HttpClient();
            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(postData));

            var o = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());

            return (bool)o["success"];
        }

        public async Task<string> RecaptchaControl(string captcharesponse = null)
        {
            string image = _httpContextAccessor.HttpContext.Request.Form["g-recaptcha-response"];
            if (string.IsNullOrEmpty(image) && string.IsNullOrEmpty(captcharesponse))
            {
                return "Lütfen doğrulamayı yapın.";
            }
            bool verified = await CheckCaptchaValidate(captcharesponse);
            if (!verified)
            {
                return "Recaptcha doğrulamasını yanlış yaptınız.";
            }
            
            return null;
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
