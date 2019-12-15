using LatinMedia.Core.Convertor;
using LatinMedia.Core.Generators;
using LatinMedia.Core.Security;
using LatinMedia.Core.Services.Interfaces;
using LatinMedia.DataLayer.Context;
using LatinMedia.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LatinMedia.Core.Services
{
    public class UserService : IUserService
    {
        private LatinMediaDbContext _context;
        public UserService(LatinMediaDbContext context)
        {
            _context = context;
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
    }
}
