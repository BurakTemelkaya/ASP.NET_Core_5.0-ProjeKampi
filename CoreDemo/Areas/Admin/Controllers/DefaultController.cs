using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Areas.Admin.Controllers
{
    public class DefaultController : Controller
    {
        public DefaultController(IMapper mapper)
        {
            Mapper = mapper;
        }
        protected IMapper Mapper { get; }
        public IActionResult Index()
        {
            return View();
        }
    }
}
