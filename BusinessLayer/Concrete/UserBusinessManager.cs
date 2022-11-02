using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.StaticTexts;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.MailUtilities;
using DataAccessLayer.Abstract;
using EntityLayer;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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
        private readonly IMailService _mailService;

        public UserBusinessManager(UserManager<AppUser> userManager, IMapper mapper, IMailService mailService) : base(mapper)
        {
            _userManager = userManager;
            _mailService = mailService;
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
        public async Task<bool> UpdateUserAsync(UserDto user)
        {
            var value = await GetByIDAsync(user.Id.ToString());
            value.NameSurname = user.NameSurname;
            value.Email = user.Email;
            value.UserName = user.UserName;           
            value.About = user.About;
            value.City = user.City;
            if(user.ImageUrl!=null)
                value.ImageUrl = user.ImageUrl;               
            if (user.Password != null)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(value, user.OldPassword);
                if (checkPassword)
                    value.PasswordHash = _userManager.PasswordHasher.HashPassword(value, user.Password);
                else
                {
                    return false;
                }
            }
            var result = await _userManager.UpdateAsync(value);
            if (result.Succeeded)
                return true;
            else
                return false;
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
        public async Task<bool> BannedUser(string id, DateTime expiration, string banMessageContent)
        {
            var user = await GetByIDAsync(id);
            if (user == null)
                return false;
            var resultSetLockot = await _userManager.SetLockoutEnabledAsync(user, true);
            if (resultSetLockot.Succeeded)
            {
                if (DateTime.Now < expiration)
                {
                    var userRole = await _userManager.GetRolesAsync(user);
                    foreach (var role in userRole)
                    {
                        if (role == "Admin")
                            return false;
                    }
                    var resultSetLockotEndDate = await _userManager.SetLockoutEndDateAsync(user, expiration);
                    if (resultSetLockotEndDate.Succeeded)
                    {
                        _mailService.SendMail(user.Email, "Core Blog Hesabınız Yasaklandı", banMessageContent);
                        return true;
                    }
                }
            }
            return false;
        }
        public async Task<bool> BanOpenUser(string id)
        {
            var user = await GetByIDAsync(id);
            if (user == null)
                return false;
            var result = await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddMinutes(1));
            if (result.Succeeded)
            {
                _mailService.SendMail(user.Email, "Core Blog Hesabınız Banı Açıldı", "Core Blog Hesabınızın Banı Adminlerimiz Tarafından Açıldı.");
                return true;
            }
            else
                return false;
        }
        public async Task UpdateUserName(string id, string userName)
        {
            var user = await GetByIDAsync(id);
            user.UserName = userName;
            await _userManager.UpdateNormalizedUserNameAsync(user);
        }
        public async Task UpdateEmail(string id, string email)
        {
            var user = await GetByIDAsync(id);
            user.Email = email;
            await _userManager.ChangeEmailAsync(user, email, "");
        }


    }
}
