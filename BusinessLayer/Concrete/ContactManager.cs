using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.FileUtilities;
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
        public async Task ContactAddAsync(Contact contact)
        {
            contact.ContactDate = DateTime.Now;
            contact.ContactStatus = true;
            await _contactDal.InsertAsync(contact);
        }

        public async Task<int> GetCountAsync(Expression<Func<Contact, bool>> filter = null)
        {
            return await _contactDal.GetCountAsync(filter);
        }

        public async Task<List<Contact>> GetListAsync(Expression<Func<Contact, bool>> filter = null)
        {
            return await _contactDal.GetListAllAsync(filter);
        }

        [ValidationAspect(typeof(ContactValidator))]
        public async Task TAddAsync(Contact t)
        {
            await _contactDal.InsertAsync(t);
        }

        public async Task TDeleteAsync(Contact t)
        {
            await _contactDal.DeleteAsync(t);
        }

        public async Task<Contact> TGetByFilterAsync(Expression<Func<Contact, bool>> filter = null)
        {
            return await _contactDal.GetByFilterAsync(filter);
        }

        public async Task<Contact> TGetByIDAsync(int id)
        {
            return await _contactDal.GetByIDAsync(id);
        }

        [ValidationAspect(typeof(ContactValidator))]
        public async Task TUpdateAsync(Contact t)
        {
            await _contactDal.UpdateAsync(t);
        }

        public async Task<bool> MarkUsReadAsync(int contactId)
        {
            if (contactId != 0)
            {
                var contact = await TGetByIDAsync(contactId);

                if (!contact.ContactStatus)
                {
                    contact.ContactStatus = true;
                }
                else
                {
                    return false;
                }

                await _contactDal.UpdateAsync(contact);
                return true;
            }
            return false;
        }

        public async Task<bool> MarkUsUnreadAsync(int contactId)
        {
            if (contactId != 0)
            {
                var contact = await TGetByIDAsync(contactId);

                if (contact.ContactStatus)
                {
                    contact.ContactStatus = false;
                }
                else
                {
                    return false;
                }

                await _contactDal.UpdateAsync(contact);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteContactsAsync(List<string> ids)
        {
            if (ids == null)
            {
                return false;
            }

            List<Contact> contacts = new();

            foreach (var id in ids)
            {
                var message = await TGetByIDAsync(Convert.ToInt32(id));
                contacts.Add(message);
            }
            await _contactDal.DeleteRangeAsync(contacts);
            return true;
        }

        public async Task<bool> MarksUsReadAsync(List<string> Ids)
        {
            List<Contact> contacts = new();

            foreach (var id in Ids)
            {
                if (Convert.ToInt32(id) != 0)
                {
                    try
                    {
                        var contact = await TGetByIDAsync(Convert.ToInt32(id));

                        if (!contact.ContactStatus)
                            contact.ContactStatus = true;

                        contacts.Add(contact);
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            await _contactDal.UpdateRangeAsync(contacts);
            return true;
        }

        public async Task<bool> MarksUsUnreadAsync(List<string> Ids)
        {
            List<Contact> contacts = new();

            foreach (var id in Ids)
            {
                if (Convert.ToInt32(id) != 0)
                {
                    try
                    {
                        var contact = await TGetByIDAsync(Convert.ToInt32(id));

                        if (contact.ContactStatus)
                            contact.ContactStatus = false;

                        contacts.Add(contact);
                    }
                    catch
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            await _contactDal.UpdateRangeAsync(contacts);
            return true;
        }
    }
}
