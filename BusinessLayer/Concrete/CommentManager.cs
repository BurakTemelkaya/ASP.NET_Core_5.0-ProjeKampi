using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CommentManager : ICommentService
    {
        private readonly ICommentDal _commentDal;
        public CommentManager(ICommentDal commentDal)
        {
            _commentDal = commentDal;
        }

        public void CommentAdd(Comment comment)
        {
            _commentDal.Insert(comment);
        }

        public int GetCount(Expression<Func<Comment, bool>> filter = null)
        {
            return _commentDal.GetCount(filter);
        }

        public List<Comment> GetListById(int id)
        {
            return _commentDal.GetListAll(x => x.BlogID == id);
        }

        public List<Comment> GetList(Expression<Func<Comment, bool>> filter = null)
        {
            return _commentDal.GetListAll(filter);
        }

        public void TAdd(Comment t)
        {
            throw new NotImplementedException();
        }

        public void TDelete(Comment t)
        {
            throw new NotImplementedException();
        }

        public Comment TGetByFilter(Expression<Func<Comment, bool>> filter = null)
        {
            return _commentDal.GetByFilter(filter);
        }

        public Comment TGetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void TUpdate(Comment t)
        {
            throw new NotImplementedException();
        }

        public List<Comment> GetBlogListWithComment()
        {
            return _commentDal.GetListWithCommentByBlog();
        }
    }
}
