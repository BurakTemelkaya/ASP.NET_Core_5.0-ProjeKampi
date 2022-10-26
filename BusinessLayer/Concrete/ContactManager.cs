using BusinessLayer.Abstract;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
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
            contact.ContactStatus = false;
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
    }
}
