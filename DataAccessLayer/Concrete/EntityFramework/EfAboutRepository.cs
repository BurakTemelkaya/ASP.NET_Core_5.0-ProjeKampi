using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.DataAccess.EntityFramework;

namespace DataAccessLayer.Concrete.EntityFramework
{
    public class EfAboutRepository : EfEntityRepositoryBase<About>, IAboutDal
    {
        public EfAboutRepository(Context context) : base(context)
        {

        }
    }
}
