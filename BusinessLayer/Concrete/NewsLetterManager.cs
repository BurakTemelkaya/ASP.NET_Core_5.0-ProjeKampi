using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using BusinessLayer.Models;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.MailUtilities;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete;

public class NewsLetterManager : INewsLetterService
{
    private readonly INewsLetterDal _newsLetterDal;
    readonly IMailService _mailService;

    public NewsLetterManager(INewsLetterDal newsLetterDal, IMailService mailService)
    {
        _newsLetterDal = newsLetterDal;
        _mailService = mailService;
    }

    public async Task<IDataResult<NewsLetter>> GetByMailAsync(string mail)
    {
        return new SuccessDataResult<NewsLetter>(await _newsLetterDal.GetByFilterAsync(x => x.Mail == mail));
    }

    public async Task<IDataResult<int>> GetCountAsync()
    {
        return new SuccessDataResult<int>(await _newsLetterDal.GetCountAsync());
    }

    public async Task<IDataResult<List<NewsLetter>>>  GetListAsync()
    {
        return new SuccessDataResult<List<NewsLetter>>(await _newsLetterDal.GetListAllAsync());
    }

    public async Task<IResultObject> SendMailAsync(NewsLetterSendMailsModel model, bool mailStatus)
    {
        List<NewsLetter> newsLetters = await _newsLetterDal.GetListAllAsync(x => x.MailStatus);

        foreach (var newsLetter in newsLetters)
        {
            await _mailService.SendEmailAsync(new Mail()
            {
                ToList =
                [
                    new MailboxAddress(newsLetter.Mail, newsLetter.Mail)
                ],
                HtmlBody = model.Content,
                Subject = model.Subject,
            });
        }
        

        return new SuccessResult();
    }

    [ValidationAspect(typeof(NewsLetterValidator))]
    public async Task<IResultObject> TAddAsync(NewsLetter t)
    {
        var mail = await GetByMailAsync(t.Mail);

        if (mail.Data == null)
        {
            t.MailStatus = true;
            await _newsLetterDal.InsertAsync(t);
            return new SuccessResult();
        }

        return new ErrorResult(Messages.NewsLetterAlreadyRegistered);
    }

    public async Task<IResultObject> TDeleteAsync(NewsLetter t)
    {
        await _newsLetterDal.DeleteAsync(t);
        return new SuccessResult();
    }

    public async Task<IDataResult<NewsLetter>> TGetByFilterAsync(Expression<Func<NewsLetter, bool>> filter = null)
    {
        return new SuccessDataResult<NewsLetter>(await _newsLetterDal.GetByFilterAsync(filter));
    }

    public async Task<IDataResult<NewsLetter>> TGetByIDAsync(int id)
    {
        return new SuccessDataResult<NewsLetter>(await _newsLetterDal.GetByIDAsync(id));
    }

    [ValidationAspect(typeof(NewsLetterValidator))]
    public async Task<IResultObject> TUpdateAsync(NewsLetter t)
    {
        await _newsLetterDal.UpdateAsync(t);
        return new SuccessResult();
    }
}
