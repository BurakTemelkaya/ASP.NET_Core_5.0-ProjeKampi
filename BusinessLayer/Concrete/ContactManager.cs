﻿using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.Results;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        public async Task<IResultObject> ContactAddAsync(Contact contact)
        {
            contact.ContactDate = DateTime.Now;
            contact.ContactStatus = false;
            await _contactDal.InsertAsync(contact);
            return new SuccessResult();
        }

        public async Task<IDataResult<int>> GetCountAsync()
        {
            return new SuccessDataResult<int>(await _contactDal.GetCountAsync());
        }

        public async Task<IDataResult<List<Contact>>> GetListAsync()
        {
            return new SuccessDataResult<List<Contact>>(await _contactDal.GetListAllAsync());
        }

        [ValidationAspect(typeof(ContactValidator))]
        public async Task<IResultObject> TAddAsync(Contact t)
        {
            t.ContactStatus = false;
            await _contactDal.InsertAsync(t);
            return new SuccessResult();
        }

        public async Task<IResultObject> TDeleteAsync(Contact t)
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
        public async Task<IResultObject> TUpdateAsync(Contact t)
        {
            await _contactDal.UpdateAsync(t);
            return new SuccessResult();
        }

        public async Task<IResultObject> MarkUsReadAsync(int contactId)
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

        public async Task<IResultObject> MarkUsUnreadAsync(int contactId)
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

        public async Task<IResultObject> DeleteContactsAsync(List<string> ids)
        {
            if (ids == null)
            {
                return new ErrorResult(Messages.ContactsIsEmpty);
            }

            List<Contact> contacts = new();

            foreach (var id in ids)
            {
                try
                {
                    var message = await TGetByIDAsync(Convert.ToInt32(id));
                    contacts.Add(message.Data);
                }
                catch
                {
                    continue;
                }
            }

            await _contactDal.DeleteRangeAsync(contacts);
            return new SuccessResult();
        }

        public async Task<IResultObject> MarksUsReadAsync(List<string> Ids)
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

        public async Task<IResultObject> MarksUsUnreadAsync(List<string> Ids)
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
