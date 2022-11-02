using EntityLayer;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IBusinessUserService
    {
        Task RegisterUserAsync(AppUser T, string password);
        Task DeleteUserAsync(AppUser t);
        Task<AppUser> GetByIDAsync(string id);
        Task<bool> UpdateUserAsync(UserDto user);
        Task<UserDto> FindByUserNameAsync(string userName);
        Task<UserDto> FindByMailAsync(string mail);
        Task CastUserRole(AppUser user, string role);
        Task<List<string>> FindUserRoleAsync(AppUser user);
        Task<int> GetByUserCountAsync(Expression<Func<AppUser, bool>> filter = null);
        Task<List<AppUser>> GetUserListAsync(Expression<Func<AppUser, bool>> filter = null);
        Task<bool> BannedUser(string id, DateTime expiration, string banMessageContent);
        Task<bool> BanOpenUser(string id);
    }
}
