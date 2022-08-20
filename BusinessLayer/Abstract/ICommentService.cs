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
        void CommentAdd(Comment comment);
        //void CommentDelete(Comment comment);
        //void CommentUpdate(Comment comment);
        List<Comment> GetListById(int id);
        List<Comment> GetBlogListWithComment();
        //Comment GetByID(int id);
    }
}
