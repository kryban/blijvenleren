using BlijvenLeren.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BlijvenLeren.Services
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUser(RegisterViewModel model, string password);
        Task<SignInResult> Login(LoginViewModel user);
        Task Logout();
    }
}