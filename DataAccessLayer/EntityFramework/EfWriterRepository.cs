using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFramework
{
    public class EfWriterRepository : GenericRepository<Writer>, IWriterDal
    {
        public Writer GetWriterByMail(string mail)
        {
            using var c = new Context();
            return c.Set<Writer>().Where(x => x.WriterMail == mail).FirstOrDefault();
        }
    }
}
