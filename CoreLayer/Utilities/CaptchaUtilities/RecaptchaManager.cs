using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.CaptchaUtilities;

public class RecaptchaManager : ICaptchaService
{
    private IConfiguration Configuration { get; }

    private readonly IHttpContextAccessor _httpContextAccessor;

    public RecaptchaManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        Configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    private CancellationToken CancellationToken
        => _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

    public async Task<bool> CheckCaptchaValidate(string captcharesponse = null)
    {
        if (string.IsNullOrEmpty(captcharesponse))
        {
            captcharesponse = _httpContextAccessor.HttpContext.Request.Form["g-recaptcha-response"].ToString();
        }
        if (string.IsNullOrEmpty(captcharesponse))
        {
            return false;
        }
        var postData = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("secret", GetSecretKey()),
            new KeyValuePair<string, string>("remoteip", _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()),
            new KeyValuePair<string, string>("response", captcharesponse)
        };

        var client = new HttpClient();
        var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(postData), CancellationToken);

        var o = (JObject)JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync(CancellationToken));

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
        return Configuration["RecaptchaKeys:SecretKey"];
    }

    public string GetSiteKey()
    {
        return Configuration["RecaptchaKeys:SiteKey"];
    }
}