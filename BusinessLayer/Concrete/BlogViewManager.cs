using BusinessLayer.Abstract;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete;

public class BlogViewManager : IBlogViewService
{
    private readonly IBlogViewDal _blogViewDal;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BlogViewManager(IBlogViewDal blogViewDal,IHttpContextAccessor httpContextAccessor)
    {
        _blogViewDal = blogViewDal;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IResultObject> AddAsync(int blogId)
    {
        string ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

        var existData = await _blogViewDal.GetByFilterAsync(x => x.BlogId == blogId && x.IpAddress == ip);

        if (existData != null)
        {
            existData.ViewCount = existData.ViewCount += 1;
            await _blogViewDal.UpdateAsync(existData);
        }
        else
        {
            BlogView newData = new()
            {
                BlogId = blogId,
                IpAddress = ip,
                ViewCount = 1,
                ViewingDate = System.DateTime.Now
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

    public async Task<IDataResult<List<BlogView>>> GetListByPagingWriterIdAsync(int writerId, int take = 0, int page = 0)
    {
        var data = await _blogViewDal.GetListAllByPagingAsync(x => x.Blog.WriterID == writerId);

        return new SuccessDataResult<List<BlogView>>(data);
    }

    public async Task<IResultObject> UpdateAsync(BlogView blogView)
    {
        await _blogViewDal.UpdateAsync(blogView);
        return new SuccessResult();
    }
}
