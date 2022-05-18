using BookStoreModels;
using BookStoreModels.AuthenticationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Authentication
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(LoginModel model);
        Task<User> Register(RegisterModel model);
    }
}
