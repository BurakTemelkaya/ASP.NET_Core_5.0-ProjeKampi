using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
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
    public class UserManager : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserManager(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async void RegisterUserAsync(AppUser T, string password)
        {
            await _userManager.CreateAsync(T, password);
        }

        public async void DeleteUserAsync(AppUser t)
        {
            await _userManager.DeleteAsync(t);
        }

        public async Task<AppUser> GetByIDAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        public async Task<AppUser> GetByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async void UpdateUserAsync(AppUser t)
        {
            await _userManager.UpdateAsync(t);
        }
    }
}
