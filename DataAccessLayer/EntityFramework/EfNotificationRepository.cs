using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFramework
{
    public class EfNotificationRepository : GenericRepository<Notification>, INotificationDal
    {
        public EfNotificationRepository(DbContextOptions<Context> context) : base(context)
        {
        }
    }
}
