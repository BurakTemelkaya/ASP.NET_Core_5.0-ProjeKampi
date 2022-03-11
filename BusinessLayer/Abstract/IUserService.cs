using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserService
    {
        void RegisterUserAsync(AppUser T, string password);
        void DeleteUserAsync(AppUser t);
        Task<AppUser> GetByIDAsync(string id);
        Task<AppUser> GetByUserNameAsync(string userName);
        void UpdateUserAsync(AppUser t);
    }
}
