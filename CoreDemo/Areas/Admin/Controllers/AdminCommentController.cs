﻿using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Extensions;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class AdminCommentController : Controller
    {
        readonly ICommentService _commentService;
        public AdminCommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        //TODO: Silinen bloglara ait yorumlar gelmiyor onun için ayrı bir sayfa yap.
        public async Task<IActionResult> Index(int page = 1)
        {
            var comments = await _commentService.GetCommentListWithBlogByPagingAsync(5, page);
            var values = comments.Data.ToPagedList(page, 5);
            return View(values);
        }
        public async Task<IActionResult> DeleteComment(int id)
        {
            var value = await _commentService.TGetByIDAsync(id);
            if (value.Success)
            {
                var result = await _commentService.TDeleteAsync(value.Data);
                if (result.Success)
                {
                    ViewBag.ReturnMessage = "Yorum Başarıyla Silindi";
                    return RedirectToAction("Index");
                }
            }

            ViewBag.ReturnMessage = value.Message;
            return View(value);
        }

        public async Task<IActionResult> StatusChangedComment(int id)
        {
            await _commentService.ChangeStatusCommentByAdmin(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditComment(int id)
        {
            var value = await _commentService.TGetByIDAsync(id);
            if (value.Success)
            {
                return View(value.Data);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> EditComment(Comment comment)
        {
            if (comment != null)
                await _commentService.TUpdateAsync(comment);
            return RedirectToAction("Index");
        }
    }
}
