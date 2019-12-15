using LatinMedia.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatinMedia.Core.Services.Interfaces
{
    public interface IUserService
    {
        bool IsExistEmail(string email);

        Task<bool> IsExistMobileAsync(string mobile);

        Task<bool> AddUserAsync(User user);

        Task<bool> AddUserAsync();

        Task<User> LoginUser(string email, string password);

        Task<bool> ActivateAccount(string Icode);

        Task<User> ForgotPassword(string email);
    }
}
