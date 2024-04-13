using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IUserBusinessService
    {
        Task<IDataResult<IdentityResult>> RegisterUserAsync(UserSignUpDto userSignUpDto, string password);
        Task<IResultObject> DeleteUserAsync(AppUser t);
        Task<IDataResult<AppUser>> GetByIDAsync(string id);
        Task<IDataResult<IdentityResult>> UpdateUserAsync(UserDto user);
        Task<IDataResult<IdentityResult>> UpdateUserForAdminAsync(UserDto user);
        Task<IDataResult<UserDto>> GetByUserNameAsync(string userName);
        Task<IDataResult<UserDto>> GetByUserNameForUpdateAsync(string userName);
        Task<IDataResult<UserDto>> GetByMailAsync(string mail);
        Task<IResultObject> CastUserRole(AppUser user, string role);
        Task<IDataResult<List<string>>> GetUserRoleListAsync(AppUser user);
        Task<IDataResult<int>> GetByUserCountAsync(Expression<Func<AppUser, bool>> filter = null);
        Task<IDataResult<List<AppUser>>> GetUserListAsync(Expression<Func<AppUser, bool>> filter = null);
        Task<IDataResult<List<AppUser>>> GetUserListByUserNameAsync(string userName);
        Task<IResultObject> BannedUser(string id, DateTime expiration, string banMessageContent);
        Task<IResultObject> BanOpenUser(string id);
        Task<IDataResult<string>> GetPasswordResetTokenAsync(string mail);
        Task<IDataResult<IdentityResult>> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<IDataResult<string>> CreateMailTokenAsync(string email);
        Task<IResultObject> ConfirmMailAsync(string email, string token);
        Task<IDataResult<DateTimeOffset?>> GetBanDateAsync(string userName);
    }
}
