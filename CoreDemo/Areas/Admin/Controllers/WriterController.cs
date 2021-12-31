using CoreDemo.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WriterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult WriterList()
        {
            var JsonWriters = JsonConvert.SerializeObject(Writers);
            return Json(JsonWriters);
        }
        public IActionResult GetWriterByID(int writerId)
        {
            var findWriter = Writers.FirstOrDefault(x => x.Id == writerId);
            var jsonWriters = JsonConvert.SerializeObject(findWriter);
            return Json(jsonWriters);
        }
        public static List<WriterModel> Writers = new List<WriterModel>
        {
            new WriterModel
            {
                Id=1,
                Name="Burak"
            },
            new WriterModel
            {
                Id=2,
                Name="Murat"
            },
            new WriterModel
            {
                Id=3,
                Name="Buse"
            }
        };
    }
}
