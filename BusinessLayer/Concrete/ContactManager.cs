using BusinessLayer.Abstract;
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

        public void ContactAdd(Contact contact)
        {
            contact.ContactStatus = false;
            _contactDal.Insert(contact);
        }

        public int GetCount(Expression<Func<Contact, bool>> filter = null)
        {
            return _contactDal.GetCount(filter);
        }

        public List<Contact> GetList(Expression<Func<Contact, bool>> filter = null)
        {
            return _contactDal.GetListAll(filter);
        }

        public void TAdd(Contact t)
        {
            _contactDal.Insert(t);
        }

        public void TDelete(Contact t)
        {
            _contactDal.Delete(t);
        }

        public Contact TGetByFilter(Expression<Func<Contact, bool>> filter = null)
        {
            return _contactDal.GetByFilter(filter);
        }

        public Contact TGetByID(int id)
        {
            return _contactDal.GetByID(id);
        }

        public void TUpdate(Contact t)
        {
            _contactDal.Update(t);
        }
    }
}
