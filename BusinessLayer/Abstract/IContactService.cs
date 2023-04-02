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
        Task<IResult> ContactAddAsync(Contact contact);
        Task<IResult> DeleteContactsAsync(List<string> ids);
        Task<IResult> MarkUsReadAsync(int contactId);
        Task<IResult> MarksUsReadAsync(List<string> Ids);
        Task<IResult> MarkUsUnreadAsync(int contactId);
        Task<IResult> MarksUsUnreadAsync(List<string> Ids);
    }
}
