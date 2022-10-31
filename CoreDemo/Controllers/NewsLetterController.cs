using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class NewsLetterController : Controller
    {
        private readonly INewsLetterService _newsLetterService;

        public NewsLetterController(INewsLetterService newsLetterService)
        {
            _newsLetterService = newsLetterService;
        }

        [HttpGet]
        public PartialViewResult SubscribeMail()
        {
            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> SubscribeMail(NewsLetter newsLetter)
        {
            NewsLetterValidator rules = new();
            if (!rules.Validate(newsLetter).IsValid)
            {
                return BadRequest("Email geçersiz.");
            }
            if (await _newsLetterService.GetByMailAsync(newsLetter.Mail) == null && newsLetter.Mail != null)
            {
                await _newsLetterService.TAddAsync(newsLetter);
                return Ok();
            }
            return BadRequest("Böyle bir mail adresi bulunuyor.");
        }
    }
}
