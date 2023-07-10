using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.Controllers
{

    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IBusinessUserService _businessUserService;
        private readonly ICategoryService _categoryService;

        public BlogController(IBlogService blogService,
            IBusinessUserService businessUserService, ICategoryService categoryService)
        {
            _blogService = blogService;
            _businessUserService = businessUserService;
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(string id, int page = 1, string search = null)
        {
            var values = await _blogService.GetBlogListByMainPage(id, page, 6, search);

            ViewData["Title"] = "Ana Sayfa";

            if (id != null || search != null)
            {
                ViewData["Title"] = values.Message;
                ViewBag.Message = values.Message;
                ViewBag.IsSuccess = values.Success;
                ViewBag.Search = search;
                ViewBag.Id = id;
            }

            return View(await values.Data.ToPagedListAsync(page, 6));
        }
        [AllowAnonymous]
        public async Task<IActionResult> BlogReadAll(int id)
        {
            ViewBag.i = id;
            var value = await _blogService.GetBlogByIdWithCommentAsync(id);
            if (value == null || !value.Data.BlogStatus)
            {
                return RedirectToAction("Error404", "ErrorPage");
            }
            ViewBag.CommentCount = value.Data.Comments.Count;
            if (value.Data.Comments != null)
            {
                if (value.Data.Comments.Count > 0)
                {
                    double star = value.Data.Comments.Average(x => x.BlogScore);
                    ViewBag.Star = Math.Round(star,1);
                }               
            }
            ViewBag.WriterId = value.Data.Writer.Id;
            ViewBag.WriterName = value.Data.Writer.NameSurname;
            ViewData["Title"] = value.Data.BlogTitle;
            return View(value.Data);
        }
        public async Task<IActionResult> BlogListByWriter(int page = 1)
        {
            var values = await _blogService.GetListWithCategoryByWriterBmAsync(User.Identity.Name, 5, page);
            return View(await values.Data.ToPagedListAsync(page, 5));
        }
        [HttpGet]
        public async Task<IActionResult> BlogAdd()
        {
            var categoryList = await _categoryService.GetCategorySelectedListItemAsync(true);
            ViewBag.CategoryList = categoryList.Data;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BlogAdd(Blog blog, IFormFile blogImage, IFormFile blogThumbnailImage)
        {
            var value = await _blogService.BlogAddAsync(blog, User.Identity.Name, blogImage, blogThumbnailImage);
            var categoryList = await _categoryService.GetCategorySelectedListItemAsync(true);
            ViewBag.CategoryList = categoryList.Data;
            if (!value.Success)
            {
                ModelState.AddModelError("blogImage", value.Message);
                return View(blog);
            }
            return RedirectToAction("BlogListByWriter", "Blog");
        }
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blogValue = await _blogService.GetBlogByIDAsync(id);
            await _blogService.DeleteBlogAsync(blogValue.Data, User.Identity.Name);
            return RedirectToAction("BlogListByWriter");
        }
        public async Task<IActionResult> ChangeStatusBlog(int id)
        {
            await _blogService.ChangedBlogStatusAsync(id, User.Identity.Name);
            return RedirectToAction("BlogListByWriter");
        }
        [HttpGet]
        public async Task<IActionResult> EditBlog(int id)
        {
            var blogValue = await _blogService.GetBlogByIdForUpdate(id);
            var categoryList = await _categoryService.GetCategorySelectedListItemAsync(true);
            ViewBag.CategoryList = categoryList.Data;
            return View(blogValue.Data);
        }
        [HttpPost]
        public async Task<IActionResult> EditBlog(Blog blog, IFormFile blogImage, IFormFile blogThumbnailImage)
        {
            var result = await _blogService.BlogUpdateAsync(blog, User.Identity.Name, blogImage, blogThumbnailImage);
            if (!result.Success)
            {
                var categoryList = await _categoryService.GetCategorySelectedListItemAsync(true);
                ViewBag.CategoryList = categoryList.Data;
                ModelState.AddModelError("", result.Message);
                return View(blog);
            }
            return RedirectToAction("BlogListByWriter");
        }
    }
}
