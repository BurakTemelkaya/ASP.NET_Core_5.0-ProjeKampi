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
        IAboutDal _aboutDal;

        public AboutManager(IAboutDal aboutDal)
        {
            _aboutDal = aboutDal;
        }

        public About TGetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void TAdd(About t)
        {
            throw new NotImplementedException();
        }

        public void TDelete(About t)
        {
            throw new NotImplementedException();
        }

        public void TUpdate(About t)
        {
            throw new NotImplementedException();
        }

        public About TGetByFilter(Expression<Func<About, bool>> filter)
        {
            throw new NotImplementedException();
        }
        public List<About> GetList()
        {
            return _aboutDal.GetListAll();
        }
        public List<About> GetList(Expression<Func<About, bool>> filter)
        {
            return _aboutDal.GetListAll(filter);
        }


    }
}
