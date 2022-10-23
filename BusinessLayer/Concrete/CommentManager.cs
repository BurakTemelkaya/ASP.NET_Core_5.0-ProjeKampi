using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
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
        [ValidationAspect(typeof(CommentValidator))]
        public void TAdd(Comment comment)
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

        public void TDelete(Comment t)
        {
            _commentDal.Delete(t);
        }

        public Comment TGetByFilter(Expression<Func<Comment, bool>> filter = null)
        {
            return _commentDal.GetByFilter(filter);
        }

        public Comment TGetByID(int id)
        {
            return _commentDal.GetByID(id);
        }
        [ValidationAspect(typeof(CommentValidator))]
        public void TUpdate(Comment t)
        {
            var oldValue = TGetByID(t.CommentID);
            if (oldValue != null)
            {
                t.BlogScore = oldValue.BlogScore;
                t.CommentDate = oldValue.CommentDate;
                t.BlogID = oldValue.BlogID;
            }
            _commentDal.Update(t);
        }

        public List<Comment> GetBlogListWithComment()
        {
            return _commentDal.GetListWithCommentByBlog();
        }
    }
}
