using AutoMapper;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserBusinessManager : ManagerBase, IBusinessUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserBusinessManager(UserManager<AppUser> userManager, IMapper mapper) : base(mapper)
        {
            _userManager = userManager;
        }
        public async Task RegisterUserAsync(AppUser T, string password)
        {
            await _userManager.CreateAsync(T, password);
        }

        public async Task DeleteUserAsync(AppUser t)
        {
            await _userManager.DeleteAsync(t);
        }

        public async Task<AppUser> GetByIDAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task UpdateUserAsync(UserDto user)
        {
            var value = await _userManager.FindByNameAsync(user.UserName);
            value.NameSurname = user.NameSurname;
            value.Email = user.Email;
            value.UserName = user.UserName;
            bool oldPassword = await _userManager.CheckPasswordAsync(value, user.OldPassword);
            if (user.Password != null && oldPassword)
            {
                value.PasswordHash = _userManager.PasswordHasher.HashPassword(value, user.Password);
            }
            await _userManager.UpdateAsync(value);
        }

        public async Task<UserDto> FindUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userDto = Mapper.Map<UserDto>(user);
            return userDto;
        }
    }
}
