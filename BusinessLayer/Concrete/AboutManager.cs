using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Caching;
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

        [CacheAspect]
        public async Task<IDataResult<About>> GetFirst()
        {
            return new SuccessDataResult<About>(await _aboutDal.GetByFilterAsync());
        }

        [CacheRemoveAspect("IAboutService.Get")]
        public async Task<IResult> TUpdateAsync(About t)
        {
            await _aboutDal.UpdateAsync(t);
            return new SuccessResult();
        }
    }
}
