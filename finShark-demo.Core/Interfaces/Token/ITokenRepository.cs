using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finShark_demo.Core.Entities;

namespace finShark_demo.Core.Interfaces.Token
{
    public interface ITokenRepository
    {
        Task<bool> AddRefreshToken(UserRefreshToken tokenDetails);
        Task<UserRefreshToken?> GetByUserIdAsync(int id);
    }
}