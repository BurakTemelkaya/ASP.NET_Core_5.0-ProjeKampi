using AutoMapper;
using BusinessLayer.Abstract;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class LoginLoggerManager : ManagerBase, ILoginLoggerService
    {
        private readonly ILoginLoggerDal _loginLogger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBusinessUserService _userService;
        private IConfiguration Configuration { get; }
        public LoginLoggerManager(ILoginLoggerDal loginLoggerDal, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IBusinessUserService userService, IConfiguration configuration) : base(mapper)
        {
            _loginLogger = loginLoggerDal;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            Configuration = configuration;
        }

        private string GetLocation(string ip)
        {
            string location = "";
            try
            {
                string apiUrl = $"http://api.ipstack.com/{ip}?access_key={Configuration["IpStackApiKeys:key"]}\r\n";
                string json = new WebClient().DownloadString(apiUrl);
                var data = JObject.Parse(json);
                string city = data["city"].ToString();
                string country = data["country_name"].ToString();
                if (city != string.Empty && country != string.Empty)
                {
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
            var user = await _userService.GetByUserNameAsync(userName);
            if (!user.Success)
            {
                return new ErrorResult();
            }

            var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var location = GetLocation(ip);

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

        public async Task<IDataResult<LoginLogger>> GetAsync(int id)
        {
            var user = await _userService.GetByUserNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
            if (!user.Success)
                return new ErrorDataResult<LoginLogger>();

            var data = await _loginLogger.GetByFilterAsync(x => x.Id == id && x.UserId == user.Data.Id);
            return new SuccessDataResult<LoginLogger>(data);
        }

        public async Task<IDataResult<List<LoginLogger>>> GetListAsync(int page = 0, int take = 0)
        {
            var user = await _userService.GetByUserNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
            if (!user.Success)
                return new ErrorDataResult<List<LoginLogger>>();

            var data = await _loginLogger.GetListAllByPagingAsync(x => x.UserId == user.Data.Id, take, page);
            return new SuccessDataResult<List<LoginLogger>>(data);
        }
    }
}
