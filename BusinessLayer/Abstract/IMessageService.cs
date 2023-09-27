using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using EntityLayer.DTO;
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
        Task<IResultObject> AddMessageAsync(Message message, string senderUserName, string receiverUserName);
        Task<IResultObject> UpdateMessageAsync(Message message, string userName);
        Task<IResultObject> DeleteMessageAsync(int id, string userName);
        Task<IResultObject> DeleteMessagesAsync(List<string> ids, string userName);
        Task<IDataResult<Message>> GetByIdAsync(int id);
        Task<IDataResult<Message>> GetByFilterAsync(Expression<Func<Message, bool>> filter = null);
        Task<IDataResult<int>> GetCountAsync(Expression<Func<Message, bool>> filter = null);
        Task<IDataResult<int>> GetSendMessageCountAsync(string userName);
        Task<IDataResult<int>> GetUnreadMessagesCountByUserNameAsync(string userName);
        Task<IDataResult<List<MessageSenderUserDto>>> GetInboxWithMessageListAsync(string userName, string search = null, Expression<Func<MessageSenderUserDto, bool>> filter = null, int take = 0, int skip = 0);
        Task<IDataResult<List<MessageReceiverUserDto>>> GetSendBoxWithMessageListAsync(string userName, Expression<Func<MessageReceiverUserDto, bool>> filter = null, int take = 0, int skip = 0);
        Task<IDataResult<Message>> GetReceivedMessageAsync(string userName, Expression<Func<Message, bool>> filter = null);
        Task<IDataResult<Message>> GetSendMessageAsync(string userName, Expression<Func<Message, bool>> filter = null);
        Task<IResultObject> MarkChangedAsync(int messageId, string userName);
        Task<IResultObject> MarkUsReadAsync(int messageId, string userName);
        Task<IResultObject> MarksUsReadAsync(List<string> messageIds, string userName);
        Task<IResultObject> MarkUsUnreadAsync(int messageId, string userName);
        Task<IResultObject> MarksUsUnreadAsync(List<string> messageIds, string userName);
        Task<IDataResult<Message>> GetByFilterFileName(Expression<Func<Message, bool>> filter = null);
    }
}
