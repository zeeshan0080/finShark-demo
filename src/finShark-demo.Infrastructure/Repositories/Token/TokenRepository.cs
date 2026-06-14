using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using finShark_demo.Core.Entities;
using finShark_demo.Core.Interfaces.Token;
using Microsoft.EntityFrameworkCore;

namespace finShark_demo.Infrastructure.Repositories.Token
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _context;

        public TokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRefreshToken(UserRefreshToken tokenDetails)
        {
            await _context.UserRefreshToken.AddAsync(tokenDetails);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserRefreshToken?> GetByUserIdAsync(int id)
        {
            return await _context.UserRefreshToken
                .Where(u => u.UserId == id)
                .OrderByDescending(u => u.Id)
                .FirstOrDefaultAsync();
        }

    }
}