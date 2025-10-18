using BusinessLayer.Abstract;
using CoreLayer.Entities;
using CoreLayer.Extensions;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using X.PagedList;

namespace BusinessLayer.Concrete;

public class BlogViewManager : IBlogViewService
{
    private readonly IBlogViewDal _blogViewDal;
    private readonly IEnvironmentService _environmentService;
    private readonly UserHelper _userHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BlogViewManager(IBlogViewDal blogViewDal, IEnvironmentService environmentService, UserHelper userHelper, IHttpContextAccessor httpContextAccessor)
    {
        _blogViewDal = blogViewDal;
        _environmentService = environmentService;
        _userHelper = userHelper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IResultObject> AddAsync(int blogId, HttpContext httpContext)
    {
        if (!_environmentService.IsProduction())
        {
            return new SuccessResult();
        }

        string ip = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (string.IsNullOrEmpty(ip))
        {
            ip = httpContext.Connection.RemoteIpAddress?.ToString();
        }

        string refererUrl = httpContext.Request.Headers["Referer"].ToString();
        if (string.IsNullOrEmpty(refererUrl))
        {
            refererUrl = null;
        }

        BlogView existData = await _blogViewDal.GetByFilterAsync(x => x.BlogId == blogId && x.IpAddress == ip);

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

    public async Task<IDataResult<IPagedList<BlogView>>> GetListByPagingWriterNameAsync(string userName, int pageNumber
        = 1, int pageSize = 15)
    {
        IPagedList<BlogView> data = await _blogViewDal.GetPagedListAsync(pageNumber, pageSize, x => x.Blog.Writer.UserName == userName, include: bw => bw.Include(bw => bw.Blog));

        return new SuccessDataResult<IPagedList<BlogView>>(data);
    }


    public async Task<IDataResult<IPagedList<BlogView>>> GetListByPagingNameAsync(int pageNumber = 1, int pageSize = 15, bool? isRedirect = null)
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

        IPagedList<BlogView> data = await _blogViewDal.GetPagedListAsync(pageNumber, pageSize, predicate, include: bw => bw.Include(bw => bw.Blog), o => o.OrderByDescending(x => x.Id));

        return new SuccessDataResult<IPagedList<BlogView>>(data);
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
            predicate = predicate.And(x => x.BlogId == blogId.Value);
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
