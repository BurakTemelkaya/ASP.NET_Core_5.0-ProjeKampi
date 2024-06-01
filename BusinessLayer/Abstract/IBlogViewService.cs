using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract;

public interface IBlogViewService
{
    Task<IResultObject> AddAsync(int blogId);
    Task<IResultObject> DeleteAsync(BlogView blogView);
    Task<IResultObject> UpdateAsync(BlogView blogView);
    Task<IDataResult<List<BlogView>>> GetListByPagingWriterIdAsync(int writerId, int take = 0, int page = 0);
    Task<IDataResult<BlogView>> GetByIDAsync(int id);
    Task<IDataResult<int>> GetCountByBlogIdAsync(int blogId);
}
