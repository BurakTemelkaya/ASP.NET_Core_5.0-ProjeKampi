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
        public List<Message> GetInboxWithMessageByWriter(int id)
        {
            using (var c = new Context())
            {
                return c.Messages.Include(x => x.SenderUser)
                .Where(x => x.ReceiverUser.Id == id).ToList();
            }
        }

        public List<Message> GetSendBoxWithMessageByWriter(int id)
        {
            using (var c = new Context())
            {
                return c.Messages.Include(x => x.ReceiverUser)
                .Where(x => x.ReceiverUser.Id == id).ToList();
            }
        }
    }
}
