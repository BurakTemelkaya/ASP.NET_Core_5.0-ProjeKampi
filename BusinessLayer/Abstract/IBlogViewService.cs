using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract;

public interface IBlogViewService
{
    Task<IResultObject> AddAsync(int blogId);
    Task<IResultObject> DeleteAsync(BlogView blogView);
    Task<IResultObject> UpdateAsync(BlogView blogView);
    Task<IDataResult<List<BlogView>>> GetListByPagingWriterNameAsync(string userName, int take = 0, int page = 0);
    Task<IDataResult<List<BlogView>>> GetListByPagingNameAsync(int take = 0, int page = 0, bool? isRedirect = null);
    Task<IDataResult<BlogView>> GetByIDAsync(int id);
    Task<IDataResult<int>> GetCountByBlogIdAsync(int blogId);
    Task<Dictionary<DateTime, int>> GetChartDataByWriterAsync(TimeSpan? interval = null, DateTime? startDate = null, DateTime? endDate = null);
    Task<Dictionary<DateTime, int>> GetChartDataByAdminAsync(TimeSpan? interval = null, DateTime? startDate = null, DateTime? endDate = null);
    Task<Dictionary<DateTime, int>> GetChartDataByBlogId(TimeSpan? interval = null, DateTime? startDate = null, DateTime? endDate = null, int? blogId = null);
}
