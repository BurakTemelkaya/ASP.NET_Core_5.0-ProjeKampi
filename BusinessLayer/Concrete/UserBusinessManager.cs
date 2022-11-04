using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.StaticTexts;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.FileUtilities;
using CoreLayer.Utilities.MailUtilities;
using CoreLayer.Utilities.MailUtilities.Models;
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
        public async Task<IEnumerable<IdentityError>> RegisterUserAsync(UserSignUpDto userSignUpDto, string password)
        {
            var user = Mapper.Map<AppUser>(userSignUpDto);
            user.RegistrationTime = DateTime.Now;
            if (userSignUpDto.ImageFile != null)
            {
                user.ImageUrl = await ImageFileManager.ImageAddAsync(userSignUpDto.ImageFile,
                    ImageFileManager.StaticProfileImageLocation());
            }
            else if (userSignUpDto.ImageUrl != null)
            {
                user.ImageUrl = userSignUpDto.ImageUrl;
            }
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await CastUserRole(user, RolesTexts.WriterRole());
                return null;
            }
            else
            {
                return result.Errors;
            }

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
        public async Task<IEnumerable<IdentityError>> UpdateUserAsync(UserDto user)
        {
            var value = await GetByIDAsync(user.Id.ToString());
            value.NameSurname = user.NameSurname;
            value.Email = user.Email;
            value.UserName = user.UserName;
            value.About = user.About;
            value.City = user.City;
            if (user.Password != null && user.Password == user.PasswordAgain)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(value, user.OldPassword);
                if (checkPassword)
                    value.PasswordHash = _userManager.PasswordHasher.HashPassword(value, user.Password);
            }
            if (user.ProfileImageFile != null)
            {
                user.ImageUrl = await ImageFileManager.ImageAddAsync(user.ProfileImageFile,
                    ImageFileManager.StaticProfileImageLocation());
            }
            else if (user.ImageUrl != null && user.ImageUrl != "")
            {
                value.ImageUrl = user.ImageUrl;
            }
            var result = await _userManager.UpdateAsync(value);
            if (result.Succeeded)
            {
                var mailTemplate = Mapper.Map<ChangedUserInformationModel>(value);
                _mailService.SendMail(user.Email, MailTemplates.ChangedUserInformationMailSubject(),
                    MailTemplates.ChangedUserInformationMailTemplate(mailTemplate));
                return null;
            }
            else
                return result.Errors;
        }
        [ValidationAspect(typeof(UserValidator))]
        public async Task<IEnumerable<IdentityError>> UpdateUserForAdminAsync(UserDto user)
        {
            var value = await GetByIDAsync(user.Id.ToString());
            value.NameSurname = user.NameSurname;
            value.Email = user.Email;
            value.UserName = user.UserName;
            value.About = user.About;
            value.City = user.City;
            user.ImageUrl = value.ImageUrl;
            var result = await _userManager.UpdateAsync(value);
            if (result.Succeeded)
            {
                var mailTemplate = Mapper.Map<ChangedUserInformationModel>(value);
                _mailService.SendMail(user.Email, MailTemplates.ChangedUserInformationMailSubject(),
                    MailTemplates.ChangedUserInformationByAdminMailTemplate(mailTemplate));
                return null;
            }
            else
                return result.Errors;
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

        public async Task<List<string>> GetUserRoleListAsync(AppUser user)
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
        /// <summary>
        /// Eğer verilirse filtreye göre verilmez bütün kullanıcı listesi Döner
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Kullanıcı listesi döner.</returns>
        public async Task<List<AppUser>> GetUserListAsync(Expression<Func<AppUser, bool>> filter = null)
        {
            if (filter == null)
                return await _userManager.Users.ToListAsync();
            else
                return await _userManager.Users.Where(filter).ToListAsync();
        }
        /// <summary>
        /// Kullanıcıları belirli bir süre yasaklamayı sağlayan mekanizma.
        /// Adminlerin birbirini banlayamaması için kontrol mekanizması var.
        /// Kullanıcılara mail olarak bilgi veriliyor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="expiration"></param>
        /// <param name="banMessageContent"></param>
        /// <returns></returns>
        public async Task<bool> BannedUser(string id, DateTime expiration, string banMessageContent)
        {
            var user = await GetByIDAsync(id);
            if (user == null)
                return false;
            var resultSetLockot = await _userManager.SetLockoutEnabledAsync(user, true);
            if (resultSetLockot.Succeeded)
            {
                if (expiration > DateTime.Now)
                {
                    var isExistUserRole = await _userManager.IsInRoleAsync(user, RolesTexts.AdminRole());
                    if (isExistUserRole)
                        return false;
                    var resultSetLockotEndDate = await _userManager.SetLockoutEndDateAsync(user, expiration);
                    if (resultSetLockotEndDate.Succeeded)
                    {
                        if (banMessageContent == "" || banMessageContent == null)
                            banMessageContent = MailTemplates.BanMessageContent(expiration);
                        _mailService.SendMail(user.Email, MailTemplates.BanMessageSubject(), banMessageContent);
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Kullanıcının yasaklamasını açılmasını sağlayan mekanizma.
        /// Kullanıcılara mail olarak bilgi veriliyor.
        /// </summary>
        /// <param name="id">Kullanıcının id değeri</param>
        /// <returns>İşlem başarılı ise true değil ise false döner.</returns>
        public async Task<bool> BanOpenUser(string id)
        {
            var user = await GetByIDAsync(id);
            if (user == null)
                return false;
            var result = await _userManager.SetLockoutEndDateAsync(user, DateTime.Now);
            if (result.Succeeded)
            {
                _mailService.SendMail(user.Email, MailTemplates.BanOpenUserSubjectTemplate(),
                    MailTemplates.BanOpenUserContentTemplate());
            }
            return result.Succeeded;
        }
        public async Task<string> GetPasswordResetTokenAsync(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            if (user == null)
            {
                return null;
            }
            var result = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (result != null)
            {
                return result;
            }
            return null;
        }
        public async Task<bool> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
                _mailService.SendMail(user.Email, MailTemplates.ResetPasswordInformationSubject(),
                    MailTemplates.ResetPasswordInformationMessage());
                return result.Succeeded;
            }
            return false;
        }
    }
}
