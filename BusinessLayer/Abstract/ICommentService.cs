using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICommentService : IGenericService<Comment>
    {
        Task<IDataResult<List<Comment>>> GetListByIdAsync(int id);

        Task<IDataResult<List<Comment>>> GetBlogListWithCommentAsync();
    }
}
