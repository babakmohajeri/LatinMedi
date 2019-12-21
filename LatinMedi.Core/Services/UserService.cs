using LatinMedia.Core.Convertor;
using LatinMedia.Core.Generators;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.Core.ViewModels;
using LatinMedia.DataLayer.Context;
using LatinMedia.DataLayer.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatinMedia.Core.Services
{
    public class UserService : IUserService
    {
        private LatinMediaDbContext _context;
        private readonly IHostingEnvironment _environment;

        public UserService(LatinMediaDbContext context, IHostingEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
                // return await SaveAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }     

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public async Task<bool> IsExistMobileAsync(string mobile)
        {
            return await _context.Users.AnyAsync(u => u.Mobile == mobile);
        }

        public async Task<bool> AddUserAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

         public async Task<User> LoginUser(string email, string password)
        {
            string Mail = FixedText.FixedEmail(email);
            string Pass = PasswordHelper.EncodePasswordMd5(password);
            
            User user = await _context.Users.Where(u => u.Email == Mail && u.Password == Pass).FirstOrDefaultAsync();
            return user;
        }

        public async Task<bool> ActivateAccount(string code)
        {
            User user = await _context.Users.SingleOrDefaultAsync(u => u.AcrivationCode == code);
            if (user == null)
            {
                return false;
            }
            else
            {
                user.IsActive = true;
                user.AcrivationCode = GeneratorName.GenerateGUID();

                try
                {
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task<User> ForgotPassword(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public User GetUserByActiveCode(string ActiveCode)
        {
            return _context.Users.SingleOrDefault(u => u.AcrivationCode == ActiveCode);
        }

        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }

        public InformationUserViewModel UserInformation(string email)
        {
            User user = _context.Users.SingleOrDefault(u => u.Email == email);
            if (user == null)
            {
                return null;
            }
            InformationUserViewModel information = new InformationUserViewModel()
            {
                FirstName=user.FirstName,
                LastName=user.LastName,
                Email=user.Email,
                Mobile=user.Mobile,
                RegisterDate=user.CreateDate,
                Wallet=0,
                UserAvatar=user.UserAvatar
            };
            
            return information;
            
        }

        public EditUserProfileViewModel GetUserInformationForEdit(string email)
        {
            return _context.Users.Where(u => u.Email == email).Select(u => new EditUserProfileViewModel() {
                FirstName=u.FirstName,
                LastName=u.LastName,
                Mobile=u.Mobile,
                AvatarName=u.UserAvatar
            }).Single();
        }

        public void UpdateUserProfile(EditUserProfileViewModel userProfile)
        {
            User user = new User()
            {
                FirstName= userProfile.FirstName,
                LastName= userProfile.LastName,
                Mobile= userProfile.Mobile,
                UserAvatar=userProfile.AvatarName
            };

            _context.Update(user);
            _context.SaveChanges();
        }

        public void EditProfile(string email, EditUserProfileViewModel profile)
        {
            if (profile.UserAvatar != null)
            {
                string ImagePath = "";
                if (profile.AvatarName != "default.png")
                {
                    ImagePath = Path.Combine(_environment.WebRootPath, "Images/UserAvatar", profile.AvatarName);
                    if (File.Exists(ImagePath))
                    {
                        File.Delete(ImagePath);
                    }
                }
                profile.AvatarName = GeneratorName.GenerateGUID() + Path.GetExtension(profile.UserAvatar.FileName);
                ImagePath = Path.Combine(_environment.WebRootPath, "Images/UserAvatar", profile.AvatarName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    profile.UserAvatar.CopyTo(stream);
                }

            }
            var user = GetUserByEmail(email);
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.Mobile = profile.Mobile;
            user.UserAvatar = profile.AvatarName;

            UpdateUser(user);
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        public void ChangePassword(string email, ChangePasswordViewModel newPass)
        {
            User user = _context.Users.Single(u => u.Email == email);
            if (user != null)
            {
                string OldPass = PasswordHelper.EncodePasswordMd5(newPass.OldPassword);
                if (user.Password == OldPass)
                {
                    string newPassword = PasswordHelper.EncodePasswordMd5(newPass.NewPassword);
                    user.Password = newPassword;
                    UpdateUser(user);
                }
            }
        }
    }
}
