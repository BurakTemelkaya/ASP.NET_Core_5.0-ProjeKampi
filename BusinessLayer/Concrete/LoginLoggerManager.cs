using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class LoginLoggerManager : ManagerBase, ILoginLoggerService
    {
        private readonly ILoginLoggerDal _loginLogger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBusinessUserService _userService;
        private IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LoginLoggerManager(ILoginLoggerDal loginLoggerDal, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IBusinessUserService userService, IConfiguration configuration,IWebHostEnvironment webHostEnvironment) : base(mapper)
        {
            _loginLogger = loginLoggerDal;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            Configuration = configuration;
            _webHostEnvironment= webHostEnvironment;
        }

        private async Task<string> GetLocationAsync(string ip)
        {
            string location = "";
            try
            {
                using var httpClient = new HttpClient();
                string url = Configuration["IpApiValues:Url"];
                using var request = new HttpRequestMessage(HttpMethod.Get, $"{url}{ip}\r\n");
                using var response = await httpClient.SendAsync(request);
                using var reader = new StreamReader(await response.Content.ReadAsStreamAsync());
                string json = await reader.ReadToEndAsync();
                var data = JObject.Parse(json);
                if (data["status"].ToString() == "success")
                {
                    string city = data["city"].ToString();
                    string country = data["country"].ToString();
                    location = $"{city}, {country}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return location;
        }

        public async Task<IResultObject> AddAsync(string userName)
        {
            if (!_webHostEnvironment.IsProduction())
            {
                return new SuccessResult();
            }

            var user = await _userService.GetByUserNameAsync(userName);
            if (!user.Success)
            {
                return new ErrorResult(user.Message);
            }

            var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            string location = await GetLocationAsync(ip);

            var logginLogger = new LoginLogger
            {
                UserId = user.Data.Id,
                IpAddress = ip,
                LoginDate = DateTime.Now,
                Location = location,
            };

            await _loginLogger.InsertAsync(logginLogger);
            return new SuccessResult();
        }

        public async Task<IDataResult<LoginLogger>> GetByUserAsync(int id)
        {
            var user = await _userService.GetByUserNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
            if (!user.Success)
                return new ErrorDataResult<LoginLogger>(user.Message);

            var data = await _loginLogger.GetByFilterAsync(x => x.Id == id && x.UserId == user.Data.Id);
            if (data != null)
            {
                return new SuccessDataResult<LoginLogger>(data);
            }
            else
            {
                return new ErrorDataResult<LoginLogger>(Messages.EmptyLoginLoggerData);
            }
        }

        public async Task<IDataResult<List<LoginLogger>>> GetListByUserAsync(int page = 1, int take = 10)
        {
            var user = await _userService.GetByUserNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
            if (!user.Success)
                return new ErrorDataResult<List<LoginLogger>>();

            var data = await _loginLogger.GetListAllByPagingAsync(x => x.UserId == user.Data.Id, take, page);
            return new SuccessDataResult<List<LoginLogger>>(data);
        }

        public async Task<IDataResult<LoginLogger>> GetAsync(int id)
        {
            var data = await _loginLogger.GetByFilterAsync(x => x.Id == id);
            return new SuccessDataResult<LoginLogger>(data);
        }

        public async Task<IDataResult<List<LoginLogger>>> GetListAllAsync(int page = 1, int take = 10, string userName = null)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                var user = await _userService.GetByUserNameAsync(userName);
                if (!user.Success)
                {
                    return new ErrorDataResult<List<LoginLogger>>(Messages.UserNotFound);
                }
                var data = await _loginLogger.GetLogginLoggerListByUserAsync(x => x.UserId == user.Data.Id, take, page);
                return new SuccessDataResult<List<LoginLogger>>(data);
            }
            else
            {
                var data = await _loginLogger.GetLogginLoggerListByUserAsync(null, take, page);
                return new SuccessDataResult<List<LoginLogger>>(data);
            }
        }
    }
}
