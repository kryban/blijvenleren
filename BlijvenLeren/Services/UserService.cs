using BlijvenLeren.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlijvenLeren.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;

            new List<string>("Extern,Intern".Split(',')).ForEach(s => CreateRole(s));
        }

        public async Task<IdentityResult> CreateUser(RegisterViewModel model, string password)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            await _userManager.CreateAsync(user, password);

            var result = await _userManager.AddToRoleAsync(user, model.UserRole);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
            }

            return result;
        }

        public async Task<SignInResult> Login(LoginViewModel user)
        {
            return await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

        }



        private async Task<IQueryable<IdentityRole>> ReturnAllRoles()
        {
            return _roleManager.Roles;
        }

        private void CreateRole(string name)
        {
            var exist = _roleManager.RoleExistsAsync(name).Result;
            if (!exist)
            {
                _roleManager.CreateAsync(new IdentityRole(name));
            }
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
