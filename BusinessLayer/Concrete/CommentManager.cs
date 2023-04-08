using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.Results;
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
        public async Task<IResult> TAddAsync(Comment comment)
        {
            comment.CommentDate = DateTime.Now;
            comment.CommentStatus = true;
            await _commentDal.InsertAsync(comment);
            return new SuccessResult();
        }

        public async Task<IDataResult<int>> GetCountAsync(Expression<Func<Comment, bool>> filter = null)
        {
            return new SuccessDataResult<int>(await _commentDal.GetCountAsync(filter));
        }

        public async Task<IDataResult<List<Comment>>> GetListByIdAsync(int id)
        {
            return new SuccessDataResult<List<Comment>>(await _commentDal.GetListAllAsync(x => x.BlogID == id));
        }

        public async Task<IDataResult<List<Comment>>> GetListAsync(Expression<Func<Comment, bool>> filter = null)
        {
            return new SuccessDataResult<List<Comment>>(await _commentDal.GetListAllAsync(filter));
        }

        public async Task<IResult> TDeleteAsync(Comment t)
        {
            if (t == null)
            {
                return new ErrorResult("Yorum boş olamaz");
            }

            await _commentDal.DeleteAsync(t);
            return new SuccessResult();
        }

        public async Task<IDataResult<Comment>> TGetByFilterAsync(Expression<Func<Comment, bool>> filter = null)
        {
            return new SuccessDataResult<Comment>(await _commentDal.GetByFilterAsync(filter));
        }

        public async Task<IDataResult<Comment>> TGetByIDAsync(int id)
        {
            var values = await _commentDal.GetByIDAsync(id);
            if (values != null)
            {
                return new SuccessDataResult<Comment>(values);
            }
            return new ErrorDataResult<Comment>("Yorum bulunamadı.");
        }
        [ValidationAspect(typeof(CommentValidator))]
        public async Task<IResult> TUpdateAsync(Comment t)
        {
            var oldValueRaw = await TGetByIDAsync(t.CommentID);
            var oldValue = oldValueRaw.Data;
            if (oldValue != null)
            {
                t.BlogScore = oldValue.BlogScore;
                t.CommentDate = oldValue.CommentDate;
                t.BlogID = oldValue.BlogID;
                await _commentDal.UpdateAsync(t);
                return new SuccessResult();
            }
            return new ErrorResult("Güncellenecek değer bulunamadı.");

        }

        public async Task<IDataResult<List<Comment>>> GetBlogListWithCommentAsync()
        {
            return new SuccessDataResult<List<Comment>>(await _commentDal.GetListWithCommentByBlogAsync());
        }

        public async Task<IDataResult<List<Comment>>> GetCommentListWithBlogByPagingAsync(int take = 0, int page = 0)
        {
            return new SuccessDataResult<List<Comment>>(await _commentDal.GetListWithCommentByBlogandPagingAsync(take, page));
        }
    }
}
