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
    public class MessageManager : IMessageService
    {
        private readonly IMessageDal _messageDal;

        public MessageManager(IMessageDal messageDal)
        {
            _messageDal = messageDal;
        }

        public int GetCount(Expression<Func<Message, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetList(Expression<Func<Message, bool>> filter = null)
        {
            return _messageDal.GetListAll(filter);
        }

        public void TAdd(Message t)
        {
            _messageDal.Insert(t);
        }

        public void TDelete(Message t)
        {
            _messageDal.Delete(t);
        }

        public Message TGetByFilter(Expression<Func<Message, bool>> filter = null)
        {
            return _messageDal.GetByFilter(filter);
        }

        public Message TGetByID(int id)
        {
            return _messageDal.GetByID(id);
        }

        public void TUpdate(Message t)
        {
            _messageDal.Update(t);
        }
    }
}
