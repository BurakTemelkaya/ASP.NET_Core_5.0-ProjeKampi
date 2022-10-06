using BusinessLayer.Abstract;
using CoreDemo.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMessageService _message2Service;
        private readonly ICommentService _commentService;

        public Statistic1(IBlogService blogService, IMessageService message2Service, ICommentService commentService)
        {
            _blogService = blogService;
            _message2Service = message2Service;
            _commentService = commentService;
        }

        public IViewComponentResult Invoke()
        {
            decimal tempFah = Convert.ToDecimal(WeatherApi().Value.Replace(".",","));
            tempFah = tempFah - Convert.ToDecimal(273.15);
            WidgetModel widgetModel = new WidgetModel
            {
                BlogCount = _blogService.GetCount(),
                MessageCount = _message2Service.GetCount(),
                CommentCount = _commentService.GetCount(),
                Temparature = tempFah.ToString()
            };
            ViewBag.v2 = _message2Service.GetCount();
            ViewBag.v3 = _commentService.GetCount();
            return View(widgetModel);
        }
        private XAttribute WeatherApi(string city = "istanbul")
        {
            string apiKey = "f262680eeed4cc44036e81067615dcaf";
            string connection = "https://api.openweathermap.org/data/2.5/weather?q=" + city + "&mode=xml&appid=" + apiKey;
            XDocument document = XDocument.Load(connection);
            return document.Descendants("temperature").ElementAt(0).Attribute("value");
        }
    }
}
