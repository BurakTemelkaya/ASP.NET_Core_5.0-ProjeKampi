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
    public class AboutManager : IAboutService
    {
        private readonly IAboutDal _aboutDal;

        public AboutManager(IAboutDal aboutDal)
        {
            _aboutDal = aboutDal;
        }

        public async Task<IDataResult<int>> GetCountAsync(Expression<Func<About, bool>> filter = null)
        {
            return new SuccessDataResult<int>(await _aboutDal.GetCountAsync(filter));
        }

        public async Task<IDataResult<List<About>>> GetListAsync(Expression<Func<About, bool>> filter = null)
        {
            return new SuccessDataResult<List<About>>(await _aboutDal.GetListAllAsync(filter));
        }

        public async Task<IResult> TAddAsync(About t)
        {
            await _aboutDal.InsertAsync(t);
            return new SuccessResult();
        }

        public async Task<IResult> TDeleteAsync(About t)
        {
            await _aboutDal.DeleteAsync(t);
            return new SuccessResult();
        }

        public async Task<IDataResult<About>> TGetByFilterAsync(Expression<Func<About, bool>> filter = null)
        {
            return new SuccessDataResult<About>(await _aboutDal.GetByFilterAsync(filter));
        }

        public async Task<IDataResult<About>> TGetByIDAsync(int id)
        {
            return new SuccessDataResult<About>(await _aboutDal.GetByIDAsync(id));
        }

        public async Task<IResult> TUpdateAsync(About t)
        {
            await _aboutDal.UpdateAsync(t);
            return new SuccessResult();
        }
    }
}
