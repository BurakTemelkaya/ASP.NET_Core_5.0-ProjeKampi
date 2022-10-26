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
    public class AboutManager : IAboutService
    {
        private readonly IAboutDal _aboutDal;

        public AboutManager(IAboutDal aboutDal)
        {
            _aboutDal = aboutDal;
        }

        public async Task<int> GetCountAsync(Expression<Func<About, bool>> filter = null)
        {
            return await _aboutDal.GetCountAsync(filter);
        }

        public Task<List<About>> GetListAsync(Expression<Func<About, bool>> filter = null)
        {
            return _aboutDal.GetListAllAsync(filter);
        }

        public async Task TAddAsync(About t)
        {
            await _aboutDal.InsertAsync(t);
        }

        public async Task TDeleteAsync(About t)
        {
            await _aboutDal.DeleteAsync(t);
        }

        public async Task<About> TGetByFilterAsync(Expression<Func<About, bool>> filter = null)
        {
            return await _aboutDal.GetByFilterAsync(filter);
        }

        public async Task<About> TGetByIDAsync(int id)
        {
            return await _aboutDal.GetByIDAsync(id);
        }

        public async Task TUpdateAsync(About t)
        {
            await _aboutDal.UpdateAsync(t);
        }
    }
}
