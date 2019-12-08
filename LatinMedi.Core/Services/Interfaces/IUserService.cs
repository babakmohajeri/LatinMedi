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

        Task<bool> SaveAsync();
    }
}
