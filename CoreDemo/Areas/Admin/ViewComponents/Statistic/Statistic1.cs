using BusinessLayer.Abstract;
using CoreDemo.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoreDemo.Areas.Admin.ViewComponents.Statistic
{
    public class Statistic1 : ViewComponent
    {
        private readonly IBlogService _blogService;
        private readonly IMessageService _messageService;
        private readonly ICommentService _commentService;
        private readonly IConfiguration _configuration;

        public Statistic1(IBlogService blogService, IMessageService message2Service, ICommentService commentService, IConfiguration configuration)
        {
            _blogService = blogService;
            _messageService = message2Service;
            _commentService = commentService;
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string tempCelcius = WeatherApi().Value;

            tempCelcius = tempCelcius.Length == 5 ? tempCelcius[..4] : tempCelcius[..3];

            var blogCount = await _blogService.GetCountAsync();

            var messageCount = await _messageService.GetCountAsync();

            var commentCount = await _commentService.GetCountAsync();

            WidgetModel widgetModel = new WidgetModel
            {
                BlogCount = blogCount.Data,
                MessageCount = messageCount.Data,
                CommentCount = commentCount.Data,
                Temparature = tempCelcius
            };
            ViewBag.v2 = _messageService.GetCountAsync().Result.Data;
            ViewBag.v3 = _commentService.GetCountAsync().Result.Data;
            return View(widgetModel);
        }
        private XAttribute WeatherApi(string city = "istanbul")
        {
            string apiKey = _configuration["OpenWeatherApiKeys:Key"];
            string connection = "https://api.openweathermap.org/data/2.5/weather?q=" + city + "&mode=xml&appid=" + apiKey + "&units=metric";
            XDocument document = XDocument.Load(connection);
            return document.Descendants("temperature").ElementAt(0).Attribute("value");
        }
    }
}
