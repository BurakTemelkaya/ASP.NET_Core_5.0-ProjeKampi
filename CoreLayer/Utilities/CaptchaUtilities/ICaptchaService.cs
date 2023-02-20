﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.CaptchaUtilities
{
    public interface ICaptchaService
    {
        Task<bool> CheckCaptchaValidate(HttpContext httpContext, string captcharesponse = null);
        Task<string> RecaptchaControl(HttpContext httpContext, string captcharesponse = null);
        public string GetSiteKey();
        public string GetSecretKey();
    }
}
