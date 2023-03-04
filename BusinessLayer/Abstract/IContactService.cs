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
        Task ContactAddAsync(Contact contact);
        Task<bool> DeleteContactsAsync(List<string> ids);
        Task<bool> MarkUsReadAsync(int contactId);
        Task<bool> MarksUsReadAsync(List<string> Ids);
        Task<bool> MarkUsUnreadAsync(int contactId);
        Task<bool> MarksUsUnreadAsync(List<string> Ids);
    }
}
