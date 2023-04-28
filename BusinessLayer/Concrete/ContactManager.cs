using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.FileUtilities;
using CoreLayer.Utilities.Results;
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
    public class ContactManager : IContactService
    {
        private readonly IContactDal _contactDal;

        public ContactManager(IContactDal contactDal)
        {
            _contactDal = contactDal;
        }

        [ValidationAspect(typeof(ContactValidator))]
        public async Task<IResult> ContactAddAsync(Contact contact)
        {
            contact.ContactDate = DateTime.Now;
            contact.ContactStatus = false;
            await _contactDal.InsertAsync(contact);
            return new SuccessResult();
        }

        public async Task<IDataResult<int>> GetCountAsync(Expression<Func<Contact, bool>> filter = null)
        {
            return new SuccessDataResult<int>(await _contactDal.GetCountAsync(filter));
        }

        public async Task<IDataResult<List<Contact>>> GetListAsync(Expression<Func<Contact, bool>> filter = null)
        {
            return new SuccessDataResult<List<Contact>>(await _contactDal.GetListAllAsync(filter));
        }

        [ValidationAspect(typeof(ContactValidator))]
        public async Task<IResult> TAddAsync(Contact t)
        {
            t.ContactStatus = false;
            await _contactDal.InsertAsync(t);
            return new SuccessResult();
        }

        public async Task<IResult> TDeleteAsync(Contact t)
        {
            if (t == null)
            {
                return new ErrorResult(Messages.ContactNotEmpty);
            }

            await _contactDal.DeleteAsync(t);
            return new SuccessResult();
        }

        public async Task<IDataResult<Contact>> TGetByFilterAsync(Expression<Func<Contact, bool>> filter = null)
        {
            return new SuccessDataResult<Contact>(await _contactDal.GetByFilterAsync(filter));
        }

        public async Task<IDataResult<Contact>> TGetByIDAsync(int id)
        {
            return new SuccessDataResult<Contact>(await _contactDal.GetByIDAsync(id));
        }

        [ValidationAspect(typeof(ContactValidator))]
        public async Task<IResult> TUpdateAsync(Contact t)
        {
            await _contactDal.UpdateAsync(t);
            return new SuccessResult();
        }

        public async Task<IResult> MarkUsReadAsync(int contactId)
        {
            if (contactId != 0)
            {
                var contact = await TGetByIDAsync(contactId);

                if (!contact.Success)
                {
                    return new ErrorResult(Messages.ContactNotFound);
                }

                if (!contact.Data.ContactStatus)
                {
                    contact.Data.ContactStatus = true;
                }
                else
                {
                    return new ErrorResult(Messages.ContactAlreadyRead);
                }

                await _contactDal.UpdateAsync(contact.Data);
                return new SuccessResult();
            }
            return new ErrorResult(Messages.ContactIsEmpty);
        }

        public async Task<IResult> MarkUsUnreadAsync(int contactId)
        {
            if (contactId != 0)
            {
                var contact = await TGetByIDAsync(contactId);

                if (contact.Data.ContactStatus)
                {
                    contact.Data.ContactStatus = false;
                }
                else
                {
                    return new ErrorResult(Messages.ContactAlreadyUnread);
                }

                await _contactDal.UpdateAsync(contact.Data);
                return new SuccessResult();
            }
            return new ErrorResult(Messages.ContactIsEmpty);
        }

        public async Task<IResult> DeleteContactsAsync(List<string> ids)
        {
            if (ids == null)
            {
                return new ErrorResult(Messages.ContactsIsEmpty);
            }

            List<Contact> contacts = new();

            foreach (var id in ids)
            {
                var message = await TGetByIDAsync(Convert.ToInt32(id));
                contacts.Add(message.Data);
            }
            await _contactDal.DeleteRangeAsync(contacts);
            return new SuccessResult();
        }

        public async Task<IResult> MarksUsReadAsync(List<string> Ids)
        {
            List<Contact> contacts = new();

            foreach (var id in Ids)
            {

                try
                {
                    var contact = await TGetByIDAsync(Convert.ToInt32(id));

                    if (!contact.Data.ContactStatus)
                        contact.Data.ContactStatus = true;

                    contacts.Add(contact.Data);
                }
                catch
                {
                    continue;
                }
            }

            if (contacts.Count == 0)
            {
                return new ErrorResult(Messages.ContactMessagesNotRead);
            }

            await _contactDal.UpdateRangeAsync(contacts);
            return new SuccessResult();
        }

        public async Task<IResult> MarksUsUnreadAsync(List<string> Ids)
        {
            List<Contact> contacts = new();

            foreach (var id in Ids)
            {
                try
                {
                    var contact = await TGetByIDAsync(Convert.ToInt32(id));

                    if (contact.Data.ContactStatus)
                        contact.Data.ContactStatus = false;

                    contacts.Add(contact.Data);
                }
                catch
                {
                    continue;
                }

            }

            if (contacts.Count == 0)
            {
                return new ErrorResult(Messages.ContactMessagesNotUnread);
            }

            await _contactDal.UpdateRangeAsync(contacts);
            return new SuccessResult();
        }
    }
}
