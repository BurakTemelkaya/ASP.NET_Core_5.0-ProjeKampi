using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using BusinessLayer.StaticTexts;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.Utilities.Business;
using CoreLayer.Utilities.FileUtilities;
using CoreLayer.Utilities.MailUtilities;
using CoreLayer.Utilities.MailUtilities.Models;
using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        [ValidationAspect(typeof(UserSignUpDtoValidator))]
        public async Task<IDataResult<IdentityResult>> RegisterUserAsync(UserSignUpDto userSignUpDto, string password)
        {
            var user = Mapper.Map<AppUser>(userSignUpDto);
            user.RegistrationTime = DateTime.Now;


            if (userSignUpDto.ImageUrl != null)
            {
                var image = ImageFileManager.DownloadImage(userSignUpDto.ImageUrl);
                if (image == null)
                {
                    return new ErrorDataResult<IdentityResult>("Profil resminiz, girdiğiniz linkten getirilemedi.");
                }
                userSignUpDto.ImageUrl = ImageFileManager.ImageAdd(image, ImageLocations.StaticProfileImageLocation(), ImageResulotions.GetProfileImageResolution());
            }
            else if (userSignUpDto.ImageFile != null)
            {
                user.ImageUrl = ImageFileManager.ImageAdd(userSignUpDto.ImageFile,
                    ImageLocations.StaticProfileImageLocation(), ImageResulotions.GetProfileImageResolution());
            }
            else
            {
                return new ErrorDataResult<IdentityResult>("Lütfen profil resmi yükleyin yada resim linki giriniz.");
            }

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await CastUserRole(user, RolesTexts.WriterRole());
                return new SuccessDataResult<IdentityResult>(result);
            }

            return new ErrorDataResult<IdentityResult>(result);
        }
        public async Task<IResult> CastUserRole(AppUser user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
            return new SuccessResult();
        }

        public async Task<IResult> DeleteUserAsync(AppUser t)
        {
            await _userManager.DeleteAsync(t);
            return new SuccessResult();
        }

        public async Task<IDataResult<AppUser>> GetByIDAsync(string id)
        {
            if (id != string.Empty)
            {
                return new SuccessDataResult<AppUser>(await _userManager.FindByIdAsync(id));
            }
            return new ErrorDataResult<AppUser>();
        }

        [ValidationAspect(typeof(UserDtoValidator))]
        public async Task<IDataResult<IdentityResult>> UpdateUserAsync(UserDto user)
        {
            var value = await GetByIDAsync(user.Id.ToString());

            if (!value.Success)
            {
                return new ErrorDataResult<IdentityResult>("Kullanıcı bulunamadı");
            }

            value.Data.UserName = user.UserName;
            value.Data.NameSurname = user.NameSurname;
            value.Data.Email = user.Email;
            value.Data.About= user.About;
            value.Data.City = user.City;

            if (user.Password != null && user.Password == user.PasswordAgain)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(user, user.OldPassword);
                if (checkPassword)
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, user.Password);
            }

            if (user.ImageUrl != null)
            {
                var image = ImageFileManager.DownloadImage(user.ImageUrl);
                if (image == null)
                {
                    return new ErrorDataResult<IdentityResult>("Profil resminiz, girdiğiniz linkten getirilemedi.");
                }
                DeleteFileManager.DeleteFile(value.Data.ImageUrl);
                value.Data.ImageUrl = ImageFileManager.ImageAdd(image, ImageLocations.StaticProfileImageLocation(), ImageResulotions.GetProfileImageResolution());               
            }

            else if (user.ProfileImageFile != null)
            {
                DeleteFileManager.DeleteFile(value.Data.ImageUrl);
                value.Data.ImageUrl = ImageFileManager.ImageAdd(user.ProfileImageFile,ImageLocations.StaticProfileImageLocation(), ImageResulotions.GetProfileImageResolution());
            }

            else
            {
                user.ImageUrl = value.Data.ImageUrl;
            }
            

            var result = await _userManager.UpdateAsync(value.Data);
            if (result.Succeeded)
            {
                var mailTemplate = Mapper.Map<ChangedUserInformationModel>(user);
                _mailService.SendMail(user.Email, MailTemplates.ChangedUserInformationMailSubject(),
                    MailTemplates.ChangedUserInformationMailTemplate(mailTemplate));
                return new SuccessDataResult<IdentityResult>(result);
            }
            else
                return new ErrorDataResult<IdentityResult>(result);
        }

        [ValidationAspect(typeof(UserDtoValidator))]
        public async Task<IDataResult<IdentityResult>> UpdateUserForAdminAsync(UserDto user)
        {
            var rawValue = await GetByIDAsync(user.Id.ToString());
            var value = rawValue.Data;

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
                return new SuccessDataResult<IdentityResult>(result);
            }
            else
                return new ErrorDataResult<IdentityResult>(result);
        }

        public async Task<IDataResult<UserDto>> FindByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var userDto = Mapper.Map<UserDto>(user);
                return new SuccessDataResult<UserDto>(userDto);
            }

            return new ErrorDataResult<UserDto>("Kullanıcı bulunamadı.");
        }
        public async Task<IDataResult<UserDto>> FindByUserNameForUpdateAsync(string userName)
        {
            var user = await FindByUserNameAsync(userName);

            if (!user.Success)
            {
                return new ErrorDataResult<UserDto>();
            }

            var userData = user.Data;
            if (userData.ImageUrl[..5] != "http" || userData.ImageUrl[..5] != "https")
            {
                userData.ImageUrl = null;
            }
            return new SuccessDataResult<UserDto>(userData);
        }

        public async Task<IDataResult<UserDto>> FindByMailAsync(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);

            var userDto = Mapper.Map<UserDto>(user);
            return new SuccessDataResult<UserDto>(userDto);
        }

        public async Task<IDataResult<List<string>>> GetUserRoleListAsync(AppUser user)
        {
            var value = await _userManager.GetRolesAsync(user);
            return new SuccessDataResult<List<string>>(value.ToList());
        }

        public async Task<IDataResult<int>> GetByUserCountAsync(Expression<Func<AppUser, bool>> filter = null)
        {
            if (filter == null)
                return new SuccessDataResult<int>(await _userManager.Users.CountAsync());
            else
                return new SuccessDataResult<int>(await _userManager.Users.CountAsync(filter));
        }
        /// <summary>
        /// Eğer verilirse filtreye göre verilmez bütün kullanıcı listesi Döner
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Kullanıcı listesi döner.</returns>
        public async Task<IDataResult<List<AppUser>>> GetUserListAsync(Expression<Func<AppUser, bool>> filter = null)
        {
            if (filter == null)
                return new SuccessDataResult<List<AppUser>>(await _userManager.Users.ToListAsync());
            else
                return new SuccessDataResult<List<AppUser>>(await _userManager.Users.Where(filter).ToListAsync());
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
        public async Task<IResult> BannedUser(string id, DateTime expiration, string banMessageContent)
        {
            var user = await GetByIDAsync(id);
            var userData = user.Data;
            if (userData == null)
                return new ErrorResult();

            var resultSetLockot = await _userManager.SetLockoutEnabledAsync(userData, true);
            if (resultSetLockot.Succeeded)
            {
                if (expiration > DateTime.Now)
                {
                    var isExistUserRole = await _userManager.IsInRoleAsync(userData, RolesTexts.AdminRole());
                    if (isExistUserRole)
                        return new ErrorResult("Adminler banlanamaz.");

                    var resultSetLockotEndDate = await _userManager.SetLockoutEndDateAsync(userData, expiration);
                    if (resultSetLockotEndDate.Succeeded)
                    {
                        if (banMessageContent == "" || banMessageContent == null)
                            banMessageContent = MailTemplates.BanMessageContent(expiration);
                        _mailService.SendMail(userData.Email, MailTemplates.BanMessageSubject(), banMessageContent);
                        return new SuccessResult();
                    }
                    return new ErrorResult(resultSetLockot.Errors.First().Description);
                }
                return new ErrorResult("Girilen ban süresi şu anki tarihten ileride olamaz.");
            }
            return new ErrorResult(resultSetLockot.Errors.First().Description);
        }
        /// <summary>
        /// Kullanıcının yasaklamasını açılmasını sağlayan mekanizma.
        /// Kullanıcılara mail olarak bilgi veriliyor.
        /// </summary>
        /// <param name="id">Kullanıcının id değeri</param>
        /// <returns>İşlem başarılı ise true değil ise false döner.</returns>
        public async Task<IResult> BanOpenUser(string id)
        {
            var user = await GetByIDAsync(id);

            var userDto = Mapper.Map<UserDto>(user);

            IResult businessRulesResult = BusinessRules.Run(UserNotEmpty(new SuccessDataResult<UserDto>(userDto)));

            if (!businessRulesResult.Success)
            {
                return businessRulesResult;
            }

            var userData = user.Data;

            var result = await _userManager.SetLockoutEndDateAsync(userData, DateTime.Now);
            if (result.Succeeded)
            {
                _mailService.SendMail(userData.Email, MailTemplates.BanOpenUserSubjectTemplate(),
                    MailTemplates.BanOpenUserContentTemplate());
                return new SuccessResult();
            }

            return new ErrorResult(result.Errors.First().Description);
        }
        public async Task<IDataResult<string>> GetPasswordResetTokenAsync(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            var result = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (result != null)
            {
                return new SuccessDataResult<string>(result);
            }
            return new ErrorDataResult<string>("Bir hata oluştu.");
        }
        public async Task<IDataResult<IdentityResult>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (result != null)
            {
                _mailService.SendMail(user.Email, MailTemplates.ResetPasswordInformationSubject(),
                MailTemplates.ResetPasswordInformationMessage());
                return new SuccessDataResult<IdentityResult>(result);
            }

            return new ErrorDataResult<IdentityResult>(result);
        }
    }
}
