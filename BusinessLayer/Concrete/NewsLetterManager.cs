using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using BusinessLayer.Models;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.BackgroundTasks;
using CoreLayer.Utilities.MailUtilities;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete;

public class NewsLetterManager : INewsLetterService
{
    private readonly INewsLetterDal _newsLetterDal;
    private readonly IMailService _mailService;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;

    public NewsLetterManager(INewsLetterDal newsLetterDal, IMailService mailService, IBackgroundTaskQueue backgroundTaskQueue)
    {
        _newsLetterDal = newsLetterDal;
        _mailService = mailService;
        _backgroundTaskQueue = backgroundTaskQueue;
    }

    public async Task<IDataResult<NewsLetter>> GetByMailAsync(string mail)
    {
        return new SuccessDataResult<NewsLetter>(await _newsLetterDal.GetByFilterAsync(x => x.Mail == mail));
    }

    public async Task<IDataResult<int>> GetCountAsync()
    {
        return new SuccessDataResult<int>(await _newsLetterDal.GetCountAsync());
    }

    public async Task<IDataResult<List<NewsLetter>>> GetListAsync()
    {
        return new SuccessDataResult<List<NewsLetter>>(await _newsLetterDal.GetListAllAsync());
    }

    public async Task<IResultObject> SendMailAsync(NewsLetterSendMailsModel model, bool mailStatus)
    {
        // Sadece mail listesini al, tüm entity'leri değil (memory optimizasyonu)
        var subscriberEmails = await _newsLetterDal
            .GetListAllAsync(x => x.MailStatus == mailStatus);

        if (!subscriberEmails.Any())
        {
            return new ErrorResult("Gönderilecek aktif abone bulunamadı.");
        }

        // Background'a gönder
        await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(async token =>
        {
            int successCount = 0;
            var failedEmails = new List<string>();

            // Batch processing: 10'ar 10'ar paralel gönder
            var batches = subscriberEmails.Chunk(10);

            foreach (var batch in batches)
            {
                // Cancellation kontrolü
                if (token.IsCancellationRequested)
                    break;

                // Paralel gönderim
                var tasks = batch.Select(async newsLetter =>
                {
                    await _mailService.SendEmailAsync(new Mail()
                    {
                        ToList = [new MailboxAddress(newsLetter.Mail, newsLetter.Mail)],
                        HtmlBody = model.Content,
                        Subject = model.Subject,
                    });

                    successCount++;
                    return true;
                });

                await Task.WhenAll(tasks);

                // Rate limiting: Her batch arasında 1 saniye bekle
                // SMTP sunucusu throttling yapmasın diye
                if (!token.IsCancellationRequested)
                    await Task.Delay(1000, token);
            }
        });

        return new SuccessResult($"{subscriberEmails.Count} aboneye mail gönderimi başlatıldı.");
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
