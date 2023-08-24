using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IContactService : IGenericService<Contact>
    {
        Task<IResultObject> ContactAddAsync(Contact contact);
        Task<IResultObject> DeleteContactsAsync(List<string> ids);
        Task<IResultObject> MarkUsReadAsync(int contactId);
        Task<IResultObject> MarksUsReadAsync(List<string> Ids);
        Task<IResultObject> MarkUsUnreadAsync(int contactId);
        Task<IResultObject> MarksUsUnreadAsync(List<string> Ids);
    }
}
