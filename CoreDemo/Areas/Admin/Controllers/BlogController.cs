using ClosedXML.Excel;
using CoreDemo.Areas.Admin.Models;
using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class BlogController : Controller
    {
        public IActionResult ExportStaticExcelBlogList()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Blog Listesi");
                worksheet.Cell(1, 1).Value = "Blog ID";
                worksheet.Cell(1, 2).Value = "Blog Adı";

                int BloRowCount = 2;
                foreach (var item in GetBlogList())
                {
                    worksheet.Cell(BloRowCount, 1).Value = item.ID;
                    worksheet.Cell(BloRowCount, 2).Value = item.BlogName;
                    BloRowCount++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Calisma1.xlsx");
                }
            }
        }
        public IActionResult BlogListExcel()
        {
            return View();
        }
        List<BlogModel> GetBlogList()
        {
            List<BlogModel> blogModels = new List<BlogModel> {
            new BlogModel{ID=1, BlogName="C# Programlamaya giriş" },
            new BlogModel{ID=2,BlogName="Tesla Firmasının Araçlar"},
            new BlogModel{ID=3,BlogName="2022 Olimpiyatları"}
            };
            return blogModels;
        }
        public IActionResult ExportDynamicExcelBlogList()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Blog Listesi");
                worksheet.Cell(1, 1).Value = "Blog ID";
                worksheet.Cell(1, 2).Value = "Blog Adı";

                int BloRowCount = 2;
                foreach (var item in GetBlogTitleList())
                {
                    worksheet.Cell(BloRowCount, 1).Value = item.ID;
                    worksheet.Cell(BloRowCount, 2).Value = item.BlogName;
                    BloRowCount++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Calisma1.xlsx");
                }
            }
        }
        public List<BlogModel2> GetBlogTitleList()
        {
            List<BlogModel2> blogModels = new List<BlogModel2>();
            using (var c = new Context())
            {
                blogModels = c.Blogs.Select(x => new BlogModel2
                {
                    ID = x.BlogID,
                    BlogName = x.BlogTitle
                }).ToList();
                return blogModels;
            }
        }
        public IActionResult BlogTitleListExcel()
        {
            return View();
        }
    }
}
