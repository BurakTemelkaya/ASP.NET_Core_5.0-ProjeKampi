using BusinessLayer.Abstract;
using CoreLayer.Entities;
using CoreLayer.Extensions;
using CoreLayer.Helpers;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete;

public class BlogViewManager : IBlogViewService
{
    private readonly IBlogViewDal _blogViewDal;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEnvironmentService _environmentService;
    private readonly UserHelper _userHelper;

    public BlogViewManager(IBlogViewDal blogViewDal, IHttpContextAccessor httpContextAccessor, IEnvironmentService environmentService, UserHelper userHelper)
    {
        _blogViewDal = blogViewDal;
        _httpContextAccessor = httpContextAccessor;
        _environmentService = environmentService;
        _userHelper = userHelper;
    }

    public async Task<IResultObject> AddAsync(int blogId)
    {
        if (!_environmentService.IsProduction())
        {
            return new SuccessResult();
        }

        string ip = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ip))
        {
            ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        string refererUrl = _httpContextAccessor.HttpContext.Request.Headers["Referer"].ToString();
        if (string.IsNullOrEmpty(refererUrl))
        {
            refererUrl = null;
        }

        var existData = await _blogViewDal.GetByFilterAsync(x => x.BlogId == blogId && x.IpAddress == ip);

        if (existData != null)
        {
            existData.ViewCount += 1;
            await _blogViewDal.UpdateAsync(existData);
        }
        else
        {
            BlogView newData = new()
            {
                BlogId = blogId,
                IpAddress = ip,
                ViewCount = 1,
                ViewingDate = DateTime.Now,
                RefererUrl = refererUrl
            };

            await _blogViewDal.InsertAsync(newData);
        }

        return new SuccessResult();
    }


    public async Task<IResultObject> DeleteAsync(BlogView blogView)
    {
        await _blogViewDal.DeleteAsync(blogView);
        return new SuccessResult();
    }

    public async Task<IDataResult<BlogView>> GetByIDAsync(int id)
    {
        return new SuccessDataResult<BlogView>(await _blogViewDal.GetByIDAsync(id));
    }

    public async Task<IDataResult<int>> GetCountByBlogIdAsync(int blogId)
    {
        return new SuccessDataResult<int>(await _blogViewDal.GetCountAsync(x => x.BlogId == blogId));
    }

    public async Task<IDataResult<List<BlogView>>> GetListByPagingWriterNameAsync(string userName, int take = 0, int page = 0)
    {
        var data = await _blogViewDal.GetListAllByPagingAsync(x => x.Blog.Writer.UserName == userName, take, page, include: bw => bw.Include(bw => bw.Blog));

        return new SuccessDataResult<List<BlogView>>(data);
    }

    public async Task<IDataResult<List<BlogView>>> GetListByPagingNameAsync(int take = 0, int page = 0, bool? isRedirect = null)
    {
        Expression<Func<BlogView, bool>> predicate = null;

        if (isRedirect == true)
        {
            predicate = x => x.RefererUrl != null;
        }
        else if (isRedirect == false)
        {
            predicate = x => x.RefererUrl == null;
        }

        var data = await _blogViewDal.GetListAllByPagingAsync(predicate, take: take, page: page, include: bw => bw.Include(bw => bw.Blog));

        return new SuccessDataResult<List<BlogView>>(data);
    }

    public async Task<IResultObject> UpdateAsync(BlogView blogView)
    {
        await _blogViewDal.UpdateAsync(blogView);
        return new SuccessResult();
    }

    public async Task<Dictionary<DateTime, int>> GetChartDataByWriterAsync(TimeSpan? interval = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        int userId = _userHelper.GetUserId();
        return await GetChartDataAsync(interval, startDate, endDate, userId);
    }

    public async Task<Dictionary<DateTime, int>> GetChartDataByAdminAsync(TimeSpan? interval = null, DateTime? startDate = null, DateTime? endDate = null)
    {
        return await GetChartDataAsync(interval, startDate, endDate);
    }

    public async Task<Dictionary<DateTime, int>> GetChartDataByBlogId(TimeSpan? interval = null, DateTime? startDate = null, DateTime? endDate = null, int? blogId = null)
    {
        return await GetChartDataAsync(interval, startDate, endDate, blogId: blogId);
    }

    private async Task<Dictionary<DateTime, int>> GetChartDataAsync(TimeSpan? interval = null, DateTime? startDate = null, DateTime? endDate = null, int? writerId = null, int? blogId = null)
    {
        Expression<Func<BlogView, bool>> predicate = null;

        if (writerId.HasValue)
        {
            predicate = x => x.Blog.WriterID == int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        if (blogId.HasValue)
        {
            predicate = predicate.And(x => x.BlogId == blogId);
        }

        interval ??= TimeSpan.FromHours(1);
        startDate ??= DateTime.Now.AddDays(-1);
        endDate ??= DateTime.Now;

        var result = await _blogViewDal.GetChartDataAsync(
            nameof(BlogView.ViewingDate),
            TimeUnit.Hour,
            startDate,
            endDate,
            predicate
        );

        return result;
    }
}
