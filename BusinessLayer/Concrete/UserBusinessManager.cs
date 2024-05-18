using AutoMapper;
using BusinessLayer.Abstract;
using BusinessLayer.Constants;
using BusinessLayer.Models;
using BusinessLayer.StaticTexts;
using BusinessLayer.ValidationRules;
using CoreLayer.Aspects.AutoFac.Caching;
using CoreLayer.Aspects.AutoFac.Logging;
using CoreLayer.Aspects.AutoFac.Validation;
using CoreLayer.CrossCuttingConcerns.Logging.Log4Net.Loggers;
using CoreLayer.Utilities.Business;
using CoreLayer.Utilities.FileUtilities;
using CoreLayer.Utilities.MailUtilities;
using CoreLayer.Utilities.Results;
using EntityLayer.Concrete;
using EntityLayer.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserBusinessManager : ManagerBase, IUserBusinessService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMailService _mailService;
        private readonly IHttpContextAccessor _httpContext;

        public UserBusinessManager(UserManager<AppUser> userManager, IMapper mapper, IMailService mailService, IHttpContextAccessor httpContext) : base(mapper)
        {
            _userManager = userManager;
            _mailService = mailService;
            _httpContext = httpContext;
        }

        [CacheRemoveAspect("IUserBusinessService.Get")]
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
                user.ImageUrl = ImageFileManager.ImageAdd(image, ContentFileLocations.StaticProfileImageLocation(), ImageResulotions.GetProfileImageResolution());
            }
            else if (userSignUpDto.ImageFile != null)
            {
                user.ImageUrl = ImageFileManager.ImageAdd(userSignUpDto.ImageFile,
                    ContentFileLocations.StaticProfileImageLocation(), ImageResulotions.GetProfileImageResolution());
                if (user.ImageUrl == null)
                {
                    return new ErrorDataResult<IdentityResult>(Messages.UserProfileImageNotUploadError);
                }
            }
            else
            {
                return new ErrorDataResult<IdentityResult>(Messages.UserProfileImageNotUploading);
            }

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await CastUserRole(user, RolesTexts.WriterRole());
                return new SuccessDataResult<IdentityResult>(result);
            }

            return new ErrorDataResult<IdentityResult>(result);
        }

        [CacheRemoveAspect("IUserBusinessService.Get")]
        public async Task<IResultObject> CastUserRole(AppUser user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
            return new SuccessResult();
        }

        [CacheRemoveAspect("IMessageService.Get")]
        [CacheRemoveAspect("IBlogService.Get")]
        [CacheRemoveAspect("IUserBusinessService.Get")]
        public async Task<IResultObject> DeleteUserAsync(AppUser t)
        {
            await _userManager.DeleteAsync(t);
            return new SuccessResult();
        }

        [CacheAspect]
        public async Task<IDataResult<AppUser>> GetByIDAsync(string id)
        {
            if (id != string.Empty)
            {
                return new SuccessDataResult<AppUser>(await _userManager.FindByIdAsync(id));
            }
            return new ErrorDataResult<AppUser>();
        }

        [CacheRemoveAspect("IMessageService.Get")]
        [CacheRemoveAspect("IBlogService.Get")]
        [CacheRemoveAspect("IUserBusinessService.Get")]
        [ValidationAspect(typeof(UserDtoValidator))]
        public async Task<IDataResult<IdentityResult>> UpdateUserAsync(UserDto user)
        {
            var value = await GetByIDAsync(user.Id.ToString());

            if (!value.Success)
            {
                return new ErrorDataResult<IdentityResult>(Messages.UserNotFound);
            }

            value.Data.UserName = user.UserName;
            value.Data.NameSurname = user.NameSurname;
            value.Data.Email = user.Email;
            value.Data.About = user.About;
            value.Data.City = user.City;

            if (user.Password != null && user.Password == user.PasswordAgain)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(value.Data, user.OldPassword);
                if (checkPassword)
                    value.Data.PasswordHash = _userManager.PasswordHasher.HashPassword(value.Data, user.Password);
            }

            if (user.ImageUrl != null)
            {
                var image = ImageFileManager.DownloadImage(user.ImageUrl);
                if (image == null)
                {
                    return new ErrorDataResult<IdentityResult>("Profil resminiz, girdiğiniz linkten getirilemedi.");
                }
                DeleteFileManager.DeleteFile(value.Data.ImageUrl);
                value.Data.ImageUrl = ImageFileManager.ImageAdd(image, ContentFileLocations.StaticProfileImageLocation(), ImageResulotions.GetProfileImageResolution());
            }
            else if (user.ProfileImageFile != null)
            {
                DeleteFileManager.DeleteFile(value.Data.ImageUrl);
                value.Data.ImageUrl = ImageFileManager.ImageAdd(user.ProfileImageFile, ContentFileLocations.StaticProfileImageLocation(), ImageResulotions.GetProfileImageResolution());
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

        [CacheRemoveAspect("IMessageService.Get")]
        [CacheRemoveAspect("IBlogService.Get")]
        [CacheRemoveAspect("IUserBusinessService.Get")]
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
            value.EmailConfirmed = user.EmailConfirmed;

            var result = await _userManager.UpdateAsync(value);

            if (result.Succeeded)
            {
                var mailTemplate = Mapper.Map<ChangedUserInformationModel>(value);
                _mailService.SendMail(user.Email, MailTemplates.ChangedUserInformationMailSubject(),
                    MailTemplates.ChangedUserInformationByAdminMailTemplate(mailTemplate, GetBaseUrl()));
                return new SuccessDataResult<IdentityResult>(result);
            }
            else
                return new ErrorDataResult<IdentityResult>(result, result.Errors.First().Description);
        }

        public async Task<IDataResult<UserDto>> GetByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var userDto = Mapper.Map<UserDto>(user);
                return new SuccessDataResult<UserDto>(userDto);
            }

            return new ErrorDataResult<UserDto>(Messages.UserNotFound);
        }

        [CacheAspect]
        public async Task<IDataResult<UserDto>> GetByUserNameForUpdateAsync(string userName)
        {
            var user = await GetByUserNameAsync(userName);

            if (!user.Success)
            {
                return new ErrorDataResult<UserDto>(user.Message);
            }

            var userData = user.Data;
            if (userData.ImageUrl[..5] != "http" || userData.ImageUrl[..5] != "https")
            {
                userData.ImageUrl = null;
            }
            return new SuccessDataResult<UserDto>(userData);
        }

        [CacheAspect]
        public async Task<IDataResult<UserDto>> GetByMailAsync(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);

            if (user == null)
            {
                return new ErrorDataResult<UserDto>(Messages.UserNotFound);
            }

            var userDto = Mapper.Map<UserDto>(user);
            return new SuccessDataResult<UserDto>(userDto);
        }

        [CacheAspect]
        public async Task<IDataResult<List<string>>> GetUserRoleListAsync(AppUser user)
        {
            var value = await _userManager.GetRolesAsync(user);
            return new SuccessDataResult<List<string>>(value.ToList());
        }

        [CacheAspect]
        public async Task<IDataResult<int>> GetByUserCountAsync()
        {
            return new SuccessDataResult<int>(await _userManager.Users.CountAsync());

        }
        /// <summary>
        /// Eğer verilirse filtreye göre verilmez bütün kullanıcı listesi Döner
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Kullanıcı listesi döner.</returns>

        public async Task<IDataResult<List<AppUser>>> GetUserListAsync()
        {
            return new SuccessDataResult<List<AppUser>>(await _userManager.Users.ToListAsync());
        }

        [CacheAspect]
        public async Task<IDataResult<List<AppUser>>> GetUserListByUserNameAsync(string userName)
        {
            if (userName != null)
            {
                var data = await _userManager.Users.Where(x => x.UserName.ToLower().Contains(userName.ToLower())).ToListAsync();
                if (data != null)
                {
                    return new SuccessDataResult<List<AppUser>>(data);
                }
            }

            return new ErrorDataResult<List<AppUser>>(Messages.UserNotFound);
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
        [CacheRemoveAspect("IMessageService.Get")]
        [CacheRemoveAspect("IBlogService.Get")]
        [CacheRemoveAspect("IUserBusinessService.Get")]
        [LogAspect(typeof(DatabaseLogger))]
        public async Task<IResultObject> BannedUser(string id, DateTime expiration, string banMessageContent)
        {
            var user = await GetByIDAsync(id);
            var userData = user.Data;
            if (userData == null)
                return new ErrorResult(user.Message);

            if (expiration > DateTime.Now)
            {
                var isExistUserRole = await _userManager.IsInRoleAsync(userData, RolesTexts.AdminRole());
                if (isExistUserRole)
                    return new ErrorResult(Messages.AdminNotBanned);


                if (banMessageContent == "" || banMessageContent == null)
                    banMessageContent = MailTemplates.BanMessageContent(expiration);
                _mailService.SendMail(userData.Email, MailTemplates.BanMessageSubject(), banMessageContent);
                userData.LockoutEnd = expiration;
                await _userManager.UpdateAsync(userData);
                return new SuccessResult();

            }
            return new ErrorResult(Messages.BannedLaterThanTheCurrentDate);
        }
        /// <summary>
        /// Kullanıcının yasaklamasını açılmasını sağlayan mekanizma.
        /// Kullanıcılara mail olarak bilgi veriliyor.
        /// </summary>
        /// <param name="id">Kullanıcının id değeri</param>
        /// <returns>İşlem başarılı ise true değil ise false döner.</returns>
        [CacheRemoveAspect("IMessageService.Get")]
        [CacheRemoveAspect("IBlogService.Get")]
        [CacheRemoveAspect("IUserBusinessService.Get")]
        public async Task<IResultObject> BanOpenUser(string id)
        {
            var user = await GetByIDAsync(id);

            var userDto = Mapper.Map<UserDto>(user.Data);

            IResultObject businessRulesResult = BusinessRules.Run(UserNotEmpty(new SuccessDataResult<UserDto>(userDto)));

            if (!businessRulesResult.Success)
            {
                return businessRulesResult;
            }

            var userData = user.Data;

            _mailService.SendMail(userData.Email, MailTemplates.BanOpenUserSubjectTemplate(),
                MailTemplates.BanOpenUserContentTemplate());

            userData.LockoutEnd = DateTime.UtcNow;
            await _userManager.UpdateAsync(userData);

            return new SuccessResult();
        }

        public async Task<IDataResult<string>> GetPasswordResetTokenAsync(string mail)
        {
            var user = await GetByMailAsync(mail);
            if (!user.Success)
            {
                return new ErrorDataResult<string>(user.Message);
            }

            var result = await _userManager.GeneratePasswordResetTokenAsync(user.Data);
            if (result != null)
            {
                return new SuccessDataResult<string>(result, null);
            }

            return new ErrorDataResult<string>(result);
        }

        [CacheRemoveAspect("IUserBusinessService.Get")]
        public async Task<IDataResult<IdentityResult>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (result.Succeeded)
            {
                _mailService.SendMail(user.Email, MailTemplates.ResetPasswordInformationSubject(),
                MailTemplates.ResetPasswordInformationMessage());
                return new SuccessDataResult<IdentityResult>(result);
            }

            string errorMessages = "";
            foreach (var error in result.Errors)
                errorMessages += "\n" + error.Description;

            return new ErrorDataResult<IdentityResult>(result, errorMessages);
        }

        public async Task<IDataResult<string>> CreateMailTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ErrorDataResult<string>(message: "Kullanıcı bulunamadı.");
            }

            if (user.EmailConfirmed)
            {
                return new ErrorDataResult<string>(message: "Kullanıcı e-maili zaten onaylı.");
            }

            if (user.MailVerifyCodeSendTime != null)
            {
                var dateRange = DateTime.Now - user.MailVerifyCodeSendTime;
                if (dateRange.Value.Minutes < 5)
                {
                    return new ErrorDataResult<string>(message: Messages.UserConfirmationCodeCannotBeSubmittedAfterFiveMinutes);
                }
            }

            user.MailVerifyCodeSendTime = DateTime.Now;
            var updateTask = await _userManager.UpdateAsync(user);
            if (!updateTask.Succeeded)
            {
                return new ErrorDataResult<string>(message: updateTask.Errors.First().Description);
            }

            return new SuccessDataResult<string>(await _userManager.GenerateEmailConfirmationTokenAsync(user), "Başarılı");
        }

        public async Task<IResultObject> ConfirmMailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return new SuccessResult();
            }
            return new ErrorResult(result.Errors.First().Description);
        }

        [CacheAspect]
        public async Task<IDataResult<DateTimeOffset?>> GetBanDateAsync(string userName = null)
        {
            AppUser user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == userName);

            if (user.LockoutEnd == null)
            {
                return new SuccessDataResult<DateTimeOffset?>();
            }

            return new SuccessDataResult<DateTimeOffset?>(user.LockoutEnd);
        }

        private string GetBaseUrl()
        {
            return $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}{_httpContext.HttpContext.Request.PathBase}";
        }
    }
}