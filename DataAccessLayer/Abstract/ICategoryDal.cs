using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface ICategoryDal
    {
        List<Category> ListAllCategory();
        void CategoryAdd(Category category);
        void DeleteCategory(Category category);
        void UpdateCategory(Category category);
        Category GetByID(int id);
    }
}
