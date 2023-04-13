﻿using BusinessLayer.Abstract;
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
        private readonly IBusinessUserService _userService;
        public CommentManager(ICommentDal commentDal, IBusinessUserService userService)
        {
            _commentDal = commentDal;
            _userService = userService;
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

        public async Task<IDataResult<List<Comment>>> GetListByBlogIdAsync(int id)
        {
            return new SuccessDataResult<List<Comment>>(await _commentDal.GetListAllAsync(x => x.BlogID == id && x.CommentStatus));
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
            return new SuccessDataResult<List<Comment>>(await _commentDal.GetListWithCommentByBlogandPagingAsync(null, take, page));
        }

        public async Task<IDataResult<List<Comment>>> GetCommentListByWriterandPaging(string userName, int take, int page)
        {
            var user = await _userService.FindByUserNameAsync(userName);
            var data = await _commentDal.GetListWithCommentByBlogandPagingAsync(x => x.Blog.WriterID == user.Data.Id, take, page);
            if (data != null)
            {
                return new SuccessDataResult<List<Comment>>(data);
            }
            return new ErrorDataResult<List<Comment>>("Kullanıcıya ait yorumlar bulunamadı.");
        }

        public async Task<IResult> DeleteCommentByWriter(string userName, int id)
        {
            var user = await _userService.FindByUserNameAsync(userName);
            var comment = await _commentDal.GetCommentByBlog(x => x.CommentID == id);
            if (comment.Blog.WriterID == user.Data.Id)
            {
                var result = await TDeleteAsync(comment);
                if (result.Success)
                {
                    return new SuccessResult("Yorum silindi");
                }
                return new ErrorResult("Yorum silinemedi.");
            }
            return new ErrorResult("Yorum bu yazara ait değil");
        }

        public async Task<IResult> DisabledCommentByWriter(string userName, int id)
        {
            var user = await _userService.FindByUserNameAsync(userName);
            var comment = await _commentDal.GetCommentByBlog(x => x.CommentID == id);
            if (comment.Blog.WriterID == user.Data.Id)
            {
                if (!comment.CommentStatus)
                {
                    return new SuccessResult("Yorum zaten pasif.");
                }
                comment.CommentStatus = false;
                var result = await TUpdateAsync(comment);
                if (result.Success)
                {
                    return new SuccessResult("Yorum pasifleştirildi.");
                }
                return new ErrorResult("Yorum pasifleştirilemedi.");
            }
            return new ErrorResult("Yorum bu yazara ait değil");
        }

        public async Task<IResult> EnabledCommentByWriter(string userName, int id)
        {
            var user = await _userService.FindByUserNameAsync(userName);
            var comment = await _commentDal.GetCommentByBlog(x => x.CommentID == id);
            if (comment.Blog.WriterID == user.Data.Id)
            {
                if (comment.CommentStatus)
                {
                    return new SuccessResult("Yorum zaten aktif.");
                }
                comment.CommentStatus = true;
                var result = await TUpdateAsync(comment);
                if (result.Success)
                {
                    return new SuccessResult("Yorum aktifleştirildi.");
                }
                return new ErrorResult("Yorum aktifleştirilemedi.");
            }
            return new ErrorResult("Yorum bu yazara ait değil");
        }

        public async Task<IResult> ChangeStatusCommentByWriter(string userName, int id)
        {
            var comment = await _commentDal.GetCommentByBlog(x => x.CommentID == id);
            if (comment == null)
            {
                return new ErrorResult("Yorum bulunamadı");
            }
            if (comment.CommentStatus)
            {
                var result = await DisabledCommentByWriter(userName, id);
                if (!result.Success)
                {
                    return new ErrorResult(result.Message);
                }
            }
            else
            {
                var result = await EnabledCommentByWriter(userName, id);
                if (!result.Success)
                {
                    return new ErrorResult(result.Message);
                }
            }
            return new SuccessResult();
        }

        public async Task<IDataResult<Comment>> GetByIdandWriterAsync(string userName, int id)
        {
            var user = await _userService.FindByUserNameAsync(userName);
            var comment = await _commentDal.GetCommentByBlog(x => x.CommentID == id);
            if (comment != null)
            {
                if (comment.Blog.WriterID == user.Data.Id)
                {
                    return new SuccessDataResult<Comment>(comment);
                }
                return new ErrorDataResult<Comment>("Yorum kullanıcıya ait değil.");
            }

            return new ErrorDataResult<Comment>("Yorum bulunamadı.");
        }
    }
}
