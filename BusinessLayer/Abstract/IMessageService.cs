using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IMessageService
    {
        Task<IResult> AddMessageAsync(Message message, string senderUserName, string receiverUserName);
        Task<IResult> UpdateMessageAsync(Message message, string userName);
        Task<IResult> DeleteMessageAsync(int id, string userName);
        Task<IResult> DeleteMessagesAsync(List<string> ids, string userName);
        Task<IDataResult<Message>> GetByIdAsync(int id);
        Task<IDataResult<Message>> GetByFilterAsync(Expression<Func<Message, bool>> filter = null);
        Task<IDataResult<int>> GetCountAsync(Expression<Func<Message, bool>> filter = null);
        Task<IDataResult<int>> GetSendMessageCountAsync(string userName);
        Task<IDataResult<int>> GetUnreadMessagesCountByUserNameAsync(string userName);
        Task<IDataResult<List<Message>>> GetInboxWithMessageListAsync(string userName, string search = null, Expression<Func<Message, bool>> filter = null, int take = 0, int skip = 0);
        Task<IDataResult<List<Message>>> GetSendBoxWithMessageListAsync(string userName, Expression<Func<Message, bool>> filter = null, int take = 0, int skip = 0);
        Task<IDataResult<Message>> GetReceivedMessageAsync(string userName, Expression<Func<Message, bool>> filter = null);
        Task<IDataResult<Message>> GetSendMessageAsync(string userName, Expression<Func<Message, bool>> filter = null);
        Task<IResult> MarkChangedAsync(int messageId, string userName);
        Task<IResult> MarkUsReadAsync(int messageId, string userName);
        Task<IResult> MarksUsReadAsync(List<string> messageIds, string userName);
        Task<IResult> MarkUsUnreadAsync(int messageId, string userName);
        Task<IResult> MarksUsUnreadAsync(List<string> messageIds, string userName);
        Task<IDataResult<Message>> GetByFilterFileName(Expression<Func<Message, bool>> filter = null);
    }
}
