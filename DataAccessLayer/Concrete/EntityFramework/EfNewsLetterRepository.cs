using CoreLayer.DataAccess.EntityFramework;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfNewsLetterRepository : EfEntityRepositoryBase<NewsLetter>, INewsLetterDal
    {
        public EfNewsLetterRepository(Context context) : base(context)
        {

        }
    }
}
