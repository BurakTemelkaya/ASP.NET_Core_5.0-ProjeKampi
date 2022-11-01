﻿using EntityLayer;
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
        Task UpdateUserAsync(UserDto user);
        Task<UserDto> FindByUserNameAsync(string userName);
        Task<UserDto> FindByMailAsync(string mail);
        Task CastUserRole(AppUser user, string role);
        Task<List<string>> FindUserRoleAsync(AppUser user);
        Task<int> GetByUserCountAsync(Expression<Func<AppUser, bool>> filter = null);
        Task<List<AppUser>> GetUserListAsync(Expression<Func<AppUser, bool>> filter = null);
        Task<bool> BannedUser(AppUser appUser, DateTime expiration);
        Task<bool> BanOpenUser(AppUser appUser);
    }
}
