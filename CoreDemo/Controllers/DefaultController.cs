using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace CoreDemo.Controllers
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
