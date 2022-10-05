using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IMessageDal : IGenericDal<Message>
    {
        List<Message> GetInboxWithMessageByWriter(int id, Expression<Func<Message, bool>> filter = null);
        List<Message> GetSendBoxWithMessageByWriter(int id, Expression<Func<Message, bool>> filter = null);
    }
}
