using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFramework
{
    public class EfMessageRepository : GenericRepository<Message>, IMessageDal
    {
        public List<Message> GetInboxWithMessageList(int id, Expression<Func<Message, bool>> filter = null)
        {
            using (var c = new Context())
            {
                return filter == null ?
                    c.Messages.Include(x => x.SenderUser)
                .Where(x => x.ReceiverUser.Id == id).ToList() :
                c.Messages.Include(x => x.SenderUser)
                .Where(x => x.ReceiverUser.Id == id).Where(filter).ToList();
            }
        }
        public Message GetReceivedMessage(int id, Expression<Func<Message, bool>> filter = null)
        {
            using (var c = new Context())
            {
                return filter == null ?
                    c.Messages.Include(x => x.SenderUser)
                .Where(x => x.ReceiverUser.Id == id).FirstOrDefault() :
                c.Messages.Include(x => x.SenderUser)
                .Where(x => x.ReceiverUser.Id == id).FirstOrDefault(filter);
            }
        }
        public Message GetSendedMessage(int id, Expression<Func<Message, bool>> filter = null)
        {
            using (var c = new Context())
            {
                return filter == null ?
                    c.Messages.Include(x => x.ReceiverUser)
                .Where(x => x.SenderUser.Id == id).FirstOrDefault() :
                c.Messages.Include(x => x.ReceiverUser)
                .Where(x => x.SenderUser.Id == id).FirstOrDefault(filter);
            }
        }

        public List<Message> GetSendBoxWithMessageList(int id, Expression<Func<Message, bool>> filter = null)
        {
            using (var c = new Context())
            {
                return filter == null ?
                    c.Messages.Include(x => x.ReceiverUser)
                .Where(x => x.SenderUser.Id == id).ToList() :
                c.Messages.Include(x => x.ReceiverUser)
                .Where(x => x.SenderUser.Id == id).Where(filter).ToList();
            }
        }
    }
}
