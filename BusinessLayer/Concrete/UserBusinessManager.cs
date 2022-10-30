using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.StaticTexts;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using DataAccessLayer.Abstract;
using EntityLayer;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        [ValidationAspect(typeof(UserValidator))]
        public async Task RegisterUserAsync(AppUser T, string password)
        {
            T.RegistrationTime = DateTime.Now;
            await _userManager.CreateAsync(T, password);
            await CastUserRole(T, RolesTexts.WriterRole());
        }
        public async Task CastUserRole(AppUser user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task DeleteUserAsync(AppUser t)
        {
            await _userManager.DeleteAsync(t);
        }

        public async Task<AppUser> GetByIDAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        [ValidationAspect(typeof(UserValidator))]
        public async Task UpdateUserAsync(UserDto user)
        {
            var value = await _userManager.FindByNameAsync(user.UserName);
            value.NameSurname = user.NameSurname;
            value.Email = user.Email;
            value.UserName = user.UserName;
            value.ImageUrl = user.ImageUrl;
            value.About = user.About;
            value.City = user.City;
            bool oldPassword = await _userManager.CheckPasswordAsync(value, user.OldPassword);
            if (user.Password != null && oldPassword)
                value.PasswordHash = _userManager.PasswordHasher.HashPassword(value, user.Password);
            await _userManager.UpdateAsync(value);
        }

        public async Task<UserDto> FindByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userDto = Mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserDto> FindByMailAsync(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            var userDto = Mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<List<string>> FindUserRoleAsync(AppUser user)
        {
            var value = await _userManager.GetRolesAsync(user);
            return value.ToList();
        }

        public async Task<int> GetByUserCountAsync(Expression<Func<AppUser, bool>> filter = null)
        {
            if (filter == null)
                return await _userManager.Users.CountAsync();
            else
                return await _userManager.Users.CountAsync(filter);
        }

        public async Task<List<AppUser>> GetUserListAsync(Expression<Func<AppUser, bool>> filter = null)
        {
            if (filter == null)
                return await _userManager.Users.ToListAsync();
            else
                return await _userManager.Users.Where(filter).ToListAsync();
        }
    }
}
