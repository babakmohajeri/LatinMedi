using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LatinMedia.Core.Services.Interfaces
{
    public interface IUserService
    {

        #region Account

        bool IsExistEmail(string email);

        Task<bool> IsExistMobileAsync(string mobile);

        Task<bool> AddUserAsync(User user);

        Task<bool> AddUserAsync();

        Task<User> LoginUser(string email, string password);

        Task<bool> ActivateAccount(string Icode);

        Task<User> ForgotPassword(string email);

        User GetUserByActiveCode(string ActiveCode);
        User GetUserByEmail(string email);

        void UpdateUser(User user);

        #endregion

        #region UserPanel

        InformationUserViewModel UserInformation(string email);

        EditUserProfileViewModel GetUserInformationForEdit(string email);

        void UpdateUserProfile(EditUserProfileViewModel userProfile);

        void EditProfile(string email, EditUserProfileViewModel profile);

        void ChangePassword(string email, ChangePasswordViewModel newPass);

        #endregion

    }
}
