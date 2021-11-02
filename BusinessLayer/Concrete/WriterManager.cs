using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class WriterManager : IWriterService
    {
        IWriterDal _writerDal;

        public WriterManager(IWriterDal writerDal)
        {
            _writerDal = writerDal;
        }

        public List<Writer> GetList()
        {
            return _writerDal.GetListAll();
        }

        public List<Writer> GetWriterByID(int id)
        {
            return _writerDal.GetListAll(x => x.WriterID == id);
        }

        public Writer GetWriterByMail(string mail)
        {
            return _writerDal.GetWriterByMail(mail);
        }

        public void TAdd(Writer t)
        {
            _writerDal.Insert(t);
        }

        public void TDelete(Writer t)
        {
            throw new NotImplementedException();
        }

        public Writer TGetByID(int id)
        {
            throw new NotImplementedException();
        }

        public void TUpdate(Writer t)
        {
            throw new NotImplementedException();
        }
    }
}
