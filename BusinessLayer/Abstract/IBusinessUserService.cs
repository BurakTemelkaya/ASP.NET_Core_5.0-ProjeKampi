using EntityLayer;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Identity;
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
        Task<IEnumerable<IdentityError>> RegisterUserAsync(UserSignUpDto userSignUpDto, string password);
        Task DeleteUserAsync(AppUser t);
        Task<AppUser> GetByIDAsync(string id);
        Task<IEnumerable<IdentityError>> UpdateUserAsync(UserDto user);
        Task<IEnumerable<IdentityError>> UpdateUserForAdminAsync(UserDto user);
        Task<UserDto> FindByUserNameAsync(string userName);
        Task<UserDto> FindByUserNameForUpdateAsync(string userName);
        Task<UserDto> FindByMailAsync(string mail);
        Task CastUserRole(AppUser user, string role);
        Task<List<string>> GetUserRoleListAsync(AppUser user);
        Task<int> GetByUserCountAsync(Expression<Func<AppUser, bool>> filter = null);
        Task<List<AppUser>> GetUserListAsync(Expression<Func<AppUser, bool>> filter = null);
        Task<bool> BannedUser(string id, DateTime expiration, string banMessageContent);
        Task<bool> BanOpenUser(string id);
        Task<string> GetPasswordResetTokenAsync(string mail);
        Task<bool> ResetPassword(ResetPasswordDto resetPasswordDto);
    }
}
