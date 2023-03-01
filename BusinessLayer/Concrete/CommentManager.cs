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
        public async Task TAddAsync(Comment comment)
        {
            comment.CommentDate= DateTime.Now;
            comment.CommentStatus = true;
            await _commentDal.InsertAsync(comment);
        }

        [ValidationAspect(typeof(CommentValidator))]
        public async Task AddAsync(Comment comment)
        {
            comment.CommentDate = DateTime.Now;
            comment.CommentStatus = true;
            await _commentDal.InsertAsync(comment);
        }

        public async Task<int> GetCountAsync(Expression<Func<Comment, bool>> filter = null)
        {
            return await _commentDal.GetCountAsync(filter);
        }

        public async Task<List<Comment>> GetListByIdAsync(int id)
        {
            return await _commentDal.GetListAllAsync(x => x.BlogID == id);
        }

        public async Task<List<Comment>> GetListAsync(Expression<Func<Comment, bool>> filter = null)
        {
            return await _commentDal.GetListAllAsync(filter);
        }

        public async Task TDeleteAsync(Comment t)
        {
            await _commentDal.DeleteAsync(t);
        }

        public async Task<Comment> TGetByFilterAsync(Expression<Func<Comment, bool>> filter = null)
        {
            return await _commentDal.GetByFilterAsync(filter);
        }

        public async Task<Comment> TGetByIDAsync(int id)
        {
            return await _commentDal.GetByIDAsync(id);
        }
        [ValidationAspect(typeof(CommentValidator))]
        public async Task TUpdateAsync(Comment t)
        {
            var oldValue = await TGetByIDAsync(t.CommentID);
            if (oldValue != null)
            {
                t.BlogScore = oldValue.BlogScore;
                t.CommentDate = oldValue.CommentDate;
                t.BlogID = oldValue.BlogID;
            }
            await _commentDal.UpdateAsync(t);
        }

        public Task<List<Comment>> GetBlogListWithCommentAsync()
        {
            return _commentDal.GetListWithCommentByBlogAsync();
        }
    }
}
